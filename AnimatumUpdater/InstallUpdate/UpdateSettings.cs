using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Animatum.Updater
{
    class UpdateSettings
    {
        public string InstalledVersion;

        public UpdateOn CurrentlyUpdating = UpdateOn.DownloadingUpdate;

        public List<string> ServerFileSites = new List<string>(1);
        public List<string> ClientServerSites = new List<string>(1);
        public string CompanyName = "Chance Snow";
        public string ProductName = "Animatum";

        string m_GUID;
        public string GUID
        {
            get
            {
                if (string.IsNullOrEmpty(m_GUID))
                {
                    // generate a GUID from the product name
                    char[] invalidChars = Path.GetInvalidFileNameChars();

                    if (ProductName != null && ProductName.IndexOfAny(invalidChars) != -1)
                    {
                        List<char> invalidFilenameChars = new List<char>(invalidChars);

                        // there are bad filename characters
                        //make a new string builder (with at least one bad character)
                        StringBuilder newText = new StringBuilder(ProductName.Length - 1);

                        //remove the bad characters
                        for (int i = 0; i < ProductName.Length; i++)
                        {
                            if (invalidFilenameChars.IndexOf(ProductName[i]) == -1)
                                newText.Append(ProductName[i]);
                        }

                        return newText.ToString();
                    }

                    return ProductName;
                }
                return m_GUID;
            }
            set
            {
                m_GUID = value;
            }
        }

        public bool CloseOnSuccess = false;

        public string PublicSignKey;
    }
}
