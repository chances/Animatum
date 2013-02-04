using System;
using System.Windows.Forms;
using Animatum.SceneGraph;
using System.Drawing;
using System.Threading;
using System.IO;
using mshtml;
using System.ComponentModel;

namespace Animatum.Controls
{
    public partial class TimelineControl : UserControl
    {
        private const float FONT_SIZE = 8.25f;
        private string path = Application.StartupPath + "\\Timeline\\index.html";
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
        public event HotkeyWebBrowser.KeyCommandHandler KeyCommand;
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

            scriptObj = new TimelineScriptingObject(null);
            scriptObj.ModelUpdated += new EventHandler(scriptObj_ModelUpdated);
            scriptObj.BeginPlayback += new EventHandler(scriptObj_BeginPlayback);
            scriptObj.PausePlayback += new EventHandler(scriptObj_PausePlayback);
            scriptObj.StopPlayback += new EventHandler(scriptObj_StopPlayback);
            webBrowser.ObjectForScripting = scriptObj;
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
                if (webBrowser.Document != null)
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
                this.webBrowser.ScriptErrorsSuppressed = !debugMode;
            }
        }

        private void OnReady()
        {
            if (Ready != null) Ready(this, new EventArgs());
        }

        private void OnModelUpdated()
        {
            if (ModelUpdated != null)
                ModelUpdated(this, new EventArgs());
        }

        void model_CurrentTimeChanged(object sender, EventArgs e)
        {
            if (webBrowser.Document != null)
                execScript("onCurrentTimeChanged();");
        }

        void model_AnimationEnded(object sender, EventArgs e)
        {
            if (webBrowser.Document != null)
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

        private void setBrowserStyle()
        {
            //Create a style element
            HtmlElement styleElem = webBrowser.Document.CreateElement("style");
            string style = "body {";
            //Background
            style += "background-color:" + ColorTranslator.ToHtml(this.BackColor) + ";";
            //Font
            style += "font-family: \"" + this.Font.FontFamily.Name + "\";";
            style += "font-size:" + FONT_SIZE + "pt;";
            style += "}";
            //Set style
            styleElem.InnerText = style;
            webBrowser.Document.GetElementsByTagName("head")[0].AppendChild(styleElem);
        }

        private void execScript(string script)
        {
            HtmlElement scriptElem = webBrowser.Document.CreateElement("script");
            scriptElem.SetAttribute("type", "text/javascript");
            IHTMLScriptElement elem = (IHTMLScriptElement)scriptElem.DomElement;
            elem.text = script;
            webBrowser.Document.GetElementsByTagName("head")[0].AppendChild(scriptElem);
        }

        private void TimelineControl_Load(object sender, EventArgs e)
        {
            if (File.Exists(path))
                webBrowser.Navigate(path);
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            setBrowserStyle();
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(path))
                webBrowser.Navigate(path);
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            OnReady();
        }

        private bool webBrowser_KeyCommand(object sender, int key)
        {
            if (KeyCommand != null)
                return KeyCommand(sender, key);
            return false;
        }
    }
}