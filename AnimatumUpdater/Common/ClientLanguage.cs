using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animatum.Updater.Common
{
    public class ScreenDialog
    {
        public string Title;
        public string SubTitle;
        public string Content;

        public bool IsEmpty
        {
            get
            {
                //if all the fields are empty then the screenDialog is empty
                return (string.IsNullOrEmpty(Title)
                    && string.IsNullOrEmpty(SubTitle)
                    && string.IsNullOrEmpty(Content));
            }
        }

        public ScreenDialog(string title, string subtitle, string content)
        {
            Title = title;
            SubTitle = subtitle;
            Content = content;
        }

        public void Clear()
        {
            Title = SubTitle = Content = null;
        }
    }

    public class ClientLanguage
    {
        // Language Name
        public string EnglishName, Name = "English", Culture = "en-US";

        // Buttons
        public string NextButton = "Next";
        public string InstallButton = "Install";
        public string UpdateButton = "Update";
        public string FinishButton = "Finish";
        public string CancelButton = "Cancel";
        public string ShowDetails = "Show details";

        // Dialogs
        public ScreenDialog ProcessDialog = new ScreenDialog("Close processes...",
                                               null,
                                               "The following processes need to be closed before updating can continue. Select a process and click Close Process.");

        public ScreenDialog FilesInUseDialog = new ScreenDialog("Files in use...",
                                                                "These files are used by the following processes:",
                                                                "The following files are in use. These files must be closed before the update can continue.");

        public ScreenDialog CancelDialog = new ScreenDialog("Cancel update?",
                                                            null,
                                                            "Are you sure you want to exit before the update is complete?");


        public string ClosePrc = "Close Process";
        public string CloseAllPrc = "Close All Processes";
        public string CancelUpdate = "Cancel Update";

        // Errors
        public string ServerError = "Unable to check for updates, the server file failed to load.";
        public string AdminError =
            "Animatum Updater needs administrative privileges to update Animatum. You can do this one of two ways:\r\n\r\n" +
            "1. When prompted, enter an administrator's username and password.\r\n\r\n" +
            "2. In Windows Explorer right click Animatum Updater.exe and click \"Run as Administrator\"";
        public string DownloadError = "The update failed to download.";
        public string GeneralUpdateError = "The update failed to install.";
        public string SelfUpdateInstallError =
            "The updated version of Animatum Updater required to update Animatum failed to install.";
        public string LogOffError = "Updating Animatum. You must cancel Animatum Updater before you can log off.";


        // Update Screens
        public ScreenDialog Checking = new ScreenDialog("Searching for updates",
                                          "Animatum Updater is searching for updates.",
                                          "Animatum Updater is searching for updates to Animatum. This process could take a few minutes.");

        public ScreenDialog UpdateInfo = new ScreenDialog("Update Information",
                                                          "Changes in the latest version of Animatum.",
                                                          "The version of Animatum installed on this computer is %old_version%. The latest version is %new_version%. Listed below are the changes and improvements:");

        public ScreenDialog DownInstall = new ScreenDialog("Downloading & Installing updates",
                                                           "Updating Animatum to the latest version.",
                                                           "Animatum Updater is downloading and installing updates for Animatum. This process could take a few minutes.");

        public ScreenDialog Uninstall = new ScreenDialog("Uninstalling files, folders, and registry",
                                                         "Uninstalling files and registry for Animatum.",
                                                         "Animatum Updater is uninstalling files created when updates were applied to Animatum.");

        public ScreenDialog SuccessUpdate = new ScreenDialog("Update successful!", null,
                                                             "Animatum has been successfully updated to version %new_version%");

        public ScreenDialog AlreadyLatest = new ScreenDialog("Latest version already installed", null,
                                                             "Animatum is currently up-to-date. Remember to check for new updates frequently.");

        public ScreenDialog NoUpdateToLatest = new ScreenDialog("No update to the latest version", null,
                                                                "There is a newer version of Animatum (version %new_version%), but no update available from the version you currently have installed (version %old_version%).");

        public ScreenDialog UpdateError = new ScreenDialog("An error occurred", null, null);


        // Bottom instructions
        public string UpdateBottom = "Click Update to begin.";
        public string FinishBottom = "Click Finish to exit.";

        // Status
        public string Download = "Downloading update";
        public string DownloadingSelfUpdate = "Downloading new Animatum Updater";
        public string SelfUpdate = "Updating Animatum Updater";
        public string Extract = "Extracting files";
        public string Processes = "Closing processes";
        public string PreExec = "Executing files";
        public string Files = "Backing up and updating files";
        public string Registry = "Backing up and updating registry";
        public string Optimize = "Optimizing and executing files";
        public string TempFiles = "Removing temporary files";
        public string UninstallFiles = "Uninstalling files & folders";
        public string UninstallRegistry = "Uninstalling registry";
        public string RollingBackFiles = "Rolling back files";
        public string RollingBackRegistry = "Rolling back registry";

        string m_ProductName, m_OldVersion, m_NewVersion;
        public string NewVersion { set { m_NewVersion = value; } }

        public void SetVariables(string product, string oldversion)
        {
            m_ProductName = product;
            m_OldVersion = oldversion;
        }

        string ParseText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            List<string> excludeVariables = new List<string>();

            return ParseVariableText(text, ref excludeVariables);
        }

        string ParseVariableText(string text, ref List<string> excludeVariables)
        {
            //parse a string, and return a pretty string (sans %%)
            StringBuilder returnString = new StringBuilder();

            int firstIndex = text.IndexOf('%', 0);

            if (firstIndex == -1)
            {
                //return the original
                return text;
            }

            returnString.Append(text.Substring(0, firstIndex));

            while (firstIndex != -1)
            {
                //find the next percent sign
                int currentIndex = text.IndexOf('%', firstIndex + 1);

                //if no closing percent sign...
                if (currentIndex == -1)
                {
                    //return the rest of the string
                    returnString.Append(text.Substring(firstIndex, text.Length - firstIndex));
                    return returnString.ToString();
                }


                //return the content of the variable
                string tempString = VariableToPretty(text.Substring(firstIndex + 1, currentIndex - firstIndex - 1), ref excludeVariables);

                //if the variable isn't defined
                if (tempString == null)
                {
                    //return the string with the percent signs
                    returnString.Append(text.Substring(firstIndex, currentIndex - firstIndex));
                }
                else
                {
                    //variable exists, add the parsed content
                    returnString.Append(tempString);
                    currentIndex++;
                    if (currentIndex == text.Length)
                    {
                        return returnString.ToString();
                    }
                }

                firstIndex = currentIndex;
            }

            return returnString.ToString();
        }

        string VariableToPretty(string variable, ref List<string> excludeVariables)
        {
            variable = variable.ToLower();

            if (excludeVariables.Contains(variable))
                return null;

            string returnValue;


            excludeVariables.Add(variable);

            switch (variable)
            {
                case "product":
                    returnValue = ParseVariableText(m_ProductName, ref excludeVariables);
                    break;
                case "old_version":
                    returnValue = ParseVariableText(m_OldVersion, ref excludeVariables);
                    break;
                case "new_version":
                    returnValue = ParseVariableText(m_NewVersion, ref excludeVariables);
                    break;
                default:
                    excludeVariables.RemoveAt(excludeVariables.Count - 1);
                    return null;
            }

            //allow the variable to be processed again
            excludeVariables.Remove(variable);

            return returnValue;
        }

        ScreenDialog ParseScreenDialog(ScreenDialog dialog)
        {
            return new ScreenDialog(ParseText(dialog.Title),
                ParseText(dialog.SubTitle),
                ParseText(dialog.Content));
        }
    }
}
