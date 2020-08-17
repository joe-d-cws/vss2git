using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace VSS2Git
{
    public class ProcessHandler : IDisposable
    {
        StringBuilder outputData;

        public string CommandLine
        {
            get; set;
        }

        public string ProgramName
        {
            get; set; 
        }

        public bool WaitForExit
        {
            get; set;
        }

        public bool HasExited
        {
            get; set;
        }

        public bool Running
        {
            get; set;
        }

        public IList<String> ProgramInput
        {
            get; set;
        }

        public string ProgramOutput
        {
            get
            {
                if (outputData == null)
                {
                    return "";
                }
                return outputData.ToString();
            }
        }


        public Process ProcessHandle
        {
            get; internal set;
        }

        public int ExitCode
        {
            get
            {
                int result = 0;
                if (ProcessHandle != null)
                {
                    result = ProcessHandle.ExitCode;
                }
                return result;
            }
        }

        public DateTime ExitTime
        {
            get
            {
                DateTime result = DateTime.MinValue;
                if (ProcessHandle != null)
                {
                    result = ProcessHandle.ExitTime;
                }
                return result;
            }
        }

        public ProcessHandler()
        {
            Reset();
        }

        public ProcessHandler(string command)
            : this()
        {
            CommandLine = command;
        }

        public event EventHandler Exited;

        protected virtual void OnExited(EventArgs e)
        {
            Exited?.Invoke(this, e);
        }

        private void ExitHandler(object sender, System.EventArgs e)
        {
            HasExited = true;
            Running = false;
            OnExited(e);
        }

        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                outputData.AppendFormat("{0}\r\n", outLine.Data);
            }
        }

        public void Run()
        {
            string curDir = null;

            try
            {
                if (ProcessHandle != null)
                {
                    throw new Exception("Process already running.");
                }
                if (String.IsNullOrEmpty(CommandLine) && String.IsNullOrEmpty(ProgramName))
                {
                    throw new Exception("Command not set.");
                }

                ProcessHandle = new Process();
                outputData = new StringBuilder();

                if (String.IsNullOrEmpty(ProgramName))
                {
                    ProcessHandle.StartInfo.FileName = Environment.GetEnvironmentVariable("COMSPEC");
                    ProcessHandle.StartInfo.Arguments = "/C " + CommandLine;
                }
                else
                {
                    ProcessHandle.StartInfo.FileName = ProgramName;
                    ProcessHandle.StartInfo.Arguments = CommandLine;
                    outputData.AppendFormat("{0} {1}\r\n", ProgramName, CommandLine);
                }

                ProcessHandle.StartInfo.UseShellExecute = false;
                ProcessHandle.StartInfo.RedirectStandardError = true;
                if (ProgramInput != null)
                {
                    ProcessHandle.StartInfo.RedirectStandardInput = true;
                }
                ProcessHandle.StartInfo.RedirectStandardOutput = true;
                ProcessHandle.StartInfo.CreateNoWindow = true;
                ProcessHandle.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                //ProcessHandle.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                ProcessHandle.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                ProcessHandle.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);

                ProcessHandle.EnableRaisingEvents = true;

                ProcessHandle.Exited += ExitHandler;

                Running = true;

                ProcessHandle.Start();

                ProcessHandle.BeginOutputReadLine();
                ProcessHandle.BeginErrorReadLine();


                if (ProgramInput != null)
                {
                    foreach (string input in ProgramInput)
                    {
                        ProcessHandle.StandardInput.WriteLine(input);
                    }
                }

                if (WaitForExit)
                {
                    ProcessHandle.WaitForExit();
                }
            }
            finally
            {
                if (curDir != null)
                {
                    Directory.SetCurrentDirectory(curDir);
                }
            }
        }

        public void Kill()
        {
            if (ProcessHandle == null)
            {
                return;
            }
            ProcessHandle.Kill();
        }

        public void Reset()
        {
            if (ProcessHandle != null)
            {
                ProcessHandle.Dispose();
                ProcessHandle = null;
            }
            outputData = new StringBuilder();
            ProgramInput = null;
            HasExited = false;
            Running = false;
        }

        #region IDisposable Members
        /// <summary>
        /// Indicates if Dispose() has already been called.
        /// </summary>
        private bool disposed;  // initial value = false

        /// <summary>
        /// Dispose managed and unmanaged resources
        /// </summary>
        /// <param name="disposing">TRUE: Dispose managed and unmanaged resources
        /// FALSE: Dispose unmanaged resources only</param>
        protected virtual void Dispose(bool disposing)
        {
            // If this has already been called, return immediately
            if (disposed)
            {
                return;
            }

            // dispose managed resources
            if (ProcessHandle != null)
            {
                ProcessHandle.Dispose();
                ProcessHandle = null;
            }

            outputData = null;

            // dispose any unmanaged resources here

            // set flag so we don't call this again.
            disposed = true;
        }

        /// <summary>
        /// Dispose respources
        /// </summary>
        public void Dispose()
        {
            // Dispose resources
            Dispose(true);
            // remove this from the finalization queue,
            // since we just disposed everything.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
