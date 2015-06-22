using Animatum.Updater.Common;
using Animatum.Updater.Downloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Animatum.Updater
{
    public partial class MainForm
    {
        bool installing = false;

        void CheckForUpdate()
        {
            // download the server file
            BeginDownload(update.ServerFileSites, 0, null, false, false);
        }

        void DownloadUpdate()
        {
            //download the update file
            update.CurrentlyUpdating = UpdateOn.DownloadingUpdate;
            //BeginDownload(updateFrom.FileSites, updateFrom.Adler32, updateFrom.SignedSHA1Hash, true, false);
            BeginDownloadAll(updateFrom.FileSites, 0, null, true, false);
        }

        //download regular update files
        void BeginDownload(List<string> sites, long adler32, byte[] signedSHA1Hash, bool relativeProgress, bool checkSigning)
        {
            if (downloader != null)
            {
                downloader.ProgressChanged -= ShowProgress;
                //downloader.ProgressChanged -= SelfUpdateProgress;
            }

            downloader = new FileDownloader(sites, tempDirectory)
            {
                Adler32 = adler32,
                UseRelativeProgress = relativeProgress,
                SignedSHA1Hash = signedSHA1Hash,
                PublicSignKey = checkSigning ? update.PublicSignKey : null
            };

            downloader.ProgressChanged += ShowProgress;
            downloader.Download();
        }

        //download regular update files
        void BeginDownloadAll(List<string> sites, long adler32, byte[] signedSHA1Hash, bool relativeProgress, bool checkSigning)
        {
            if (downloader != null)
            {
                downloader.ProgressChanged -= ShowProgress;
                //downloader.ProgressChanged -= SelfUpdateProgress;
            }

            downloader = new FileDownloader(sites, tempDirectory, true)
            {
                Adler32 = adler32,
                UseRelativeProgress = relativeProgress,
                SignedSHA1Hash = signedSHA1Hash,
                PublicSignKey = checkSigning ? update.PublicSignKey : null
            };

            downloader.ProgressChanged += ShowProgress;
            downloader.Download();
        }

        void ServerDownloadedSuccessfully()
        {
            //load the server file into memory
            LoadServerFile(true);

            // Always write the server provided version
            if (QuickCheckJustCheck && String.IsNullOrEmpty(OutputInfo))
                Console.WriteLine(ServerFile.NewVersion);

            // if we went to the finish page, bail out
            if (frameOn != Frame.Checking)
                return;

            if (isAutoUpdateMode)
            {
                // set the autoupdate filename
                autoUpdateStateFile = Path.Combine(tempDirectory, "autoupdate");
            }

            // Show update info page (or just start downloading & installing)
            if (SkipUpdateInfo)
            {
                // check if elevation is needed
                needElevation = NeedElevationToUpdate();

                if (needElevation)
                    StartSelfElevated();
                else
                    ShowFrame(Frame.InstallUpdates);
            }
            else
                ShowFrame(Frame.UpdateInfo);
        }

        //returns True if an update is necessary, otherwise false
        void LoadServerFile(bool setChangesText)
        {
            //load the server file
            ServerFile = ServerFile.Load(serverFileLoc);

            updateFilename = serverFileLoc;

            clientLang.NewVersion = ServerFile.NewVersion;

            // if no update is needed...
            if (VersionTools.Compare(update.InstalledVersion, ServerFile.NewVersion) >= 0)
            {
                if (isAutoUpdateMode)
                {
                    // send reponse that there's no update available
                    updateHelper.SendSuccess(null, null, true);

                    // close this client
                    isCancelled = true;

                    // let wyUpdate cleanup the files
                    isAutoUpdateMode = false;

                    // let ServerDownloadedSuccessfully() exit early
                    frameOn = Frame.AlreadyUpToDate;

                    Close();

                    return;
                }

                // Show "All Finished" page
                ShowFrame(Frame.AlreadyUpToDate);
                return;
            }

            // get the correct update file to download
            updateFrom = ServerFile.GetVersionChoice(update.InstalledVersion);

            // if the update install the x64 system32 folder on an x86 machine we need to throw an error
            if ((updateFrom.InstallingTo & InstallingTo.SysDirx64) == InstallingTo.SysDirx64 && !SystemFolders.Is64Bit())
            {
                error = "Update available, but can't install 64-bit files on a 32-bit machine.";
                errorDetails = "There's an update available (version " + ServerFile.NewVersion + "). However, this update will install files to the x64 (64-bit) system32 folder. And because this machine is an x86 (32-bit), there isn't an x64 system32 folder.";

                ShowFrame(Frame.Error);
                return;
            }

            // if the update install the x64 system32 folder on an x86 machine we need to throw an error
            if ((updateFrom.InstallingTo & InstallingTo.CommonFilesx64) == InstallingTo.CommonFilesx64 && !SystemFolders.Is64Bit())
            {
                error = "Update available, but can't install 64-bit files on a 32-bit machine.";
                errorDetails = "There's an update available (version " + ServerFile.NewVersion + "). However, this update will install files to the x64 (64-bit) \"Program File\\Common Files\" folder. And because this machine is an x86 (32-bit), there isn't an x64 \"Program File\\Common Files\" folder.";

                ShowFrame(Frame.Error);
                return;
            }

            // Update the client language variables
            if (VersionTools.Compare(update.InstalledVersion, "1.0.0.0") < 0)
            {
                clientLang.UpdateInfo.Content = String.Format("Version {0} of Animatum is ready to be installed. Listed below is its current changelog:", ServerFile.NewVersion);
                clientLang.UpdateBottom = "Click Install to begin.";

                clientLang.DownInstall.Content = "Animatum Updater is installing Animatum. This process could take a few minutes.";

                clientLang.Download = "Downloading Animatum and its dependecies";

                clientLang.SuccessUpdate.Content = String.Format("Animatum has been successfully updated to version {0}", ServerFile.NewVersion);
                clientLang.FinishBottom = "Click Finish to exit and start Animatum.";

                installing = true;
            }
            else
            {
                clientLang.UpdateInfo.Content = String.Format("The version of Animatum installed on this computer is {0}. The latest version is {1}. Listed below are the changes and improvements:",
                    update.InstalledVersion, ServerFile.NewVersion);
            }

            // set the changes text
            if (setChangesText || isAutoUpdateMode)
            {
                int i = ServerFile.VersionChoices.IndexOf(updateFrom);

                //if there's a catch-all update start with one less than "update.VersionChoices.Count - 1"

                //build the changes from all previous versions
                for (int j = ServerFile.VersionChoices.Count - 1; j >= i; j--)
                {
                    //show the version number for previous updates we may have missed
                    if (j != ServerFile.VersionChoices.Count - 1 && (!ServerFile.CatchAllUpdateExists || ServerFile.CatchAllUpdateExists && j != ServerFile.VersionChoices.Count - 2))
                        panelDisplaying.AppendAndBoldText("\r\n\r\n" + ServerFile.VersionChoices[j + 1].Version + ":\r\n\r\n");

                    // append the changes to the total changes list
                    if (!ServerFile.CatchAllUpdateExists || ServerFile.CatchAllUpdateExists && j != ServerFile.VersionChoices.Count - 2)
                    {
                        if (ServerFile.VersionChoices[j].RTFChanges)
                            panelDisplaying.AppendRichText(ServerFile.VersionChoices[j].Changes);
                        else
                            panelDisplaying.AppendText(ServerFile.VersionChoices[j].Changes);
                    }
                }
            }
        }
    }
}
