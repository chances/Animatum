using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Animatum.Updater.Common;

namespace Animatum.Updater
{
    partial class InstallUpdate
    {
        public void RunProcessesCheck()
        {
            bw.DoWork += bw_DoWorkProcessesCheck;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompletedProcessesCheck;

            bw.RunWorkerAsync();
        }

        void bw_DoWorkProcessesCheck(object sender, DoWorkEventArgs e)
        {
            // processes
            List<FileInfo> files = null;
            List<Process> rProcesses = null;

            // rollback list for stopped services
            List<string> stoppedServices = new List<string>();
            Exception except = null; // store any errors

            // create the backup folder
            Directory.CreateDirectory(Path.Combine(TempDirectory, "backup"));

            try
            {
                files = new List<FileInfo>(new DirectoryInfo(ProgramDirectory).GetFiles("*.exe", SearchOption.AllDirectories));

                RemoveSelfFromProcesses(files);

                //check for (and delete) a newer client if it exists
                DeleteClientInPath(ProgramDirectory, Path.Combine(TempDirectory, "base"));

                rProcesses = ProcessesNeedClosing(files);

                if (rProcesses.Count == 0)
                {
                    // no processes need closing, all done
                    files = null;
                    rProcesses = null;
                }
                else if (SkipUIReporting) // and rProcesses.Count > 0
                {
                    // check every second for 20 seconds.
                    for (int i = 0; i < 20; ++i)
                    {
                        // sleep for 1 second
                        Thread.Sleep(1000);

                        rProcesses = ProcessesNeedClosing(files);

                        if (rProcesses.Count == 0)
                            break;
                    }

                    if (rProcesses.Count != 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.AppendLine(rProcesses.Count + " processes are running:\r\n");

                        foreach (Process proc in rProcesses)
                        {
                            sb.AppendLine(proc.MainWindowTitle + " (" + proc.ProcessName + ".exe)");
                        }

                        // tell the user about the open processes
                        throw new Exception(sb.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                except = ex;
            }

            // save rollback info
            RollbackUpdate.WriteRollbackServices(Path.Combine(TempDirectory, "backup\\stoppedServices.bak"), stoppedServices);

            if (IsCancelled() || except != null)
            {
                bw.ReportProgress(1, false);

                // rollback stopped services
                RollbackUpdate.RollbackStoppedServices(TempDirectory);

                bw.ReportProgress(0, new object[] { -1, -1, string.Empty, ProgressStatus.Failure, except });
            }
            else // completed successfully
            {
                bw.ReportProgress(0, new object[] { -1, -1, string.Empty, ProgressStatus.Success, new object[] { files, rProcesses } });
            }
        }

        void bw_RunWorkerCompletedProcessesCheck(object sender, RunWorkerCompletedEventArgs e)
        {
            bw.DoWork -= bw_DoWorkProcessesCheck;
            bw.ProgressChanged -= bw_ProgressChanged;
            bw.RunWorkerCompleted -= bw_RunWorkerCompletedProcessesCheck;
        }

        static void RemoveSelfFromProcesses(List<FileInfo> files)
        {
            for (int i = 0; i < files.Count; i++)
            {
                if (ProcessIsSelf(files[i].FullName))
                {
                    // remove self from the list of processes
                    files.RemoveAt(i);
                    return;
                }
            }
        }

        public static bool ProcessIsSelf(string processPath)
        {
            string self = VersionTools.SelfLocation;

#if DEBUG
            // exclude the vshost file when debugging
            if (string.Equals(processPath, self.Substring(0, self.Length - 3) + "vshost.exe", StringComparison.OrdinalIgnoreCase))
                return true;
#endif

            return string.Equals(processPath, self, StringComparison.OrdinalIgnoreCase);
        }

        static List<Process> ProcessesNeedClosing(List<FileInfo> baseFiles)
        {
            List<Process> rProcesses = new List<Process>();

            foreach (FileInfo filename in baseFiles)
            {
                Process[] aProcess = Process.GetProcessesByName(filename.Name.Replace(filename.Extension, ""));

                foreach (Process proc in aProcess)
                {
                    try
                    {
                        //are one of the exe's in baseDir running?
                        if (proc.MainModule != null && string.Equals(proc.MainModule.FileName, filename.FullName, StringComparison.OrdinalIgnoreCase))
                        {
                            rProcesses.Add(proc);
                        }
                    }
                    catch { }
                }
            }

            return rProcesses;
        }
    }
}