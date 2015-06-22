using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Animatum.Updater.Controls;
using Animatum.Updater.Common;

namespace Animatum.Updater
{
    public partial class MainForm
    {
        // Automatic Update Mode (aka API mode)
        readonly UpdateHelper updateHelper = new UpdateHelper();
        //bool isAutoUpdateMode;
        string autoUpdateStateFile;

        UpdateStep autoUpdateStepProcessing;


        bool currentlyExtracting;

        // is this instance of wyUpdate the New Self
        public bool IsNewSelf;

        bool beginAutoUpdateInstallation;

        string oldAUTempFolder;

        void SetupAutoupdateMode()
        {
            updateHelper.SenderProcessClosed += UpdateHelper_SenderProcessClosed;
            updateHelper.RequestReceived += UpdateHelper_RequestReceived;
            updateHelper.StartPipeServer(this);
        }

        /// <summary>This starts the PipeServer and waits for at least one "client" before delivering the error.</summary>
        void StartQuickAndDirtyAutoUpdateMode()
        {
            updateHelper.StartPipeServer(this);

            // there must be at least one client running to receive this message
            int timeSpent = 0;

            while (updateHelper.TotalConnectedClients == 0)
            {
                // if we've already waited 30 seconds, we've wait long enough
                // something has gone wrong with the AutomaticUpdater control
                // no point in waiting around any longer.
                if (timeSpent == 30000)
                    break;

                // wait 1/3 of a second
                timeSpent += 300;
                Thread.Sleep(300);
            }
        }

        /*
        void StartNewSelfAndClose()
        {
            bool checkForClients = false;

            // when this function is called in the constructor
            // (i.e. before the handle for the form is created)
            // then the pipeserver will not have yet been created
            if (!updateHelper.RunningServer)
            {
                checkForClients = true;
                updateHelper.StartPipeServer(this);
            }

            Process clientProcess = new Process
                                        {
                                            StartInfo =
                                                {
                                                    FileName = newSelfLocation,

                                                    //NOTE: (Very goddamn important - change this and die)
                                                    // Arguments must have the "clear space" before the closing quote after
                                                    // baseDirectory. "Why?" you ask, because Windows is the offspring
                                                    // of a Unicorn and an Angel. Everyone knows that Angels don't
                                                    // respect backslash-quote combos.
                                                    // And Unicorns are racists.

                                                    // The non-absurd reason is that if you have a baseDirectory variable with
                                                    // a trailing slash then a quote character adjacent to this slash (i.e.
                                                    // with no space between the slash and quote) the commandline args
                                                    // get fubar-ed. A base directory with a trailing slash is valid
                                                    // input, thus the slash-space-quote combo must remain.

                                                    // start the client in automatic update mode (a.k.a. wait mode)
                                                    Arguments =
                                                        "-cdata:\"" + clientFileLoc + "\" -basedir:\"" + baseDirectory +
                                                        " \" /autoupdate /ns",

                                                    WindowStyle = ProcessWindowStyle.Hidden
                                                }
                                        };

            clientProcess.Start();

            if (checkForClients)
            {
                // there must be at least one client running to receive this message
                int timeSpent = 0;

                while (updateHelper.TotalConnectedClients == 0)
                {
                    // if we've already waited 30 seconds, we've wait long enough
                    // something has gone wrong with the AutomaticUpdater control
                    // no point in waiting around any longer.
                    if (timeSpent == 30000)
                        break;

                    // wait 1/3 of a second
                    timeSpent += 300;
                    Thread.Sleep(300);
                }
            }

            // tell all the clients that there's a new wyUpdate
            updateHelper.SendNewWyUpdate(UpdateHelperData.PipenameFromFilename(newSelfLocation), clientProcess.Id);

            CancelUpdate(true);
        }
         * */

        void UpdateHelper_RequestReceived(object sender, UpdateAction a, UpdateStep s)
        {
            if (a == UpdateAction.Cancel)
            {
                CancelUpdate(true);
                return;
            }

            // filter out-of-order requests (never assume the step 's' is coming in the correct order)
            if (FilterBadRequest(s))
                return;

            // set the current update step (ForceCheck == Check)
            autoUpdateStepProcessing = s == UpdateStep.ForceRecheckForUpdate ? UpdateStep.CheckForUpdate : s;

            switch (s)
            {
                case UpdateStep.ForceRecheckForUpdate:

                    //TODO: perhaps delete old crufty files

                    // show the checking frame regardless of the current step
                    panelDisplaying.ClearText();
                    ShowFrame(Frame.Checking);
                    
                    CheckForUpdate();

                    break;

                case UpdateStep.CheckForUpdate:

                    CheckForUpdate();

                    break;
                case UpdateStep.DownloadUpdate:

                    ShowFrame(Frame.InstallUpdates);
                    DownloadUpdate();

                    break;
                case UpdateStep.BeginExtraction:

                    update.CurrentlyUpdating = UpdateOn.Extracting;
                    InstallUpdates(update.CurrentlyUpdating);

                    break;
                case UpdateStep.RestartInfo:

                    // send a success signal (with the Window Handle)
                    updateHelper.SendSuccess(autoUpdateStepProcessing, (int)Handle);

                    break;
                case UpdateStep.Install:

                    if (!updateHelper.IsAService)
                    {
                        // show self & make topmost
                        Visible = true;
                        TopMost = true;
                        TopMost = false;
                    }

                    if (needElevation)
                    {
                        // save the RestartInfo details (file to launch, where to save the update success details)
                        //SaveAutoUpdateData(UpdateStepOn.UpdateReadyToInstall);

                        StartSelfElevated();
                        return;
                    }

                    // install the regular update
                    update.CurrentlyUpdating = UpdateOn.ClosingProcesses;
                    InstallUpdates(update.CurrentlyUpdating);

                    break;
            }
        }

        void UpdateHelper_SenderProcessClosed(object sender, EventArgs e)
        {
            // close wyUpdate if we're not installing an update
            if (isAutoUpdateMode && !updateHelper.Installing)
            {
                // if the restart info was already sent, then start installing
                // otherwise, cancel the update
                if (updateHelper.RestartInfoSent)
                {
                    updateHelper.Installing = true;
                    UpdateHelper_RequestReceived(null, UpdateAction.UpdateStep, UpdateStep.Install);
                }
                else
                    CancelUpdate(true);
            }
        }

        /// <summary>Filters bad request by responding with the required info.</summary>
        /// <param name="s">The requested step.</param>
        /// <returns>True if a bad request has been filtered, false otherwise</returns>
        bool FilterBadRequest(UpdateStep s)
        {
            switch (s)
            {
                case UpdateStep.ForceRecheckForUpdate:
                    // if already checking ...
                    if (frameOn == Frame.Checking && downloader != null)
                    {
                        // report progress of 0%
                        updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
                        return true;
                    }

                    // ignore request when a download / extraction is pending
                    if (frameOn != Frame.Checking && frameOn != Frame.UpdateInfo)
                        return true;
                    
                    break;
                case UpdateStep.CheckForUpdate:

                    // if already checking ...
                    if (frameOn == Frame.Checking && downloader != null)
                    {
                        // report progress of 0%
                        updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
                        return true;
                    }

                    // if on another step ...
                    if (frameOn != Frame.Checking)
                    {
                        // report UpdateAvailable, with changes
                        updateHelper.SendSuccess(ServerFile.NewVersion, panelDisplaying.GetChanges(true), true);

                        return true;
                    }

                    break;

                case UpdateStep.DownloadUpdate:

                    if (frameOn == Frame.Checking)
                    {
                        // waiting to be told to check for updates...
                        if (downloader == null)
                        {
                            autoUpdateStepProcessing = UpdateStep.CheckForUpdate;

                            // report 0% and begin checking
                            updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
                            CheckForUpdate();
                        }
                        else // already checking ...
                        {
                            // report 0% progress
                            updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
                        }

                        return true;
                    }
                    
                    if (frameOn == Frame.InstallUpdates)
                    {
                        // if already downloading ...
                        if (IsInDownloadState())
                        {
                            // report 0%
                            updateHelper.SendProgress(0, UpdateStep.DownloadUpdate);
                        }
                        else // on another step (extracting, etc.) ...
                        {
                            // report UpdateDownloaded
                            updateHelper.SendSuccess(UpdateStep.DownloadUpdate);
                        }

                        return true;
                    }

                    break;

                case UpdateStep.BeginExtraction:

                    if (frameOn == Frame.Checking)
                    {
                        // waiting to be told to check for updates...
                        if (downloader == null)
                        {
                            autoUpdateStepProcessing = UpdateStep.CheckForUpdate;

                            // report 0% and begin checking
                            updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
                            CheckForUpdate();
                        }
                        else // already checking ...
                        {
                            // report 0% progress
                            updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
                        }

                        return true;
                    }

                    // if we haven't downloaded yet...
                    if (frameOn == Frame.UpdateInfo)
                    {
                        ShowFrame(Frame.InstallUpdates);

                        autoUpdateStepProcessing = UpdateStep.DownloadUpdate;

                        // report 0% progress & download
                        updateHelper.SendProgress(0, UpdateStep.DownloadUpdate);
                        DownloadUpdate();

                        return true;
                    }

                    if (frameOn == Frame.InstallUpdates)
                    {
                        // if already downloading ...
                        if (IsInDownloadState())
                        {
                            // report 0%
                            updateHelper.SendProgress(0, UpdateStep.DownloadUpdate);
                            return true;
                        }

                        // if done extracting...
                        if (updtDetails != null)
                        {
                            // report extraction completed successfully
                            updateHelper.SendSuccess(UpdateStep.BeginExtraction);
                            return true;
                        }

                        if (currentlyExtracting)
                        {
                            // report extraction has begun
                            updateHelper.SendProgress(0, UpdateStep.BeginExtraction);
                            return true;
                        }
                    }


                    break;

                case UpdateStep.RestartInfo:
                case UpdateStep.Install:

                    if (frameOn == Frame.Checking)
                    {
                        // waiting to be told to check for updates...
                        if (downloader == null)
                        {
                            autoUpdateStepProcessing = UpdateStep.CheckForUpdate;

                            // report 0% and begin checking
                            updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
                            CheckForUpdate();
                        }
                        else // already checking ...
                        {
                            // report 0% progress
                            updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
                        }

                        return true;
                    }

                    // if we haven't downloaded yet...
                    if (frameOn == Frame.UpdateInfo)
                    {
                        ShowFrame(Frame.InstallUpdates);

                        autoUpdateStepProcessing = UpdateStep.DownloadUpdate;

                        // report 0% progress & download
                        updateHelper.SendProgress(0, UpdateStep.DownloadUpdate);
                        DownloadUpdate();

                        return true;
                    }

                    if (frameOn == Frame.InstallUpdates)
                    {
                        // if already downloading ...
                        if (IsInDownloadState())
                        {
                            // report 0%
                            updateHelper.SendProgress(0, UpdateStep.DownloadUpdate);
                            return true;
                        }

                        if (currentlyExtracting)
                        {
                            // report extraction has begun
                            updateHelper.SendProgress(0, UpdateStep.BeginExtraction);
                            return true;
                        }
                    }

                    break;
            }

            // no bad request found - continue processing as usual
            return false;
        }

        /// <summary>For filtering bad requests. Self updating is still in the "download" state.</summary>
        /// <returns>True if downloading, or downloading/extracting self update</returns>
        bool IsInDownloadState()
        {
            return update.CurrentlyUpdating == UpdateOn.DownloadingUpdate;
        }

        string CreateAutoUpdateTempFolder()
        {
            oldAUTempFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "wyUpdate AU\\cache\\" + update.GUID);

            // if we're upgrading from an pre v2.5.10 wyUpdate, then use old cache folder
            // otherwise create the new one
            return Directory.Exists(oldAUTempFolder) ? oldAUTempFolder : GetCacheFolder(update.GUID);
        }

        /// <summary>Gets or creates the cache folder for a GUID.</summary>
        /// <param name="guid">The GUID.</param>
        /// <returns>The directory to the cache folder</returns>
        static string GetCacheFolder(string guid)
        {
            string userprofile = SystemFolders.GetUserProfile();

            // if getting the userprofile folder fails, then try the app data folder
            if (string.IsNullOrEmpty(userprofile))
                userprofile = SystemFolders.GetCurrentUserAppData();

            if (string.IsNullOrEmpty(userprofile))
                throw new Exception("Failed to retrieve the user profile folder.");

            // C:\Users\USERNAME\wc
            string temp = Path.Combine(userprofile, "wc");

            // if the folder temp folder doesn't exist, create the folder with hidden attributes
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
                File.SetAttributes(temp, FileAttributes.System | FileAttributes.Hidden);
            }

            string fullGuidFolder = Path.Combine(temp, guid);

            // Workaround for the "pyramid of death" bug.
            // Note: This still doesn't address the root cause of the "pyramid of death"
            //       namely, why aren't the folders being cleaned up on the failure of wyUpdate?
            // Perhaps it's a crashing bug. Further investigation is needed. We need to look into
            // possible causes for cached files being left around. This problem was first spotted
            // when a user's app crashed when they were debugging. It was crashing over and over again
            if (Directory.Exists(fullGuidFolder))
            {
                string guidFile = Path.Combine(fullGuidFolder, guid);

                // if the GUID file doesn't already exist then the cache is busted.
                if (!File.Exists(guidFile))
                {
                    // delete every file and folder in the directory because it's an invalid cache
                    string[] obs = Directory.GetDirectories(fullGuidFolder);

                    // remove all directories
                    foreach (var dir in obs)
                        Directory.Delete(dir, true);

                    obs = Directory.GetFiles(fullGuidFolder);

                    // remove all the files
                    foreach (var file in obs)
                        File.Delete(file);

                    // create the blank GUID file
                    using (File.Create(guidFile)) { };
                }

                return fullGuidFolder;
            }

            // try to create the smallest possible folder name (using the GUID)
            string closestMatch = null;

            string[] dirs = Directory.GetDirectories(temp);

            // loop through the directories - stop at the first partial match this GUID
            for (int i = 0; i < dirs.Length; i++)
            {
                string name = Path.GetFileName(dirs[i]);

                if (string.IsNullOrEmpty(name) || guid.IndexOf(name) != 0)
                    continue;

                // see if the partial-matching folder contains an empty GUID file
                if (File.Exists(Path.Combine(dirs[i], guid)))
                    return dirs[i];

                closestMatch = name;
            }

            // the folder doesn't exist, so we'll create it
            string guidCacheFolder = Path.Combine(temp, guid.Substring(0, closestMatch == null ? 1 : closestMatch.Length + 1));

            Directory.CreateDirectory(guidCacheFolder);

            // create the blank GUID file
            using (File.Create(Path.Combine(guidCacheFolder, guid))) { };

            return guidCacheFolder;
        }

        void PrepareStepOn(UpdateStepOn step)
        {
            switch (step)
            {
                case UpdateStepOn.Checking:

                    ShowFrame(Frame.Checking);

                    break;

                case UpdateStepOn.UpdateAvailable:

                    ShowFrame(Frame.UpdateInfo);

                    break;

                case UpdateStepOn.UpdateDownloaded:

                    // set the update step pending (extracting)
                    update.CurrentlyUpdating = UpdateOn.Extracting;

                    needElevation = NeedElevationToUpdate();

                    // show frame InstallUpdate
                    ShowFrame(Frame.InstallUpdates);

                    // put a checkmark next to downloaded
                    panelDisplaying.UpdateItems[0].Status = UpdateItemStatus.Success;

                    break;

                case UpdateStepOn.UpdateReadyToInstall:

                    string updtDetailsFilename = Path.Combine(tempDirectory, "updtdetails.udt");

                    // Try to load the update details file

                    if (File.Exists(updtDetailsFilename))
                    {
                        updtDetails = UpdateDetails.Load(updtDetailsFilename);
                    }
                    else
                        throw new Exception("Update details file does not exist.");

                    // set the update step pending (closing processes & installing files, etc.)
                    update.CurrentlyUpdating = UpdateOn.ClosingProcesses;

                    needElevation = NeedElevationToUpdate();

                    // show frame InstallUpdate
                    ShowFrame(Frame.InstallUpdates);

                    // put a checkmark next to downloaded
                    panelDisplaying.UpdateItems[0].Status = UpdateItemStatus.Success;

                    // set the "Extracting" text
                    SetStepStatus(1, clientLang.Extract);

                    break;

                default:
                    throw new Exception("Can't restore from this automatic update state: " + step);
            }
        }
    }
}