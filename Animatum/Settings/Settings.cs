using System;
using System.Windows.Forms;
using System.Xml;

namespace Animatum.Settings
{
    //From: http://www.codeproject.com/Articles/15530/Quick-and-Dirty-Settings-Persistence-with-XML
    public class Settings
    {
        XmlDocument xmlDocument = new XmlDocument();
        string documentPath = Application.StartupPath + "//settings.xml";

        public Settings()
        {
            try { xmlDocument.Load(documentPath); }
            catch { xmlDocument.LoadXml("<settings></settings>"); }
        }

        public bool GetSetting(string xPath, bool defaultValue)
        { return bool.Parse(GetSetting(xPath, defaultValue.ToString())); }

        public void PutSetting(string xPath, bool value)
        { PutSetting(xPath, value.ToString()); }

        public int GetSetting(string xPath, int defaultValue)
        { return Int16.Parse(GetSetting(xPath, defaultValue.ToString())); }

        public void PutSetting(string xPath, int value)
        { PutSetting(xPath, value.ToString()); }

        public string GetSetting(string xPath, string defaultValue)
        {
            XmlNode xmlNode = xmlDocument.SelectSingleNode("settings/" + xPath);
            if (xmlNode != null) { return xmlNode.InnerText; }
            else { return defaultValue; }
        }

        public void PutSetting(string xPath, string value)
        {
            XmlNode xmlNode = xmlDocument.SelectSingleNode("settings/" + xPath);
            if (xmlNode == null) { xmlNode = createMissingNode("settings/" + xPath); }
            xmlNode.InnerText = value;
            xmlDocument.Save(documentPath);
        }

        private XmlNode createMissingNode(string xPath)
        {
            string[] xPathSections = xPath.Split('/');
            string currentXPath = "";
            XmlNode testNode = null;
            XmlNode currentNode = xmlDocument.SelectSingleNode("settings");
            foreach (string xPathSection in xPathSections)
            {
                currentXPath += xPathSection;
                testNode = xmlDocument.SelectSingleNode(currentXPath);
                if (testNode == null)
                {
                    currentNode.InnerXml += "<" +
                                xPathSection + "></" +
                                xPathSection + ">";
                }
                currentNode = xmlDocument.SelectSingleNode(currentXPath);
                currentXPath += "/";
            }
            return currentNode;
        }
    }
}