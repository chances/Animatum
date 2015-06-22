using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace Animatum.Updater.Common
{
    public partial class ServerFile
    {
        public static ServerFile Load(string fileName)
        {
            ServerFile serv = new ServerFile();

            string[] fileContents = null;

            try
            {
                fileContents = File.ReadAllLines(fileName);

                if (fileContents.Length < 7)
                    throw new InvalidOperationException("Invalid update file format.");
                // TODO: More robust format checking?
            }
            catch (Exception)
            {
                throw;
            }

            serv.VersionChoices.Add(new VersionChoice());

            serv.NewVersion = fileContents[0];
            serv.MinClientVersion = fileContents[1];

            VersionChoice version = new VersionChoice()
            {
                //InstallingTo = InstallingTo.BaseDir | InstallingTo.CommonAppData,
                InstallingTo = InstallingTo.BaseDir,
                Version = fileContents[2].Trim()
            };

            bool changelogSection = true;
            for (int i = 3; i < fileContents.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(fileContents[i]))
                {
                    if (!changelogSection) // Add dependent file
                        version.FileSites.Add(fileContents[i].Trim());
                    else // Add to the changelog
                        version.Changes += fileContents[i] + "\n";
                }
                else
                {
                    changelogSection = !changelogSection;
                }
            }

            serv.VersionChoices.Clear();
            serv.VersionChoices.Add(version);

            serv.NoUpdateToLatestLinkText = "No applicable updates were found. Please update manually.";
            serv.NoUpdateToLatestLinkURL = "https://github.com/chances/Animatum/releases";

            return serv;
        }

        public VersionChoice GetVersionChoice(string installedVersion)
        {
            VersionChoice updateFrom = null;

            for (int i = 0; i < VersionChoices.Count; i++)
            {
                // select the correct delta-patch version choice
                if (VersionTools.Compare(VersionChoices[i].Version, installedVersion) == 0)
                {
                    updateFrom = VersionChoices[i];
                    break;
                }
            }

            // if no delta-patch update has been selected, use the catch-all update (if it exists)
            if (updateFrom == null && CatchAllUpdateExists)
                updateFrom = VersionChoices[VersionChoices.Count - 1];

            if (updateFrom == null)
                throw new NoUpdatePathToNewestException();

            return updateFrom;
        }

        bool? catchAllExists;

        public bool CatchAllUpdateExists
        {
            get
            {
                if (catchAllExists == null)
                {
                    catchAllExists = VersionChoices.Count > 0 &&
                                     (VersionTools.Compare(VersionChoices[VersionChoices.Count - 1].Version, NewVersion) == 0
                                     || VersionChoices[VersionChoices.Count - 1].Version.Equals("any", StringComparison.OrdinalIgnoreCase));
                }

                return catchAllExists.Value;
            }
        }
    }
}