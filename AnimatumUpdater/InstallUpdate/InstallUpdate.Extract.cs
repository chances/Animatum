using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Animatum.Updater.Common;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;

namespace Animatum.Updater
{
    partial class InstallUpdate
    {
        // unzip the update to the temp folder
        public void RunUnzipProcess()
        {
            bw.DoWork += bw_DoWorkUnzip;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompletedUnzip;

            bw.RunWorkerAsync();
        }

        void bw_DoWorkUnzip(object sender, DoWorkEventArgs e)
        {
            Exception except = null;

            try
            {
                try
                {
                    // remove update file (it's no longer needed)
                    File.Delete(Filename);
                }
                catch { }

                var zipFiles = Directory.EnumerateFiles(TempDirectory, "*.zip");
                ExtractUpdateFiles(zipFiles);

                // Delete zip files
                foreach (string zipFile in zipFiles)
                {
                    try
                    {
                        File.Delete(zipFile);
                    }
                    catch { }
                }

                // Setup update details
                UpdtDetails = new UpdateDetails();

                var folders = Directory.EnumerateDirectories(OutputDirectory);
                foreach (string folder in folders)
                {
                    var files = Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                        UpdtDetails.UpdateFiles.Add(new UpdateFile()
                        {
                            Filename = file,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                except = ex;
            }

            if (IsCancelled() || except != null)
            {
                // report cancellation
                bw.ReportProgress(0, new object[] { -1, -1, "Cancelling update...", ProgressStatus.None, null });

                // Delete temporary files

                if (except != null && except.GetType() != typeof(PatchApplicationException))
                {
                    // remove the entire temp directory
                    try
                    {
                        Directory.Delete(OutputDirectory, true);
                    }
                    catch { }
                }

                bw.ReportProgress(0, new object[] { -1, -1, string.Empty, ProgressStatus.Failure, except });
            }
            else
            {
                bw.ReportProgress(0, new object[] { -1, -1, string.Empty, ProgressStatus.Success, null });
            }
        }

        void bw_RunWorkerCompletedUnzip(object sender, RunWorkerCompletedEventArgs e)
        {
            bw.DoWork -= bw_DoWorkUnzip;
            bw.ProgressChanged -= bw_ProgressChanged;
            bw.RunWorkerCompleted -= bw_RunWorkerCompletedUnzip;
        }

        void ExtractUpdateFiles(IEnumerable<string> zipFiles)
        {
            int totalFiles = 0;
            int filesDone = 0;

            foreach (string file in zipFiles)
            {
                totalFiles += GetZipEntriesCount(file);
            }

            foreach (string file in zipFiles)
            {
                string outDirectory = Path.Combine(OutputDirectory, "base");

                if (file.Contains("Timeline"))
                    outDirectory = Path.Combine(outDirectory, "Timeline");

                Directory.CreateDirectory(outDirectory);

                using (ZipInputStream zip = new ZipInputStream(File.OpenRead(file)))
                {
                    ZipEntry entry;
                    while ((entry = zip.GetNextEntry()) != null)
                    {
                        if (IsCancelled())
                            break; //stop outputting new files

                        if (!SkipProgressReporting && entry.IsFile)
                        {
                            int unweightedPercent = totalFiles > 0 ? (filesDone * 100) / totalFiles : 0;

                            bw.ReportProgress(0, new object[] { GetRelativeProgess(1, unweightedPercent), unweightedPercent, "Extracting " + Path.GetFileName(entry.Name), ProgressStatus.None, null });

                            filesDone++;
                        }

                        string directoryName = Path.GetDirectoryName(entry.Name);
                        string fileName = Path.GetFileName(entry.Name);

                        if (!String.IsNullOrEmpty(directoryName))
                        {
                            Directory.CreateDirectory(Path.Combine(outDirectory, directoryName));
                        }

                        if (!String.IsNullOrEmpty(fileName))
                        {
                            string filePath = Path.Combine(outDirectory, directoryName, fileName);
                            using (FileStream streamWriter = File.Open(filePath, FileMode.Create))
                            {

                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = zip.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        int GetZipEntriesCount(string filename)
        {
            int totalFiles = 0;
            using (ZipInputStream zip = new ZipInputStream(File.OpenRead(filename)))
            {
                ZipEntry entry;
                while((entry = zip.GetNextEntry()) != null) {
                    if (entry.IsFile)
                    {
                        totalFiles++;
                    }
                }
            }

            return totalFiles;
        }
    }
}