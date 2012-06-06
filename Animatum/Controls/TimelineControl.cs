using System;
using System.Windows.Forms;
using Animatum.SceneGraph;
using System.Drawing;
using System.Threading;
using System.IO;
using mshtml;

namespace Animatum.Controls
{
    public partial class TimelineControl : UserControl
    {
        private string path = Application.StartupPath + "\\Timeline\\index.html";
        private TimelineScriptingObject scriptObj;
        private Model model;
        private bool debugMode;

        public event EventHandler Ready;
        public event EventHandler ModelUpdated;
        public event EventHandler BeginPlayback;
        public event EventHandler PausePlayback;
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
            style += "font-family:\"" + this.Font.FontFamily.Name + "\";";
            style += "font-size:" + this.Font.SizeInPoints + "pt;";
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
    }
}