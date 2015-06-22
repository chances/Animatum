using Animatum.Updater.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatum.Updater
{
    public enum UpdateOn
    {
        DownloadingUpdate = 4, Extracting = 5, ClosingProcesses = 6, 
        PreExecute = 7, BackUpInstalling = 8, ModifyReg = 9, 
        DeletingTemp = 10, Uninstalling = 11
    }

    public partial class MainForm
    {
        public string InstalledVersion;

        public UpdateOn CurrentlyUpdating = UpdateOn.DownloadingUpdate;

        void ShowFrame(Frame frameNum)
        {
            frameOn = frameNum;

            switch (frameNum)
            {
                case Frame.Checking: //Update checking screen
                    panelDisplaying.ChangePanel(FrameType.Update,
                        clientLang.Checking.Content, String.Empty);

                    btnNext.Enabled = false;

                    if (!isAutoUpdateMode)
                        CheckForUpdate();

                    break;
                case Frame.UpdateInfo: //Update Info Screen
                    panelDisplaying.ChangePanel(FrameType.TextInfo,
                        clientLang.UpdateInfo.Content, clientLang.UpdateBottom);

                    // check if elevation is needed
                    needElevation = NeedElevationToUpdate();

                    btnNext.Enabled = true;
                    btnNext.Text = (installing ? clientLang.InstallButton : clientLang.UpdateButton);

                    if (QuickCheck)
                    {
                        // return 2 if we're just checking
                        if (QuickCheckJustCheck)
                        {
                            if (String.IsNullOrEmpty(OutputInfo))
                            {
                                // output the changelog
                                //Console.WriteLine(ServerFile.NewVersion);
                                Console.WriteLine(panelDisplaying.GetChanges(false));
                            }
                            else if (OutputInfo != null)
                            {
                                try
                                {
                                    using (StreamWriter outfile = new StreamWriter(OutputInfo))
                                    {
                                        outfile.WriteLine(ServerFile.NewVersion);
                                        outfile.WriteLine(panelDisplaying.GetChanges(false));
                                    }
                                }
                                catch { }
                            }

                            ReturnCode = 2;
                            Close();
                            return;
                        }

                        ShowInTaskbar = true;
                        WindowState = FormWindowState.Normal;

                        TopMost = true;
                        TopMost = false;

                        QuickCheck = false;
                    }
                    else if (isAutoUpdateMode)
                    {
                        updateHelper.SendSuccess(ServerFile.NewVersion, panelDisplaying.GetChanges(true), true);
                    }

                    break;
                case Frame.InstallUpdates: //Download and Install Updates
                    panelDisplaying.ShowChecklist = true;

                    panelDisplaying.ChangePanel(FrameType.Update,
                        clientLang.DownInstall.Content, String.Empty);

                    //show status for the downloading update
                    SetStepStatus(0, clientLang.Download);

                    if (!isAutoUpdateMode)
                        DownloadUpdate();

                    btnNext.Enabled = false;

                    break;
                case Frame.UpdatedSuccessfully: //Display Congrats Window
                    panelDisplaying.ChangePanel(FrameType.WelcomeFinish,
                        clientLang.SuccessUpdate.Content, clientLang.FinishBottom);

                    btnNext.Enabled = true;
                    btnCancel.Visible = false;
                    btnNext.Text = clientLang.FinishButton;

                    break;
                case Frame.AlreadyUpToDate: //Your Product is already up to date screen
                    panelDisplaying.ChangePanel(FrameType.WelcomeFinish,
                        clientLang.AlreadyLatest.Content,
                        clientLang.FinishBottom);

                    btnNext.Enabled = true;
                    btnCancel.Visible = false;
                    btnNext.Text = clientLang.FinishButton;

                    break;
                case Frame.NoUpdatePathAvailable: //No update to the latest version is available
                    if (!string.IsNullOrEmpty(ServerFile.NoUpdateToLatestLinkText))
                        panelDisplaying.SetNoUpdateAvailableLink(ServerFile.NoUpdateToLatestLinkText, ServerFile.NoUpdateToLatestLinkURL);

                    panelDisplaying.ChangePanel(FrameType.WelcomeFinish,
                        clientLang.NoUpdateToLatest.Content,
                        clientLang.FinishBottom);

                    btnNext.Enabled = true;
                    btnCancel.Visible = false;
                    btnNext.Text = clientLang.FinishButton;

                    break;
                case Frame.Uninstall: //Uninstall screen
                    panelDisplaying.ShowChecklist = true;

                    panelDisplaying.ChangePanel(FrameType.Update,
                        clientLang.Uninstall.Content, String.Empty);


                    //Show uninstalling status
                    SetStepStatus(0, clientLang.UninstallFiles);

                    btnNext.Enabled = false;

                    InstallUpdates(UpdateOn.Uninstalling);

                    break;
                case Frame.Error: //Display error screen

                    //TODO: make the return codes error specific
                    ReturnCode = 1;

                    // show details button to hide all the complex crap from users
                    panelDisplaying.ErrorDetails = errorDetails;
                    panelDisplaying.SetUpErrorDetails(clientLang.ShowDetails);

                    panelDisplaying.ChangePanel(FrameType.WelcomeFinish,
                        error,
                        clientLang.FinishBottom);

                    btnNext.Enabled = true;
                    btnCancel.Visible = false;
                    btnNext.Text = clientLang.FinishButton;

                    // show wyUpdate if 
                    if (QuickCheck && !QuickCheckNoErr)
                    {
                        ShowInTaskbar = true;
                        WindowState = FormWindowState.Normal;

                        TopMost = true;
                        TopMost = false;

                        QuickCheck = false;
                    }

                    break;
            }

            // handle all success / error cases
            if (FrameIs.ErrorFinish(frameNum))
            {
                // allow the user to forcefuly exit
                BlockLogOff(false);

                EnableCancel();

                // allow the user to exit by pressing ESC
                CancelButton = btnNext;

                // set the error return code (1) or success (0)
                ReturnCode = frameNum == Frame.Error ? 1 : 0;

                if (QuickCheck)
                {
                    if (frameNum == Frame.Error && !QuickCheckNoErr)
                    {
                        Visible = true;
                        TopMost = true;
                        TopMost = false;
                    }
                    else
                    {
                        if (frameNum == Frame.Error)
                        {
                            if (OutputInfo == string.Empty)
                            {
                                // output the error
                                Console.WriteLine(error + "\r\n");
                                Console.WriteLine(errorDetails);
                            }
                            else if (OutputInfo != null)
                            {
                                try
                                {
                                    using (StreamWriter outfile = new StreamWriter(OutputInfo))
                                    {
                                        outfile.WriteLine(error);
                                        outfile.WriteLine(errorDetails);
                                    }
                                }
                                catch { }
                            }

                            // If we're starting a process on error, then start it
                            if (StartOnErr != null)
                            {
                                try
                                {
                                    LimitedProcess.Start(StartOnErr, StartOnErrArgs);
                                }
                                catch { }
                            }
                        }

                        WindowState = FormWindowState.Minimized;
                        ShowInTaskbar = false;
                        Visible = true;
                        Close();
                        return;
                    }
                }
                else if (isAutoUpdateMode)
                {
                    // if it's reasonable to expect a client to be waiting for an error
                    // that is, if we haven't already started the update process
                    // then send all waiting processes the error message
                    if (update.CurrentlyUpdating < UpdateOn.ClosingProcesses)
                    {
                        // wait for any clients to connect
                        if (!updateHelper.RunningServer)
                            StartQuickAndDirtyAutoUpdateMode();

                        // send the error to any running "client" processes
                        updateHelper.SendFailed(error, errorDetails, autoUpdateStepProcessing);
                    }

                    if (frameNum == Frame.UpdatedSuccessfully || frameNum == Frame.Error)
                    {
                        // save whether an update succeeded or failed
                        AutoUpdaterInfo auInfo;

                        if (frameNum == Frame.Error)
                        {
                            auInfo = new AutoUpdaterInfo(updateHelper.AutoUpdateID, oldAUTempFolder)
                            {
                                AutoUpdaterStatus = AutoUpdaterStatus.UpdateFailed,
                                ErrorTitle = error,
                                ErrorMessage = errorDetails
                            };
                        }
                        else
                        {
                            auInfo = new AutoUpdaterInfo(updateHelper.AutoUpdateID, oldAUTempFolder)
                            {
                                AutoUpdaterStatus = AutoUpdaterStatus.UpdateSucceeded,
                                UpdateVersion = ServerFile.NewVersion,
                                ChangesInLatestVersion = panelDisplaying.GetChanges(true),
                                ChangesIsRTF = true
                            };
                        }

                        auInfo.Save();

                        try
                        {
                            // start the updated program as a limited user
                            LimitedProcess.Start(updateHelper.FileOrServiceToExecuteAfterUpdate,
                                                 updateHelper.ExecutionArguments);
                        }
                        catch { }
                        // if the app fails to start, no big deal
                    }

                    // we're no longer in autoupdate mode - cleanup temp files on close
                    isAutoUpdateMode = false;

                    Close();
                    return;
                }
                else if ((update.CloseOnSuccess && frameNum == Frame.UpdatedSuccessfully)
                        || (StartOnErr != null && frameNum == Frame.Error))
                {
                    // If we're in normal mode but the user has specified they want "CloseOnSuccess", then do it.

                    if (log != null)
                    {
                        if (frameNum == Frame.UpdatedSuccessfully)
                            log.Write("Updated successfully.");
                        else
                            log.Write(error + " - " + errorDetails);
                    }

                    // If we're starting a process on error, then start it
                    if (StartOnErr != null && frameNum == Frame.Error)
                    {
                        // if the "failure" process fails to start then show
                        // the error screen.
                        try
                        {
                            LimitedProcess.Start(StartOnErr, StartOnErrArgs);
                        }
                        catch
                        {
                            return;
                        }
                    }

                    Close();
                    return;
                }
            }

            try
            {
                // so the user doesn't accidentally cancel update.
                btnNext.Focus();
            }
            catch { }

            // if silent & if on one of the user interaction screens, then click next
            if (isSilent && (FrameIs.Interaction(frameOn)))
            {
                btnNext_Click(null, EventArgs.Empty);
                return;
            }
        }

        void InstallUpdates(UpdateOn CurrentlyUpdating)
        {
            switch (CurrentlyUpdating)
            {
                case UpdateOn.Extracting:

                    // set for auto-updates
                    currentlyExtracting = true;

                    SetStepStatus(1, clientLang.Extract);

                    installUpdate = new InstallUpdate
                    {
                        Filename = Path.Combine(tempDirectory, updateFilename),
                        OutputDirectory = tempDirectory,
                        TempDirectory = tempDirectory,
                        ProgramDirectory = baseDirectory,

                        // use the password passed in by commandline (take precedence)
                        // or use the password embedded in the client.wyc file.
                        //ExtractPassword = PasswordUpdateCmd ?? update.UpdatePassword
                    };

                    installUpdate.ProgressChanged += ShowProgress;
                    installUpdate.RunUnzipProcess();
                    break;
                case UpdateOn.ClosingProcesses:
                    SetStepStatus(1, clientLang.Processes);

                    installUpdate = new InstallUpdate
                    {
                        UpdtDetails = updtDetails,
                        TempDirectory = tempDirectory,
                        ProgramDirectory = baseDirectory,

                        // skip ui reporting when updating from a service
                        //SkipUIReporting = UpdatingFromService || updateHelper.IsAService,
                        //SkipStartService = updateHelper.IsAService ? updateHelper.FileOrServiceToExecuteAfterUpdate : null
                    };

                    installUpdate.Rollback += ChangeRollback;
                    installUpdate.ProgressChanged += CheckProcess;
                    installUpdate.RunProcessesCheck();
                    break;
                case UpdateOn.PreExecute:

                    // try to stop the user from forcefuly exiting
                    BlockLogOff(true);

                    SetStepStatus(1, clientLang.PreExec);

                    installUpdate = new InstallUpdate
                    {
                        UpdtDetails = updtDetails,
                        TempDirectory = tempDirectory,
                        ProgramDirectory = baseDirectory,
                        IsAdmin = IsAdmin,
                        MainWindowHandle = Handle
                    };

                    installUpdate.Rollback += ChangeRollback;
                    installUpdate.ProgressChanged += ShowProgress;
                    installUpdate.RunPreExecute();
                    break;
                case UpdateOn.BackUpInstalling:
                    SetStepStatus(1, clientLang.Files);

                    installUpdate = new InstallUpdate
                    {
                        UpdtDetails = updtDetails,
                        TempDirectory = tempDirectory,
                        ProgramDirectory = baseDirectory,

                        // skip ui reporting when updating from a service
                        //SkipUIReporting = UpdatingFromService || updateHelper.IsAService
                    };

                    installUpdate.Rollback += ChangeRollback;
                    installUpdate.ProgressChanged += ShowProgress;
                    installUpdate.RunUpdateFiles();
                    break;
                case UpdateOn.ModifyReg:
                    SetStepStatus(2, clientLang.Registry);

                    installUpdate = new InstallUpdate
                    {
                        UpdtDetails = updtDetails,
                        TempDirectory = tempDirectory,
                        ProgramDirectory = baseDirectory
                    };

                    installUpdate.Rollback += ChangeRollback;
                    installUpdate.ProgressChanged += ShowProgress;
                    installUpdate.RunUpdateRegistry();
                    break;
                case UpdateOn.DeletingTemp:
                    SetStepStatus(3, clientLang.TempFiles);

                    installUpdate = new InstallUpdate
                    {
                        TempDirectory = tempDirectory
                    };

                    installUpdate.ProgressChanged += ShowProgress;
                    installUpdate.RunDeleteTemporary();
                    break;
                case UpdateOn.Uninstalling:
                    // need to pass: client data file loc, delegate, this
                    installUpdate = new InstallUpdate
                    {
                        //Filename = clientFileLoc
                    };

                    installUpdate.ProgressChanged += UninstallProgress;
                    installUpdate.RunUninstall();
                    break;
            }
        }

        void SetStepStatus(int stepNum, string stepText)
        {
            panelDisplaying.ProgressStatus = String.Empty;
            panelDisplaying.UpdateItems[stepNum].Status = UpdateItemStatus.Working;
            panelDisplaying.UpdateItems[stepNum].Text = stepText;
        }

        //update step completed, continue to next
        void StepCompleted()
        {
            if (update.CurrentlyUpdating == UpdateOn.DeletingTemp)
            {
                //successfully deleted temporary files
                panelDisplaying.UpdateItems[3].Status = UpdateItemStatus.Success;

                //Successfully Updated
                ShowFrame(Frame.UpdatedSuccessfully);

                btnNext.Enabled = true;
            }
            else
            {
                //show check mark to completed
                switch (update.CurrentlyUpdating)
                {
                    case UpdateOn.DownloadingUpdate: //done downloading
                        panelDisplaying.UpdateItems[0].Status = UpdateItemStatus.Success;
                        break;
                    case UpdateOn.Extracting:
                        //done extracting, load update details
                        updtDetails = installUpdate.UpdtDetails;
                        break;
                    case UpdateOn.BackUpInstalling: //done backing up & installing files
                        panelDisplaying.UpdateItems[1].Status = UpdateItemStatus.Success;
                        break;
                    case UpdateOn.ModifyReg: //done updateing registry
                        panelDisplaying.UpdateItems[2].Text = clientLang.Registry;
                        panelDisplaying.UpdateItems[2].Status = UpdateItemStatus.Success;
                        break;
                }

                // if we're in wait mode, then don't continue to the next step
                if (isAutoUpdateMode && (update.CurrentlyUpdating == UpdateOn.DownloadingUpdate || update.CurrentlyUpdating == UpdateOn.Extracting))
                {
                    // save the autoupdate state details
                    //SaveAutoUpdateData(update.CurrentlyUpdating == UpdateOn.DownloadingUpdate
                    //                        ? UpdateStepOn.UpdateDownloaded
                    //                        : UpdateStepOn.UpdateReadyToInstall);

                    updateHelper.SendSuccess(autoUpdateStepProcessing);

                    //Go to the next step
                    update.CurrentlyUpdating += 1;

                    // set for auto-updates
                    currentlyExtracting = false;

                    return;
                }

                //Go to the next step
                update.CurrentlyUpdating += 1;

                //if there isn't an updateDetails file
                if (updtDetails == null &&
                    (update.CurrentlyUpdating == UpdateOn.PreExecute ||
                     update.CurrentlyUpdating == UpdateOn.ModifyReg))
                {
                    update.CurrentlyUpdating += 1;

                    if (update.CurrentlyUpdating == UpdateOn.ModifyReg)
                        update.CurrentlyUpdating += 1;
                }

                InstallUpdates(update.CurrentlyUpdating);
            }
        }
    }
}
