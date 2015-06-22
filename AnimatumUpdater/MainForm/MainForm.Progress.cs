using Animatum.Updater.Common;
using Animatum.Updater.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatum.Updater
{
    public partial class MainForm
    {
        FilesInUseForm inUseForm;

        // update the label & progress bar when downloading/updating
        void ShowProgress(int percentDone, int unweightedPercent, string extraStatus, ProgressStatus status, Object payload)
        {
            if (IsDisposed)
                return;

            //update progress bar when between 0 and 100
            if (percentDone > -1 && percentDone < 101)
            {
                panelDisplaying.Progress = percentDone;

                // send the progress to the AutoUpdate control
                if (isAutoUpdateMode && autoUpdateStepProcessing != UpdateStep.Install)
                    updateHelper.SendProgress(unweightedPercent, autoUpdateStepProcessing);
            }

            // update bottom status
            if (!string.IsNullOrEmpty(extraStatus) && extraStatus != panelDisplaying.ProgressStatus)
                panelDisplaying.ProgressStatus = extraStatus;

            if (status == ProgressStatus.SharingViolation)
            {
                // show the dialog showing which file is in-use by another process
                if (inUseForm == null)
                {
                    inUseForm = new FilesInUseForm(clientLang, (string)payload);

                    inUseForm.ShowDialog(this);

                    // the inUseForm can be null when closed from another thread
                    // see the inUseForm.Close();, inUseForm = null; statements below
                    if (inUseForm != null

                        // cancel the update if the user can't/won't close the necessary files
                        && inUseForm.CancelUpdate)
                    {
                        CancelUpdate(false, true);
                    }
                }

                return;
            }

            if (inUseForm != null)
            {
                // get rid of the in-use form
                inUseForm.Close();
                inUseForm.Dispose();
                inUseForm = null;
            }

            if (installUpdate != null && (status == ProgressStatus.Success || status == ProgressStatus.Failure))
            {
                installUpdate.Rollback -= ChangeRollback;
                installUpdate.ProgressChanged -= ShowProgress;
            }

            if (status == ProgressStatus.Success)
            {
                if (frameOn == Frame.Checking)
                {
                    //set the serverfile location
                    if (!downloader.DownloadAll)
                        serverFileLoc = downloader.DownloadingTo;
                    try
                    {
                        ServerDownloadedSuccessfully();
                    }
                    catch (NoUpdatePathToNewestException)
                    {
                        //there is no update path to the newest version
                        ShowFrame(Frame.NoUpdatePathAvailable);
                        return;
                    }
                    catch (Exception e)
                    {
                        //error occured, show error screen
                        status = ProgressStatus.Failure;
                        payload = e;
                    }
                }
                else
                {
                    if (update.CurrentlyUpdating == UpdateOn.DownloadingUpdate)
                    {
                        // continue on to next step if finished with all files
                        if (downloader.CurrentURLIndex == updateFrom.FileSites.Count - 1)
                            StepCompleted();
                    }
                    else
                    {
                        StepCompleted();
                    }
                }
            }


            if (status == ProgressStatus.Failure)
            {
                // Show the error (rollback has already been done)
                if (isCancelled)
                {
                    Close();
                    return;
                }

                if (frameOn == Frame.Checking)
                {
                    error = clientLang.ServerError;
                    errorDetails = ((Exception)payload).Message + "\n\n" + ((Exception)payload).StackTrace;
                }
                else
                {
                    if (update.CurrentlyUpdating == UpdateOn.DownloadingUpdate)
                    {
                        //a download error occurred
                        error = clientLang.DownloadError;
                        errorDetails = ((Exception)payload).Message;

                        //TODO: retry downloads in AutoUpdateMode
                        /*
                        if (isAutoUpdateMode)
                        {
                            TimeSpan span = DateTime.Now - File.GetCreationTime(serverFileLoc);

                            if (span.Days > 0 || span.Hours > 2)
                            {
                                
                            }
                            //TODO: if we're in AutoUpdate mode, and the
                            //download failure was an update file, and the
                            //server file is older than 1 hour, redownload the server and see if there's a new download site

                            //TODO: ditto for self-update failures (clientSFLoc)
                        }*/
                    }
                    else // an update error occurred
                    {
                        // if the exception was PatchApplicationException, then
                        //see if a catch-all update exists (and the catch-all update isn't the one that failed)
                        if (payload is PatchApplicationException &&
                            updateFrom != ServerFile.VersionChoices[ServerFile.VersionChoices.Count - 1] &&
                            ServerFile.VersionChoices[ServerFile.VersionChoices.Count - 1].Version == ServerFile.NewVersion)
                        {
                            updateFrom = ServerFile.VersionChoices[ServerFile.VersionChoices.Count - 1];

                            error = null;

                            panelDisplaying.UpdateItems[1].Status = UpdateItemStatus.Nothing;

                            // we're no longer extracting
                            if (isAutoUpdateMode)
                                autoUpdateStepProcessing = UpdateStep.DownloadUpdate;

                            // download the catch-all update
                            DownloadUpdate();

                            // set for auto-updates
                            currentlyExtracting = false;

                            return;
                        }

                        error = clientLang.GeneralUpdateError;
                        errorDetails = ((Exception)payload).Message;
                    }
                }

                ShowFrame(Frame.Error);
            }
        }

        void UninstallProgress(int percentDone, int unweightedPercent, string extraStatus, ProgressStatus status, Object payload)
        {
            if (IsDisposed)
                return;

            // update progress bar
            if (percentDone > -1 && percentDone < 101)
                panelDisplaying.Progress = percentDone;

            //update bottom status
            if (!string.IsNullOrEmpty(extraStatus) && extraStatus != panelDisplaying.ProgressStatus)
                panelDisplaying.ProgressStatus = extraStatus;

            if (status == ProgressStatus.Success || status == ProgressStatus.Failure)
                installUpdate.ProgressChanged -= UninstallProgress;

            switch (status)
            {
                case ProgressStatus.None:
                    panelDisplaying.UpdateItems[0].Status = UpdateItemStatus.Success;
                    SetStepStatus(1, clientLang.UninstallRegistry);
                    break;
                case ProgressStatus.Success:
                    //just bail out.
                    Close();
                    break;
                case ProgressStatus.Failure:
                    if (isSilent)
                        Close();
                    else
                    {
                        // Show the error (rollback has ocurred)
                        error = clientLang.GeneralUpdateError;
                        errorDetails = ((Exception)payload).Message;
                        ShowFrame(Frame.Error);
                    }
                    break;
            }
        }

        void CheckProcess(int percentDone, int unweightedPercent, string extraStatus, ProgressStatus status, Object payload)
        {
            if (IsDisposed)
                return;

            // update bottom status
            if (!string.IsNullOrEmpty(extraStatus) && extraStatus != panelDisplaying.ProgressStatus)
                panelDisplaying.ProgressStatus = extraStatus;

            if (status == ProgressStatus.Success || status == ProgressStatus.Failure)
            {
                installUpdate.Rollback -= ChangeRollback;
                installUpdate.ProgressChanged -= CheckProcess;
            }

            if (status == ProgressStatus.Success)
            {
                List<FileInfo> files = null;
                List<Process> rProcesses = null;

                var objects = payload as object[];
                if (objects != null)
                {
                    files = (List<FileInfo>)(objects)[0];
                    rProcesses = (List<Process>)(objects)[1];
                }

                if (rProcesses != null) //if there are some processes need closing
                {
                    // remove processes that have exited since last checked
                    for (int i = 0; i < rProcesses.Count; i++)
                    {
                        try
                        {
                            if (rProcesses[i].HasExited)
                                rProcesses.RemoveAt(i);
                        }
                        catch { }
                    }

                    // only continue if processes are still running
                    if (rProcesses.Count > 0)
                    {
                        // show myself, make topmost
                        Show();
                        TopMost = true;
                        TopMost = false;

                        // start the close processes form
                        using (ProcessesForm proc = new ProcessesForm(files, rProcesses, clientLang))
                        {
                            if (proc.ShowDialog() == DialogResult.Cancel)
                            {
                                // cancel the update process
                                CancelUpdate(true, false);
                                return;
                            }
                        }
                    }
                }

                // processes closed, continue on
                update.CurrentlyUpdating += 1;
                InstallUpdates(update.CurrentlyUpdating);
            }

            if (status == ProgressStatus.Failure)
            {
                if (isCancelled)
                {
                    Close();
                    return;
                }

                error = clientLang.GeneralUpdateError;
                errorDetails = ((Exception)payload).Message;

                ShowFrame(Frame.Error);
            }
        }

        void ChangeRollback(bool rbRegistry)
        {
            if (IsDisposed)
                return;

            DisableCancel();

            // set the error icon to current progress item
            if (rbRegistry)
            {
                // error updating the registry
                panelDisplaying.UpdateItems[2].Status = UpdateItemStatus.Error;

                SetStepStatus(3, clientLang.RollingBackRegistry);
            }
            else if (panelDisplaying.UpdateItems[2].Status != UpdateItemStatus.Error)
            {
                // error updating the files
                panelDisplaying.UpdateItems[1].Status = UpdateItemStatus.Error;

                SetStepStatus(2, clientLang.RollingBackFiles);
            }
            else
            {
                SetStepStatus(3, clientLang.RollingBackFiles);
            }
        }
    }
}
