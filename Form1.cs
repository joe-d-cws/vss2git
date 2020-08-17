using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

using Microsoft.VisualStudio.SourceSafe.Interop;

namespace VSS2Git
{
    public partial class Form1 : Form
    {
        private int checkinGap;
        private List<ItemInfo> itemList;
        private List<ItemInfo> projectList;
        private string lastCheckPath = "";
        private string extractPath;
        private string logFile;
        private string cmdLogFile;
        private string createCommand;
        private string openCommand;
        private string addCommand;
        private string updateCommand;
        private string commitCommand;
        private string closeCommand;
        private StringBuilder sbStatus;
        private bool testMode;
        private DateTime earliestDate;
        private DateTime latestDate;
        string configFileName = "";
        string rootPath = "";

        private const string DateFormat = "yyyy-MM-dd HH:mm:ss";

        public Form1()
        {
            InitializeComponent();

            SetDefaults();
        }

        private void SetDefaults()
        {
            RegistryKey software = null;

            try
            {
                configFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "VSS2Git.cfg");

                // set default extract path
                txtExtractPath.Text = Environment.ExpandEnvironmentVariables(txtExtractPath.Text);
                txtLog.Text = Path.Combine(txtExtractPath.Text, "VSS2Git.log");
                txtReport.Text = Path.Combine(txtExtractPath.Text, "VSS2Git.html");
                txtCmdLog.Text = Path.Combine(txtExtractPath.Text, "VSS2Git-commands.txt");

                // Set default database and user names

                // database is in HKEY_CURRENT_USER\Software\Microsoft\Sourcesafe\Current Database

                software = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Sourcesafe");
                if (software != null)
                {
                    string vssname = (string)software.GetValue("Current Database");

                    if (!String.IsNullOrEmpty(vssname))
                    {
                        txtVss.Text = Path.Combine(vssname, "srcsafe.ini");
                    }
                }

                txtSSUser.Text = Environment.UserName;

                // default values set, now override them from config

                LoadConfig(configFileName);
            }
            finally
            {
                if (software != null)
                {
                    software.Close();
                }
            }
        }

        private void SaveConfig(string fileName)
        {
            string options = String.Format("SSDatabase={0}\r\n" +
                                           "SSUser={1}\r\n" +
                                           "SSPassword={2}\r\n" +
                                           "SSRoot={3}\r\n" +
                                           "ExtractPath={4}\r\n" +
                                           "SCMCreate={5}\r\n" +
                                           "SCMOpen={6}\r\n" +
                                           "SCMAddNew={7}\r\n" +
                                           "SCMUpdate={8}\r\n" +
                                           "SCMCommit={9}\r\n" +
                                           "SCMClose={10}\r\n" +
                                           "LogFile={11}\r\n" +
                                           "ReportFile={12}\r\n" +
                                           "CommandLog={13}\r\n" +
                                           "TestMode={14}\r\n",
                                           txtVss.Text, txtSSUser.Text, txtSSPassword.Text, txtSSRoot.Text,
                                           txtExtractPath.Text, txtCreate.Text, txtOpen.Text, txtAdd.Text, txtUpdate.Text, txtCommit.Text, txtClose.Text,
                                           txtLog.Text, txtReport.Text, txtCmdLog.Text, chkTest.Checked.ToString());

            try
            {
                File.WriteAllText(fileName, options);
            }
            finally
            {
            }
        }

        private bool LoadConfig(string fileName)
        {
            string[] lines;
            try 
            {
                lines = File.ReadAllLines(fileName);
            }
            catch 
            {
                lines = null;
            }
            if (lines == null || lines.Length == 0) 
            {
                return false;
            }
            foreach (string line in lines) 
            {
                int eqPos = line.IndexOf('=');
                if (eqPos >= 0) {
                    string settingValue;
                    if (eqPos + 1 < line.Length) 
                    {
                        settingValue = line.Substring(eqPos + 1);
                    }
                    else 
                    {
                        settingValue = "";
                    }
                    if (line.StartsWith("SSDatabase=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtVss.Text = settingValue;
                    }
                    else if (line.StartsWith("SSUser=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtSSUser.Text = settingValue;
                    }
                    else if (line.StartsWith("SSPassword=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtSSPassword.Text = settingValue;
                    }
                    else if (line.StartsWith("SSRoot=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtSSRoot.Text = settingValue;
                    }
                    else if (line.StartsWith("ExtractPath=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtExtractPath.Text = settingValue;
                    }
                    else if (line.StartsWith("SCMCreate=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtCreate.Text = settingValue;
                    }
                    else if (line.StartsWith("SCMOpen=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtOpen.Text = settingValue;
                    }
                    else if (line.StartsWith("SCMAddNew=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtAdd.Text = settingValue;
                    }
                    else if (line.StartsWith("SCMUpdate=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtUpdate.Text = settingValue;
                    }
                    else if (line.StartsWith("SCMCommit=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtCommit.Text = settingValue;
                    }
                    else if (line.StartsWith("SCMClose=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtClose.Text = settingValue;
                    }
                    else if (line.StartsWith("LogFile=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtLog.Text = settingValue;
                    }
                    else if (line.StartsWith("ReportFile=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtReport.Text = settingValue;
                    }
                    else if (line.StartsWith("CommandLog=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtCmdLog.Text = settingValue;
                    }
                    else if (line.StartsWith("TestMode=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        chkTest.Checked = (settingValue.ToLower() == "true");
                    }
                }
            }
            return true;
        }

        private void StatusMessage(string format, params object[] args)
        {
            string message = String.Format(format, args);
            if (!String.IsNullOrEmpty(logFile))
            {
                File.AppendAllText(logFile, message);
            }
            sbStatus.AppendFormat(message);
        }

        private void SetStatusLabel(string message)
        {
            if (lblStatus.Text == message)
            {
                return;
            }
            lblStatus.Text = message;
            lblStatus.Refresh();
        }

        private void RunCommand(string commandLine)
        {
            if (String.IsNullOrEmpty(commandLine))
            {
                return;
            }
            if (!String.IsNullOrEmpty(cmdLogFile))
            {
                File.AppendAllText(cmdLogFile, commandLine + "\r\n");
            }
            if (testMode)
            {
                StatusMessage("{0}\r\n\r\n", commandLine);
                return;
            }
            using (ProcessHandler ph = new ProcessHandler())
            {
                ph.CommandLine = commandLine;
                ph.WaitForExit = true;

                StatusMessage("{0}\r\n{1}\r\n", DateTime.Now.ToString(DateFormat), commandLine);

                ph.Run();

                StatusMessage("{0}\r\n{1}\r\n", DateTime.Now.ToString(DateFormat), ph.ProgramOutput);
            }
        }

        /// <summary>
        /// Process a VSS item, and recurse through the children
        /// </summary>
        /// <param name="vssItem"></param>
        private void DumpSubitems(IVSSItem vssItem)
        {
            if (vssItem.Deleted)
            {
                // skip deleted items.
                return;
            }
            // Type = 0 for projects, 1 for files.
            if (vssItem.Type == 0)
            {
                // Process a project.

                // Display the project name 
                StatusMessage("{0}\r\n", vssItem.Spec);
                SetStatusLabel(vssItem.Spec);

                ItemInfo project = new ItemInfo(vssItem, vssItem.VSSVersion);

                // Now process all the children of this project
                foreach (IVSSItem item in vssItem.get_Items(true))
                {
                    DumpSubitems(item);
                }
            }
            else
            {
                // process a file.

                StatusMessage("1 {0}\r\n", vssItem.Spec);

                // Get each version of this file, and save to the version list.
                foreach (IVSSVersion vssVersion in vssItem.get_Versions(0))
                {
                    ItemInfo item = new ItemInfo(vssItem, vssVersion);
                    if (item.VersionDate < earliestDate)
                    {
                        earliestDate = item.VersionDate;
                    }
                    if (item.VersionDate > latestDate)
                    {
                        latestDate = item.VersionDate;
                    }
                    itemList.Add(item);
                }
            }
        }

        // make sure the specified directory exists.
        private void CheckPath(string pathName)
        {
            // if this is the same as the last path
            // we checked, return.
            // This saves a bit of time.
            if (pathName == lastCheckPath)
            {
                return;
            }
            lastCheckPath = pathName;
            if (Directory.Exists(pathName))
            {
                return;
            }
            Directory.CreateDirectory(pathName);
            //StatusMessage("Create path: {0}\r\n", pathName);
        }

        private void DumpItems(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return;
            }
            TextWriter tr = new StreamWriter(fileName);
            tr.WriteLine("<HTML><HEAD></HEAD><BODY><TABLE>");
            tr.WriteLine("<TR><TD>ITEM_TYPE</TD><TD>PARENT</TD><TD>PHYSICAL</TD><TD>SPEC</TD><TD>VERSION_NUMBER</TD><TD>VERSION_DATE</TD><TD>ACTION</TD><TD>COMMENT</TD><TD>NAME</TD><TD>LOCAL_SPEC</TD><TD>LABEL</TD><TD>LABEL_COMMENT</TD><TD>USER_NAME</TD></TR>");
            foreach (ItemInfo item in itemList)
            {
                tr.WriteLine("<TR><TD>{0}</TD><TD>{1}</TD><TD>{2}</TD><TD>{3}</TD><TD>{4}</TD><TD>{5}</TD><TD>{6}</TD><TD>{7}</TD><TD>{8}</TD><TD>{9}</TD><TD>{10}</TD><TD>{11}</TD><TD>{12}</TD></TR>", 
                                item.ItemType, item.Parent, item.Physical, item.Spec,
                                item.VersionNumber, item.VersionDate, item.Action, item.Comment,
                                item.Name, item.LocalSpec, item.Label, item.LabelComment, item.UserName);
            }
            tr.WriteLine("</TABLE></BODY></HTML>");
            tr.Close();
        }

        private string FixComment(string comment)
        {
            // remove cr, lf, tab from comment
            string result;

            if (String.IsNullOrEmpty(comment))
            {
                return "";
            }

            result = comment.Trim();

            result = result.Replace('"', '\'');
            result = result.Replace('\r', ' ');
            result = result.Replace('\n', ' ');
            result = result.Replace('\t', ' ');

            while (result.IndexOf("  ") >= 0)
            {
                result = result.Replace("  ", " ");
            }

            return result.Trim();
        }

        /// <summary>
        /// Extract individual file versions, and commit to the new source control
        /// </summary>
        private void Transfer()
        {
            DateTime lastCommit = DateTime.MinValue;
            int commitCount = 0;
            int checkinCount = 0;
            bool newFile;
            bool isDirectory;
            string commandLine;
            string curDir = null;
            List<String> commits = new List<string>();
            string commitComment = "";
            string commitDate = "";
            string itemComment;
            string rootStrip;

            // Make sure the base path exists,
            // then make it the current directory
            CheckPath(extractPath);

            if (rootPath.StartsWith("$/") && rootPath.Length > 2)
            {
                rootStrip = rootPath.Substring(2) + "/";
            }
            else
            {
                rootStrip = "";
            }

            curDir = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(extractPath);
            }
            catch
            {
                // if unable to change dir,
                // don't worry about it.
            }

            if (earliestDate > DateTime.Now)
            {
                earliestDate = DateTime.Now;
            }

            // Run the "create" command
            if (!String.IsNullOrEmpty(createCommand))
            {
                RunCommand(String.Format(createCommand, extractPath, earliestDate.ToString(DateFormat)));
            }

            // Run the "open" command
            if (!String.IsNullOrEmpty(openCommand))
            {
                RunCommand(String.Format(openCommand, extractPath));
            }

            lastCheckPath = "";

            for (int i = 0; i < itemList.Count; i++)
            {
                // spin through the results, extracting and checking in as we go.
                // We will check in whenever the version date changes.

                // NOTE: The date is set for each item individually, even when multiple items were checked in
                // at the same time.  If the checkin takes a long time, then the date will advance even though they
                // were all checked in at the same time.  The longest gap I saw was 10 seconds, 
                // so we will wait until there's a gap of more than 10 seconds between checkin dates to do the
                // actual checkin.
                // (gap interval is in checkinGap)
                if (itemList[i].ItemType != 0)
                {
                    if ((commits.Contains(itemList[i].Spec) || itemList[i].VersionDate > lastCommit) && commitCount != 0)
                    {
                        if (itemList[i].VersionDate <= lastCommit)
                        {
                            StatusMessage("********************\r\n");
                        }
                        commitDate = lastCommit.ToString(DateFormat);
                        if (String.IsNullOrEmpty(commitComment))
                        {
                            commitComment = commitDate;
                        }
                        // checkin time gap found, commit
                        SetStatusLabel(commitDate);
                        string msg = String.Format("Commit {0} items ({1})\r\nCheckin gap: {2:0000000}\r\n", commitCount, commitDate, (itemList[i].VersionDate - lastCommit).TotalSeconds+checkinGap);
                        StatusMessage(msg);
                        commitCount = 0;
                        checkinCount += 1;
                        if (!String.IsNullOrEmpty(commitCommand))
                        {
                            RunCommand(String.Format(commitCommand, commitDate, commitComment));
                        }
                        commits.Clear();
                        commitComment = "";
                        if (itemList[i].VersionDate > latestDate)
                        {
                            // hit end of list, break
                            break;
                        }
                    }

                    // set the last commit to "checkinGap" seconds after the current version date.
                    // this becomes the new threshold for the commit check.
                    lastCommit = itemList[i].VersionDate.AddSeconds(checkinGap);

                    commits.Add(itemList[i].Spec);

                    itemComment = FixComment(itemList[i].Comment);

                    if (!String.IsNullOrEmpty(itemComment))
                    {
                        commitComment = itemComment;
                    }

                    StatusMessage("{0} (ver {1} - {2} {3} {4})\r\n", itemList[i].Spec, itemList[i].VersionNumber, itemList[i].VersionDate, itemList[i].Action ?? "", itemComment);

                    
                    // strip root path from spec
                    string itemName = itemList[i].Spec.Substring(2);
                    if (!String.IsNullOrEmpty(rootStrip))
                    {
                        if (itemName.StartsWith(rootStrip))
                        {
                            itemName = itemName.Substring(rootStrip.Length);
                        }
                        else
                        {
                            StatusMessage("******************* Unable to strip root path.\r\n");
                        }
                    }


                    // strip the $/ from the start of the item spec, and replace / with \
                    itemName = itemName.Replace('/', '\\');

                    // get the full path name to extract to
                    string fileName = Path.Combine(extractPath, itemName);

                    // then get the directory for that full path
                    string pathName = Path.GetDirectoryName(fileName);

                    // make sure the directory exists
                    CheckPath(pathName);

                    string getName = fileName;
                    
                    isDirectory = false;

                    newFile = !File.Exists(fileName);

                    if (newFile)
                    {
                        isDirectory = Directory.Exists(fileName);
                    }

                    if (!isDirectory)
                    {
                        // extract the file.
                        // Enclose this in a try/catch block in case the extract fails.
                        // I've seen this fail if older versions of the file are missing.
                        bool extracted;

                        // delete any existing version of the file
                        File.Delete(getName);

                        try
                        {
                            itemList[i].VSSItem.Get(ref getName, (int)VSSFlags.VSSFLAG_CMPFAIL);
                            extracted = true;
                        }
                        catch (Exception ex)
                        {
                            extracted = false;
                            StatusMessage(ex.Message);
                        }

                        if (extracted)
                        {
                            // set the file date to the checkin time.
                            File.SetAttributes(fileName, FileAttributes.Normal);
                            File.SetCreationTime(fileName, itemList[i].VersionDate);
                            File.SetLastWriteTime(fileName, itemList[i].VersionDate);

                            commitCount += 1;

                            commandLine = null;

                            if (newFile)
                            {
                                if (!String.IsNullOrEmpty(addCommand))
                                {
                                    // new file - use the add command
                                    commandLine = String.Format(addCommand, fileName, itemList[i].Comment ?? "");
                                }
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(updateCommand))
                                {
                                    // new version of existing file - use the update command
                                    commandLine = String.Format(updateCommand, fileName, itemList[i].Comment ?? "");
                                }
                            }
                            if (!String.IsNullOrEmpty(commandLine))
                            {
                                // run the add/update command
                                RunCommand(commandLine);
                            }
                        }
                    }
                }
            }

            // close the SCM
            if (!String.IsNullOrEmpty(closeCommand))
            {
                RunCommand(closeCommand);
            }

            if (curDir != null)
            {
                Directory.SetCurrentDirectory(curDir);
            }
            StatusMessage("Total checkins: {0}\r\n", checkinCount);
            SetStatusLabel("Done.");
        }

        private void DumpVssDatabase()
        {
            rootPath = txtSSRoot.Text;

            Cursor oldCursor = this.Cursor;

            SaveConfig(configFileName);

            try
            {
                sbStatus = new StringBuilder();
                lblStatus.Text = "";
                this.Cursor = Cursors.WaitCursor;

                // get various settings from the UI
                extractPath = txtExtractPath.Text.Trim();
                logFile = txtLog.Text.Trim();
                cmdLogFile = txtCmdLog.Text.Trim();
                createCommand = txtCreate.Text.Trim();
                openCommand = txtOpen.Text.Trim();
                addCommand = txtAdd.Text.Trim();
                updateCommand = txtUpdate.Text.Trim();
                commitCommand = txtCommit.Text.Trim();
                closeCommand = txtClose.Text.Trim();
                testMode = chkTest.Checked;
                earliestDate = DateTime.MaxValue;
                latestDate = DateTime.MinValue;
                checkinGap = 10;
                itemList = new List<ItemInfo>();
                projectList = new List<ItemInfo>();
                lastCheckPath = "";

                // Make sure the base path exists,
                CheckPath(extractPath);

                // Create database instance...
                IVSSDatabase vssDatabase = new VSSDatabase();

                // and open it
                vssDatabase.Open(txtVss.Text, txtSSUser.Text, txtSSPassword.Text);

                // Get the root item
                IVSSItem vssRoot = vssDatabase.get_VSSItem(rootPath, false);

                // Process the root item.  This will also recurse through everything else.
                DumpSubitems(vssRoot);

                // add a dummy record that will sort to the end
                ItemInfo item = new ItemInfo();
                item.VersionDate = latestDate.AddSeconds(checkinGap + 1);
                item.Spec = "~";
                item.VersionNumber = int.MaxValue;
                item.ItemType = 1;
                itemList.Add(item);

                // sort all versions by date, name and version
                itemList.Sort(new ItemInfoComparer());

                // create HTML report
                DumpItems(txtReport.Text);

                // Now extract individual file versions,
                // and commit them to new source control

                Transfer();

                // Close the VSS database
                vssDatabase.Close();
                vssDatabase = null;

                StatusMessage("Done.\r\n");

                txtStatus.Text = sbStatus.ToString();
            }
            finally
            {
                this.Cursor = oldCursor;
            }

        }

        private void btnDump_Click(object sender, EventArgs e)
        {
            //try
            //{
                DumpVssDatabase();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }
    }
}
