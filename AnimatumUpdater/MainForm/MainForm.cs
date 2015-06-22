using Animatum.Updater.Common;
using Animatum.Updater.Controls;
using Animatum.Updater.Downloader;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Animatum.Updater
{
    public partial class MainForm : Form
    {
        private PanelDisplay panelDisplaying;

        readonly UpdateSettings update = new UpdateSettings();
        readonly ClientLanguage clientLang = new ClientLanguage();

        Frame frameOn = Frame.Checking;
        bool isCancelled;

        // the first step the updater should take
        UpdateStepOn startStep = UpdateStepOn.Nothing;

        /// <summary>Does the client need elevation?</summary>
        bool needElevation;
        bool isSilent = false;
        bool isAutoUpdateMode = false;
        // base directory: same path as the executable, unless specified
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //the extract directory
        string tempDirectory;

        string animatumLocation;

        string updateFilename;
        string serverFileLoc;
        ServerFile ServerFile;
        VersionChoice updateFrom;

        UpdateDetails updtDetails;

        FileDownloader downloader;
        InstallUpdate installUpdate;

        // handle hidden form
        bool _isApplicationRun = true;

        // start hidden, close if no update, show if update
        bool QuickCheck;
        bool QuickCheckNoErr;
        bool QuickCheckJustCheck;
        bool SkipUpdateInfo;
        string OutputInfo;

        string StartOnErr;
        string StartOnErrArgs;

        string error;
        string errorDetails;

        Logger log;

        public int ReturnCode { get; private set; }

        public bool IsAdmin { get; private set; }

        public MainForm(string[] args)
        {
            IsAdmin = Win32.IsUserAnAdmin();

            InitializeComponent();

            panelDisplaying = new PanelDisplay(mainPanel.Width, mainPanel.Height);
            panelDisplaying.Dock = DockStyle.Fill;
            panelDisplaying.BackColor = this.BackColor;

            mainPanel.Controls.Add(panelDisplaying);
            mainPanel.Width = ClientRectangle.Width;

            update.ServerFileSites.Add("http://web.cecs.pdx.edu/~chances/animatum/animatum.update");

            animatumLocation = Path.Combine(baseDirectory, "Animatum.exe");
            if (File.Exists(animatumLocation))
                update.InstalledVersion = FileVersionInfo.GetVersionInfo(animatumLocation).FileVersion;
            else
                update.InstalledVersion = "0.0.0.0";

            panelDisplaying.UpdateItems[1].Text = "Installing dependencies";
            panelDisplaying.UpdateItems[2].Text = "Backing up and updating files";

            //process commandline argument
            Arguments commands = new Arguments(args);
            ProcessArguments(commands);

            //sets up Next & Cancel buttons
            SetButtonText();

            if (isAutoUpdateMode)
            {

            }
            
            startStep = UpdateStepOn.Checking;
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);

            if (!_isApplicationRun)
                return;

            _isApplicationRun = false;

            if (isAutoUpdateMode)
            {
                /* SetupAutoupdateMode must happen after the handle is created
                     * (aka. in OnHandleCreated, or after base.SetVisibleCore() is called)
                     * because Control.Invoke() used in UpdateHelper
                     * requires the handle to be created.
                     *
                     * This solves the problem where the AutomaticUpdater control sends a message,
                     * it thinks the message was recieved successfully because there
                     * wasn't an error on the pipe stream, however in reality it never gets past
                     * the try-catch block in 'pipeServer_MessageReceived'. The exception is gobbled up
                     * and there's a stalemate: wyUpdate is waiting for its first message, AutomaticUpdater
                     * is waiting for a progress report.
                     */
                SetupAutoupdateMode();
            }


            // run the OnLoad code

            if (startStep != UpdateStepOn.Nothing)
            {
                // either begin checking or load the step from the autoupdate file
                try
                {
                    PrepareStepOn(startStep);

                    // selfupdate & post-selfupdate installation
                    if (beginAutoUpdateInstallation)
                        UpdateHelper_RequestReceived(this, UpdateAction.UpdateStep, UpdateStep.Install);
                }
                catch (Exception ex)
                {
                    if (startStep != UpdateStepOn.Checking)
                        startStep = UpdateStepOn.Checking;
                    else
                    {
                        // show the error screen
                        error = "Automatic update state failed to load.";
                        errorDetails = ex.Message;

                        ShowFrame(Frame.Error);
                        return;
                    }

                    try
                    {
                        PrepareStepOn(startStep);
                    }
                    catch (Exception ex2)
                    {
                        // show the error screen
                        error = "Automatic update state failed to load.";
                        errorDetails = ex2.Message;

                        ShowFrame(Frame.Error);
                    }
                }
            }
        }

        void ProcessArguments(Arguments commands)
        {
            // automatic update mode
            if (commands["autoupdate"] != null)
            {
                // the actual pipe will be created when OnHandleCreated is called
                isAutoUpdateMode = true;

                // check if this instance is the "new self"
                if (commands["ns"] != null)
                    IsNewSelf = true;
            }
            else // standalone updater mode
            {
                if (commands["quickcheck"] != null)
                {
                    WindowState = FormWindowState.Minimized;
                    ShowInTaskbar = false;

                    QuickCheck = true;

                    if (commands["noerr"] != null)
                        QuickCheckNoErr = true;

                    if (commands["justcheck"] != null)
                        QuickCheckJustCheck = true;

                    // for outputting errors & update information to
                    // STDOUT or to a file
                    if (QuickCheckNoErr || QuickCheckJustCheck)
                        OutputInfo = commands["outputinfo"];
                }

                if (commands["skipinfo"] != null)
                    SkipUpdateInfo = true;

                StartOnErr = commands["startonerr"];
                StartOnErrArgs = commands["startonerra"];
            }

            //set basedirectory as the location of the executable
            baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (commands["basedir"] != null && Directory.Exists(commands["basedir"]))
            {
                //if the specified directory exists, then set as directory
                // also trim the trailing space
                baseDirectory = commands["basedir"].TrimEnd();
            }

            // create "random" temp dir.
            tempDirectory = Path.Combine(Path.GetTempPath(), @"w" + DateTime.Now.ToString("sff"));
            Directory.CreateDirectory(tempDirectory);

            // only allow silent uninstalls 
            if (commands["s"] != null)
            {
                isSilent = true;

                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // only warn if after the welcome page and not self updating/elevating
            if (needElevation
                || isSilent
                || isAutoUpdateMode
                || isCancelled
                || panelDisplaying.TypeofFrame == FrameType.WelcomeFinish
                || panelDisplaying.TypeofFrame == FrameType.TextInfo)
            {
                //close the form
                e.Cancel = false;
            }
            else //currently updating
            {
                // stop closing
                e.Cancel = true;

                // prompt the user if they really want to cancel
                CancelUpdate();
            }

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            RemoveTempDirectory();

            if (File.Exists(animatumLocation) && installing
                && !QuickCheck && !isAutoUpdateMode && !isCancelled
                && String.IsNullOrEmpty(error) && String.IsNullOrEmpty(errorDetails))
                Process.Start(animatumLocation);

            if (isCancelled)
                ReturnCode = 3;

            base.OnClosed(e);
        }

        /// <summary>
        /// Remove the temporary directory if it exists.
        /// </summary>
        void RemoveTempDirectory()
        {
            if (!Directory.Exists(tempDirectory))
                return;

            try
            {
                Directory.Delete(tempDirectory, true);
            }
            catch { }
        }
    }
}
