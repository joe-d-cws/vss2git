using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace VSS2Git
{
    public class ProcessHandler : IDisposable
    {
        private string commandLine;
        Process process;
        StringBuilder outputData;
        string outputString;
        IList<string> inputData;
        bool waitForExit;
        bool exited;
        bool running;
        string programName;

        public string CommandLine
        {
            get
            {
                return commandLine;
            }
            set
            {
                commandLine = value;
            }
        }

        public string ProgramName
        {
            get
            {
                return programName;
            }
            set
            {
                programName = value;
            }
        }

        public bool WaitForExit
        {
            get
            {
                return waitForExit;
            }
            set
            {
                waitForExit = value;
            }
        }

        public string ProgramOutput
        {
            get
            {
                if (outputData == null)
                {
                    return "";
                }
                if (outputString == null)
                {
                    outputString = outputData.ToString();
                }
                return outputString;
            }
        }

        public IList<String> ProgramInput
        {
            get
            {
                return inputData;
            }
            set
            {
                inputData = value;
            }
        }

        public Process ProcessHandle
        {
            get
            {
                return process;
            }
        }

        public bool HasExited
        {
            get
            {
                return exited;
            }
            set
            {
                exited = value;
            }
        }

        public bool Running
        {
            get
            {
                return running;
            }
            set
            {
                running = value;
            }
        }

        public int ExitCode
        {
            get
            {
                int result = 0;
                if (process != null)
                {
                    result = process.ExitCode;
                }
                return result;
            }
        }

        public DateTime ExitTime
        {
            get
            {
                DateTime result = DateTime.MinValue;
                if (process != null)
                {
                    result = process.ExitTime;
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
            commandLine = command;
        }

        public event EventHandler Exited;

        protected virtual void OnExited(EventArgs e)
        {
            EventHandler handler = Exited;
            if (handler != null)
            {
                // Invokes the delegates. 
                handler(this, e);
            }
        }

        private void ExitHandler(object sender, System.EventArgs e)
        {
            exited = true;
            running = false;
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
                if (process != null)
                {
                    throw new Exception("Process already running");
                }
                if (String.IsNullOrEmpty(commandLine) && String.IsNullOrEmpty(programName))
                {
                    throw new Exception("Command not set");
                }

                process = new Process();
                outputData = new StringBuilder();

                if (String.IsNullOrEmpty(programName))
                {
                    process.StartInfo.FileName = Environment.GetEnvironmentVariable("COMSPEC");
                    process.StartInfo.Arguments = "/C " + commandLine;
                }
                else
                {
                    process.StartInfo.FileName = programName;
                    process.StartInfo.Arguments = commandLine;
                    outputData.AppendFormat("{0} {1}\r\n", programName, commandLine);
                }

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                if (inputData != null)
                {
                    process.StartInfo.RedirectStandardInput = true;
                }
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);

                process.EnableRaisingEvents = true;

                process.Exited += ExitHandler;

                running = true;

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();


                if (inputData != null)
                {
                    foreach (string input in inputData)
                    {
                        process.StandardInput.WriteLine(input);
                    }
                }

                if (waitForExit)
                {
                    process.WaitForExit();
                }
            }
            catch
            {
                throw;
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
            if (process == null)
            {
                return;
            }
            process.Kill();
        }

        public void Reset()
        {
            if (process != null)
            {
                process.Dispose();
                process = null;
            }
            outputData = new StringBuilder();
            outputString = null;
            inputData = null;
            exited = false;
            running = false;
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
            try
            {
                // If this has already been called, return immediately
                if (disposed)
                {
                    return;
                }

                // if disposing is true, dispose was called from
                // code, so dispose managed resources.
                // if false, it was called by the runtime,
                // so just dispose the unmanaged stuff, if any.
                if (disposing)
                {
                    // dispose managed resources
                    if (process != null)
                    {
                        process.Dispose();
                        process = null;
                    }
                    outputData = null;
                }

                // dispose any unmanaged resources here

                // set flag so we don't call this again.
                disposed = true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Dispose respources
        /// </summary>
        public void Dispose()
        {
            try
            {
                // Dispose resources
                Dispose(true);
                // remove this from the finalization queue,
                // since we just disposed everything.
                GC.SuppressFinalize(this);
            }
            catch
            {
                throw;
            }
        }
        #endregion

    }
}
