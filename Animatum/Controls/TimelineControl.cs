using System;
using System.Windows.Forms;
using Animatum.SceneGraph;
using System.Drawing;
using System.Threading;
using System.IO;
using mshtml;
using System.ComponentModel;
using CefSharp.WinForms;
using CefSharp;

namespace Animatum.Controls
{
    public partial class TimelineControl : UserControl
    {
        private readonly ChromiumWebBrowser webBrowser;

        private const float FONT_SIZE = 8.25f;
        private string path = "app://timeline/index.html";
        private TimelineScriptingObject scriptObj;
        private Model model;
        private bool debugMode;

        /// <summary>
        /// Occurs when the timeline has loaded and is ready.
        /// </summary>
        [Browsable(false)]
        public event EventHandler Ready;
        /// <summary>
        /// Occurs when the user performed a keyboard shortcut.
        /// </summary>
        [Category("Key")]
        [Description("Occurs when the user performed a keyboard shortcut.")]
        public event ChromiumBrowserHandlers.KeyboardHandler.KeyCommandHandler KeyCommand;
        /// <summary>
        /// Occurs when the Model has been updated by the user in the timeline.
        /// </summary>
        [Browsable(false)]
        public event EventHandler ModelUpdated;
        /// <summary>
        /// Occurs when the user begins playback from the timeline.
        /// </summary>
        [Browsable(false)]
        public event EventHandler BeginPlayback;
        /// <summary>
        /// Occurs when the user pauses playback from the timeline.
        /// </summary>
        [Browsable(false)]
        public event EventHandler PausePlayback;
        /// <summary>
        /// Occurs when the user stops playback from the timeline.
        /// </summary>
        [Browsable(false)]
        public event EventHandler StopPlayback;

        public TimelineControl()
        {
            InitializeComponent();

            if (DesignMode)
            {
                this.BackColor = Color.Black;
            }
            else
            {

                InitializeCef();

                webBrowser = new ChromiumWebBrowser(path)
                {
                    Dock = DockStyle.Fill
                };
                this.Controls.Add(webBrowser);
                webBrowser.SendToBack();

                webBrowser.MenuHandler = new ChromiumBrowserHandlers.MenuHandler();
                var keyboardHandler = new ChromiumBrowserHandlers.KeyboardHandler();
                keyboardHandler.KeyCommand += webBrowser_KeyCommand;
                webBrowser.KeyboardHandler = keyboardHandler;

                webBrowser.ConsoleMessage += webBrowser_ConsoleMessage;
                webBrowser.LoadError += webBrowser_LoadError;

                webBrowser.BrowserSettings.JavaScriptAccessClipboardDisabled = false;
                webBrowser.BrowserSettings.JavaScriptCloseWindowsDisabled = true;
                webBrowser.BrowserSettings.JavaScriptOpenWindowsDisabled = true;

                scriptObj = new TimelineScriptingObject(null);
                scriptObj.DOMReady += scriptObj_DOMReady;
                scriptObj.ModelUpdated += new EventHandler(scriptObj_ModelUpdated);
                scriptObj.BeginPlayback += new EventHandler(scriptObj_BeginPlayback);
                scriptObj.PausePlayback += new EventHandler(scriptObj_PausePlayback);
                scriptObj.StopPlayback += new EventHandler(scriptObj_StopPlayback);

                webBrowser.RegisterJsObject("external", scriptObj);
            }
        }

        void scriptObj_DOMReady(object sender, EventArgs e)
        {
            setBrowserStyles();

            if (Ready != null) Ready(this, new EventArgs());
        }

        public Model Model
        {
            get { return model; }
            set
            {
                bool makeHandler = (model == null);
                model = value;
                if (makeHandler && model != null)
                {
                    model.CurrentTimeChanged += new EventHandler(model_CurrentTimeChanged);
                    model.AnimationEnded += new EventHandler(model_AnimationEnded);
                }
                scriptObj.model = model;
                execScript("onModelUpdated();");
            }
        }

        public bool DebugMode
        {
            get { return debugMode; }
            set
            {
                debugMode = value;
                this.reloadButton.Visible = debugMode;

                if (DebugMode)
                    webBrowser.ShowDevTools();
            }
        }

        private void InitializeCef()
        {
            CefSettings settings = new CefSettings();
            settings.Locale = "en-US";

            settings.RegisterScheme(new CefCustomScheme()
            {
                IsDisplayIsolated = true,
                IsStandard = true,
                SchemeName = AppSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new AppSchemeHandlerFactory()
            });

            Cef.Initialize(settings, true, true);
        }

        private void OnModelUpdated()
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                if (ModelUpdated != null)
                    ModelUpdated(this, new EventArgs());
            });
        }

        void model_CurrentTimeChanged(object sender, EventArgs e)
        {
            execScript("onCurrentTimeChanged();");
        }

        void model_AnimationEnded(object sender, EventArgs e)
        {
            execScript("onAnimationEnded();");
        }

        void scriptObj_ModelUpdated(object sender, EventArgs e)
        {
            OnModelUpdated();
        }

        void scriptObj_BeginPlayback(object sender, EventArgs e)
        {
            if (BeginPlayback != null)
                BeginPlayback(this, new EventArgs());
        }

        void scriptObj_PausePlayback(object sender, EventArgs e)
        {
            if (PausePlayback != null)
                PausePlayback(this, new EventArgs());
        }

        void scriptObj_StopPlayback(object sender, EventArgs e)
        {
            if (StopPlayback != null)
                StopPlayback(this, new EventArgs());
        }

        private void setBrowserStyles()
        {
            string style = "body {";
            // Background
            style += "background-color:" + ColorTranslator.ToHtml(this.BackColor) + ";";
            // Font
            style += "font-family: \"" + this.Font.FontFamily.Name + "\";";
            style += "font-size:" + FONT_SIZE + "pt;";
            style += "}";

            // Set style
            execScript("var node = document.createElement(\"style\"); node.innerHTML = \"" +
                style + "\"; document.head.appendChild(node);");
        }

        private void execScript(string script)
        {
            if (scriptObj != null && scriptObj.isReady)
            {
                webBrowser.ExecuteScriptAsync(script);
            }
        }

        private void TimelineControl_Load(object sender, EventArgs e)
        {
            if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
            {
                webBrowser.Load(path);
            }
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
            {
                webBrowser.Load(path);
            }
        }

        private bool webBrowser_KeyCommand(object sender, int key)
        {
            bool response = false;

            this.InvokeOnUiThreadIfRequired(() =>
            {
                if (KeyCommand != null)
                    response = KeyCommand(sender, key);
            });

            return response;
        }

        void webBrowser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            if (debugMode)
            {
                // Add to a log, maybe?
            }
        }

        void webBrowser_LoadError(object sender, LoadErrorEventArgs e)
        {
            throw new Exception("Error " + e.ErrorCode.ToString() + ": " +
                e.FailedUrl + "\n" + e.ErrorText);
        }
    }
}