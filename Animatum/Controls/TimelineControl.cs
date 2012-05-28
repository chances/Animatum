using System;
using System.Windows.Forms;
using Animatum.SceneGraph;
using System.Drawing;
using System.Threading;
using System.IO;

namespace Animatum.Controls
{
    public partial class TimelineControl : UserControl
    {
        private string path = Application.StartupPath + "\\Timeline\\index.html";
        private TimelineScriptingObject scriptObj;
        private Model model;

        public event EventHandler Ready;
        public event EventHandler ModelUpdated;

        public TimelineControl()
        {
            InitializeComponent();

            scriptObj = new TimelineScriptingObject(null);
            scriptObj.ModelUpdated += new EventHandler(scriptObj_ModelUpdated);
            webBrowser.ObjectForScripting = scriptObj;
        }

        public Model Model
        {
            get { return model; }
            set
            {
                model = value;
                scriptObj.model = model;
                if (webBrowser.Document != null)
                    execScript("onModelUpdated();");
            }
        }

        private void OnReady()
        {
            if (Ready != null) Ready(this, new EventArgs());
        }

        private void OnModelUpdated()
        {
            if (ModelUpdated != null) ModelUpdated(this, new EventArgs());
        }

        void scriptObj_ModelUpdated(object sender, EventArgs e)
        {
            OnModelUpdated();
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
            scriptElem.InnerText = script;
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