using System;
using System.Windows.Forms;
using System.Xml;

namespace Animatum
{
    //From: http://www.codeproject.com/Articles/15530/Quick-and-Dirty-Settings-Persistence-with-XML
    public class Settings
    {
        static XmlDocument xmlDocument = new XmlDocument();
        static string documentPath = Application.StartupPath + "//settings.xml";

        public static void Load()
        {
            try { xmlDocument.Load(documentPath); }
            catch { xmlDocument.LoadXml("<settings></settings>"); }
        }

        public static bool GetSetting(string xPath, bool defaultValue)
        { return bool.Parse(GetSetting(xPath, defaultValue.ToString())); }

        public static void PutSetting(string xPath, bool value)
        { PutSetting(xPath, value.ToString()); }

        public static int GetSetting(string xPath, int defaultValue)
        { return Int16.Parse(GetSetting(xPath, defaultValue.ToString())); }

        public static void PutSetting(string xPath, int value)
        { PutSetting(xPath, value.ToString()); }

        public static string GetSetting(string xPath, string defaultValue)
        {
            XmlNode xmlNode = xmlDocument.SelectSingleNode("settings/" + xPath);
            if (xmlNode != null) { return xmlNode.InnerText; }
            else { return defaultValue; }
        }

        public static void PutSetting(string xPath, string value)
        {
            XmlNode xmlNode = xmlDocument.SelectSingleNode("settings/" + xPath);
            if (xmlNode == null) { xmlNode = createMissingNode("settings/" + xPath); }
            xmlNode.InnerText = value;
            xmlDocument.Save(documentPath);
        }

        static private XmlNode createMissingNode(string xPath)
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