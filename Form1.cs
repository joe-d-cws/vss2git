﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

using Microsoft.VisualStudio.SourceSafe.Interop;

namespace VSS2Git
{
    public partial class Form1 : Form
    {
        private int gapInterval;
        private List<ItemInfo> itemList;
        private List<ItemInfo> projectList;
        private string lastCheckPath = "";
        private string lastChangeDir = "";
        private string extractPath;
        private string logFile;
        private string remoteRepoBasePath;
        private string remoteRepoBaseUrl;
        private StringBuilder sbStatus;
        private bool testMode;
        private bool flatten;
        private bool autoPush;
        private DateTime earliestDate;
        private DateTime latestDate;
        string configFileName = "";
        string rootPath = "";
        string gitIgnore = "";
        List<String> projectNames;

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
                                           "LogFile={5}\r\n" +
                                           "ReportFile={6}\r\n" +
                                           "RemoteRepoUrl={7}\r\n" +
                                           "RemoteRepoPath={8}\r\n" +
                                           "TestMode={9}\r\n" +
                                           "AutoPushToRemote={10}\r\n" +
                                           "Flatten={11}\r\n",
                                           txtVss.Text, txtSSUser.Text, txtSSPassword.Text, txtSSRoot.Text,
                                           txtExtractPath.Text,
                                           txtLog.Text, txtReport.Text, txtRemoteUrl.Text, txtRemotePath.Text,
                                           chkTest.Checked, chkAutoPush.Checked, chkFlatten.Checked);

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
                if (eqPos >= 0)
                {
                    string settingValue;
                    bool boolValue;

                    if (eqPos + 1 < line.Length)
                    {
                        settingValue = line.Substring(eqPos + 1);
                        boolValue = (settingValue.ToLower() == "true");
                    }
                    else
                    {
                        settingValue = "";
                        boolValue = false;
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
                    else if (line.StartsWith("LogFile=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtLog.Text = settingValue;
                    }
                    else if (line.StartsWith("ReportFile=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtReport.Text = settingValue;
                    }
                    else if (line.StartsWith("RemoteRepoUrl=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtRemoteUrl.Text = settingValue;
                    }
                    else if (line.StartsWith("RemoteRepoPath=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtRemotePath.Text = settingValue;
                    }
                    else if (line.StartsWith("TestMode=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        chkTest.Checked = boolValue;
                    }
                    else if (line.StartsWith("AutoPushToRemote=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        chkAutoPush.Checked = boolValue;
                    }
                    else if (line.StartsWith("Flatten=", StringComparison.CurrentCultureIgnoreCase))
                    {
                        chkFlatten.Checked = boolValue;
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
            sbStatus.Append(message);
        }

        private void SetStatusLabel(string message)
        {
            if (lblStatus.Text == message)
            {
                return;
            }
            StatusMessage("----------------------------------\r\n");
            lblStatus.Text = message;
            lblProgress.Text = "";
            lblStatus.Refresh();
            lblProgress.Refresh();
        }

        private void SetProgressLabel(string message)
        {
            if (lblProgress.Text == message)
            {
                return;
            }
            lblProgress.Text = message;
            lblProgress.Refresh();
        }

        private void RunCommand(string commandLine)
        {
            if (String.IsNullOrEmpty(commandLine))
            {
                return;
            }

            StatusMessage("{0}\r\n", commandLine);

            if (!testMode)
            {
                using (ProcessHandler ph = new ProcessHandler())
                {
                    ph.CommandLine = commandLine;
                    ph.WaitForExit = true;

                    ph.Run();

                    StatusMessage("{0}\r\n", ph.ProgramOutput);
                }
            }
        }

        /// <summary>
        /// Process a VSS item, and recurse through the children
        /// </summary>
        /// <param name="vssItem"></param>
        private void DumpSubitems(IVSSItem vssItem, ItemInfo parentProject)
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
                SetProgressLabel(vssItem.Spec);

                ItemInfo project = new ItemInfo(vssItem, vssItem.VSSVersion);

                project.Project = parentProject;

                // Now process all the children of this project
                foreach (IVSSItem item in vssItem.get_Items(true))
                {
                    DumpSubitems(item, project);
                }
                StatusMessage("Files: {0}\r\n", project.FileCount);
                //itemList.Add(project);
            }
            else
            {
                // process a file.

                StatusMessage("1 {0}\r\n", vssItem.Spec);

                // Get each version of this file, and save to the version list.
                foreach (IVSSVersion vssVersion in vssItem.get_Versions(0))
                {
                    ItemInfo item = new ItemInfo(vssItem, vssVersion);
                    item.Project = parentProject;
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
                parentProject.FileCount++;
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

        private string EncodeItem(string item)
        {
            if (String.IsNullOrEmpty(item))
            {
                return "";
            }
            return item.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;"); ;
        }

        private void DumpItems(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return;
            }
            TextWriter tr = new StreamWriter(fileName);
            tr.Write("<HTML>\r\n" +
                     "<HEAD>\r\n" +
                     "<STYLE>\r\n" +
                     "table, th, td {\r\n" +
                     "  border: 1px solid black;\r\n" +
                     "  white-space: nowrap;\r\n" +
                     "}\r\n" +
                     "</STYLE>\r\n" +
                     "</HEAD>\r\n" +
                     "<BODY>\r\n" +
                     "<TABLE>\r\n" +
                     "<TR><TD>ITEM_TYPE</TD><TD>PROJECT</TD><TD>PARENT</TD><TD>PHYSICAL</TD><TD>SPEC</TD><TD>VERSION_NUMBER</TD><TD>VERSION_DATE</TD><TD>ACTION</TD><TD>COMMENT</TD><TD>NAME</TD><TD>LOCAL_SPEC</TD><TD>LABEL</TD><TD>LABEL_COMMENT</TD><TD>USER_NAME</TD></TR>\r\n");
            foreach (ItemInfo item in itemList)
            {
                string projectName = item.FindProjectName();

                tr.Write("<TR><TD>{0}</TD><TD>{1}</TD><TD>{2}</TD><TD>{3}</TD><TD>{4}</TD><TD>{5}</TD><TD>{6}</TD><TD>{7}</TD><TD>{8}</TD><TD>{9}</TD><TD>{10}</TD><TD>{11}</TD><TD>{12}</TD><TD>{13}</TD></TR>\r\n",
                                EncodeItem(item.ItemType.ToString()), EncodeItem(projectName), EncodeItem(item.Parent), EncodeItem(item.Physical),
                                EncodeItem(item.Spec), EncodeItem(item.VersionNumber.ToString()), EncodeItem(item.VersionDate.ToString(DateFormat)), EncodeItem(item.Action),
                                EncodeItem(item.Comment), EncodeItem(item.Name), EncodeItem(item.LocalSpec), EncodeItem(item.Label),
                                EncodeItem(item.LabelComment), EncodeItem(item.UserName));
            }
            tr.Write("</TABLE>\r\n" +
                         "</BODY>\r\n" +
                         "</HTML>");
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

        private void ChangeDirectory(string newdir)
        {
            if (newdir == lastChangeDir)
            {
                return;
            }
            Directory.SetCurrentDirectory(newdir);
            StatusMessage("cd \"{0}\"\r\n", newdir);
            lastChangeDir = newdir;
        }

        private bool DeleteFile(string fileName)
        {
            try
            {
                var dir = new DirectoryInfo(".");

                foreach (var file in dir.EnumerateFiles(fileName))
                {
                    file.Delete();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void GitInit(string projectPath)
        {
            ChangeDirectory(projectPath);

            if (!testMode && !String.IsNullOrEmpty(gitIgnore) && !File.Exists(".gitignore"))
            {
                StatusMessage("Creating .gitignore\r\n");
                File.WriteAllText(".gitignore", gitIgnore);
            }

            RunCommand("git init");
        }

        private void GitAdd(string projectPath, string fileName, string comment)
        {
            ChangeDirectory(projectPath);

            RunCommand(String.Format("git add \"{0}\"", fileName));
        }

        private void GitCommit(string projectPath, string comment, string date)
        {
            string newComment = "";

            if (!String.IsNullOrEmpty(date))
            {
                newComment += date;
            }
            if (!String.IsNullOrEmpty(comment))
            {
                if (newComment.Length > 0)
                {
                    newComment += " - ";
                }
                newComment += comment;
            }

            ChangeDirectory(projectPath);

            if (!testMode)
            {
                DeleteFile("*.scc");
                DeleteFile("*.vspscc");
                DeleteFile("*.vssscc");
            }


            RunCommand("git add -A");

            if (String.IsNullOrEmpty(newComment))
            {
                RunCommand(String.Format("git commit -a --allow-empty-message"));
            }
            else
            {
                RunCommand(String.Format("git commit -a -m \"{0}\"", date, newComment));
            }
        }

        private string MakeGitRepoName(string projectPath, string projectName)
        {
            string result;

            if (flatten)
            {
                string stub = projectName.Replace(' ', '-').ToLower();
                int count = 1;
                result = stub;
                while (projectNames.Contains(result))
                {
                    count++;
                    result = stub + "-" + count.ToString();
                }
                projectNames.Add(result);
            }
            else
            {
                result = projectPath.Replace(' ', '-').ToLower();
            }
            return result + ".git";
        }

        /// <summary>
        /// Extract individual file versions, and commit to git
        /// </summary>
        private void Transfer()
        {
            DateTime lastCommit = DateTime.MinValue;
            int commitCount = 0;
            int checkinCount = 0;
            bool newFile;
            bool isDirectory;
            string curDir = null;
            List<String> commits = new List<string>();
            string commitComment = "";
            string commitDate = "";
            string itemComment;
            string rootStrip;
            ItemInfo project = null;
            string currentProjectName = "";
            string currentProjectPath = "";
            string currentRemoteRepoPath = "";
            string currentRemoteRepoUrl = "";
            bool extracted;
            List<ProjectInfo> cleanup = new List<ProjectInfo>();
            List<String> gitProjectList = new List<String>();
            StringBuilder createRemoteRepo = new StringBuilder();
            StringBuilder pushRemoteRepo = new StringBuilder();
            List<string> projectXref = new List<String>();

            SetStatusLabel("Extracting and committing");

            // Make sure the base path exists,
            // then make it the current directory
            CheckPath(extractPath);

            if (rootPath.StartsWith("$/") && rootPath.Length > 2)
            {
                rootStrip = Path.GetDirectoryName(rootPath.Substring(2)) ?? "";
                // GetDirectoryName will convert / to \.  Reverse that.
                rootStrip = rootStrip.Replace('\\', '/');
                if (!rootStrip.EndsWith("/"))
                {
                    rootStrip = rootStrip + "/";
                }
            }
            else
            {
                rootStrip = "";
            }

            curDir = Directory.GetCurrentDirectory();

            Directory.SetCurrentDirectory(extractPath);

            if (earliestDate > DateTime.Now)
            {
                earliestDate = DateTime.Now;
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
                // (gap interval is in gapInterval)

                if (itemList[i].ItemType != 0)
                {
                    project = itemList[i].FindProject();

                    if (
                          (project.Spec != currentProjectName)        // if the project has changed...
                          || (commits.Contains(itemList[i].Spec))     // or a previous version of this file is already staged to commit...
                          || (itemList[i].VersionDate > lastCommit)   // or there's a date gap...
                        )
                    {
                        if (commitCount != 0)
                        {
                            // there are items to commit
                            commitDate = lastCommit.ToString(DateFormat);
                            // checkin time gap found, commit
                            SetProgressLabel(commitDate);
                            int checkinGap = Convert.ToInt32((itemList[i].VersionDate - lastCommit).TotalSeconds) + gapInterval;
                            StatusMessage("Commit {0} items ({1})\r\n", commitCount, commitDate);

                            // print a message saying why we are committing.
                            if (project.Spec != currentProjectName)
                            {
                                StatusMessage("Commit reason: Project name changed from {0} to {1}\r\n", currentProjectName, project.Spec);
                            }
                            else if (commits.Contains(itemList[i].Spec))
                            {
                                StatusMessage("Commit reason: A previous version of {0} is already staged to commit.\r\n", itemList[i].Spec);
                            }
                            else if (itemList[i].VersionDate > lastCommit)
                            {
                                if (checkinGap > gapInterval)
                                {
                                    StatusMessage("Commit reason: Checkin gap: {0}\r\n", checkinGap);
                                }
                            }

                            commitCount = 0;
                            checkinCount += 1;

                            GitCommit(currentProjectPath, commitComment, commitDate);

                            commits.Clear();
                            commitComment = "";
                        }
                        if (itemList[i].VersionDate > latestDate)
                        {
                            // hit end of list.  We are done.
                            // break out of loop
                            break;
                        }

                        if (project.Spec != currentProjectName)
                        {
                            // project name has changed.  
                            if (!project.Created)
                            {
                                currentProjectPath = project.Spec.Substring(2);

                                if (!String.IsNullOrEmpty(rootStrip))
                                {
                                    if (currentProjectPath.StartsWith(rootStrip))
                                    {
                                        currentProjectPath = currentProjectPath.Substring(rootStrip.Length);
                                    }
                                    else
                                    {
                                        StatusMessage("******************* Unable to strip root path from project path.\r\n");
                                        throw new Exception("Unable to strip project root path from version " + project.VersionNumber.ToString() + " of " + project.Spec + ".");
                                    }
                                }

                                string gitRepoName = MakeGitRepoName(currentProjectPath, project.Name);
                                gitProjectList.Add(gitRepoName);
                                /// *******
                                if (!String.IsNullOrEmpty(remoteRepoBasePath))
                                {
                                    currentRemoteRepoPath = remoteRepoBasePath + gitRepoName;
                                    StatusMessage("New remote repo path: {0}\r\n", currentRemoteRepoPath);
                                }
                                else
                                {
                                    currentRemoteRepoPath = gitRepoName;
                                }
                                if (!String.IsNullOrEmpty(remoteRepoBaseUrl))
                                {
                                    currentRemoteRepoUrl = remoteRepoBaseUrl + gitRepoName;
                                    StatusMessage("New remote repo url: {0}\r\n", currentRemoteRepoUrl);
                                }
                                else
                                {
                                    currentRemoteRepoUrl = gitRepoName;
                                }

                                projectXref.Add(String.Format("{0} -> {1}", project.Spec, gitRepoName));

                                // strip the $/ from the start of the item spec, and replace / with \
                                currentProjectPath = Path.Combine(extractPath, currentProjectPath.Replace('/', '\\'));

                                CheckPath(currentProjectPath);

                                // run git init 
                                GitInit(currentProjectPath);

                                cleanup.Add(new ProjectInfo(currentProjectPath, gitRepoName, currentRemoteRepoUrl, currentRemoteRepoPath));

                                project.ProjectPath = currentProjectPath;

                                project.Created = true;
                            }
                            else
                            {
                                currentProjectPath = project.ProjectPath;
                            }
                            currentProjectName = project.Spec;
                        }
                    }

                    // set the last commit to "checkinGap" seconds after the current version date.
                    // this becomes the new threshold for the commit check.
                    lastCommit = itemList[i].VersionDate.AddSeconds(gapInterval);

                    // add this item to the list of things that need to be committed
                    commits.Add(itemList[i].Spec);

                    // strip control characters from comment
                    itemComment = FixComment(itemList[i].Comment);

                    if (!String.IsNullOrEmpty(itemComment))
                    {
                        commitComment = itemComment;
                    }

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
                            throw new Exception("Unable to strip root path from version " + itemList[i].VersionNumber.ToString() + " of " + itemList[i].Spec + ".");
                        }
                    }

                    // strip the $/ from the start of the item spec, and replace / with \
                    itemName = itemName.Replace('/', '\\');

                    StatusMessage("{0} (ver {1} - {2} {3} {4})\r\n", itemList[i].Spec, itemList[i].VersionNumber, itemList[i].VersionDate, itemList[i].Action ?? "", itemComment);

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
                        // labels are stored in VSS as file items with the name of the directory.
                        // this check filters them out.
                        isDirectory = Directory.Exists(fileName);
                    }

                    if (!isDirectory)
                    {
                        // extract the file.
                        // Enclose this in a try/catch block in case the extract fails.
                        // I've seen this fail if older versions of the file are missing in VSS.
                        if (!testMode)
                        {
                            // delete any existing version of the file
                            DeleteFile(getName);
                        }

                        extracted = false;

                        // don't extract if it's a visual studio source control file
                        if (!fileName.EndsWith(".vssscc", StringComparison.OrdinalIgnoreCase) &&
                            !fileName.EndsWith(".vspscc", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                if (!testMode)
                                {
                                    itemList[i].VSSItem.Get(ref getName, (int)VSSFlags.VSSFLAG_CMPFAIL);
                                }
                                extracted = true;
                            }
                            catch (Exception ex)
                            {
                                extracted = false;
                                StatusMessage("{0} - {1}\r\n", ex.GetType().Name, ex.Message);
                            }
                        }

                        if (extracted)
                        {
                            if (!testMode)
                            {
                                // set the file date to the checkin time.
                                File.SetAttributes(fileName, FileAttributes.Normal);
                                File.SetCreationTime(fileName, itemList[i].VersionDate);
                                File.SetLastWriteTime(fileName, itemList[i].VersionDate);
                            }

                            // add changed file
                            // Note: This git add is not needed, but can be uncommented if you want.
                            // The program will do a "git add -A" before the commit.
                            //GitAdd(currentProjectPath, fileName, itemList[i].Comment ?? "");
                            commitCount += 1;
                        }
                    }
                }
            }

            cleanup.Sort(new ProjectInfoComparer());

            if (autoPush && !String.IsNullOrEmpty(txtRemoteUrl.Text))
            {
                SetStatusLabel("Cleanup and push to remote");
            }
            else
            {
                SetStatusLabel("Cleanup");
            }

            foreach (ProjectInfo pi in cleanup)
            {
                SetProgressLabel(pi.Path);

                ChangeDirectory(pi.Path);

                RunCommand("git gc");

                if (!String.IsNullOrEmpty(pi.RemoteRepoPath))
                {
                    createRemoteRepo.AppendFormat("git init --bare \"{0}\"\r\n", pi.RemoteRepoPath);
                }

                if (!String.IsNullOrEmpty(pi.RemoteRepoUrl))
                {
                    string gitRemoteAddOrigin = String.Format("git remote add origin \"{0}\"", pi.RemoteRepoUrl);
                    string gitPush = "git push -u origin master";
                    if (autoPush)
                    {
                        RunCommand(gitRemoteAddOrigin);
                        RunCommand(gitPush);
                    }
                    else
                    {
                        pushRemoteRepo.AppendFormat("cd \"{0}\"\r\n", pi.Path);
                        pushRemoteRepo.AppendFormat("{0}\r\n", gitRemoteAddOrigin);
                        pushRemoteRepo.AppendFormat("{0}\r\n", gitPush);
                    }
                }
                RunCommand("git status");

            }

            if (curDir != null)
            {
                Directory.SetCurrentDirectory(curDir);
            }

            if (gitProjectList.Count > 0)
            {
                gitProjectList.Sort();

                TextForm gpl = new TextForm(gitProjectList, "Suggested list of remote project names");
                gpl.Show();
            }

            if (createRemoteRepo.Length > 0)
            {
                TextForm cr = new TextForm(createRemoteRepo.ToString(), "Commands to create remote repo(s)");
                cr.Show();
            }

            if (pushRemoteRepo.Length > 0)
            {
                TextForm pr = new TextForm(pushRemoteRepo.ToString(), "Commands to push to remote repos");
                pr.Show();
            }

            if (projectXref.Count > 0)
            {
                projectXref.Sort();

                TextForm xr = new TextForm(projectXref, "VSS project -> Git project");
                xr.Show();
            }

            StatusMessage("Total checkins: {0}\r\n", checkinCount);
            SetStatusLabel("Done.");
        }

        public string GetResource(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string resName = asm.GetName().Name + "." + resourceName;
            using (Stream s = asm.GetManifestResourceStream(resName))
            {
                if (s != null)
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }

            return "";
        }


        private void DumpVssDatabase()
        {
            Cursor oldCursor = this.Cursor;

            try
            {
                rootPath = txtSSRoot.Text;

                SaveConfig(configFileName);

                gitIgnore = GetResource(".gitignore");

                sbStatus = new StringBuilder();
                lblStatus.Text = "";
                lblProgress.Text = "";
                this.Cursor = Cursors.WaitCursor;

                // get various settings from the UI
                extractPath = txtExtractPath.Text.Trim();
                if (!String.IsNullOrEmpty(txtRemoteUrl.Text.Trim()))
                {
                    remoteRepoBaseUrl = txtRemoteUrl.Text.Trim();
                    if (!remoteRepoBaseUrl.EndsWith("/"))
                    {
                        remoteRepoBaseUrl += "/";
                    }
                }
                else
                {
                    remoteRepoBaseUrl = "";
                }
                if (!String.IsNullOrEmpty(txtRemotePath.Text.Trim()))
                {
                    remoteRepoBasePath = txtRemotePath.Text.Trim();
                    if (!remoteRepoBasePath.EndsWith("/"))
                    {
                        remoteRepoBasePath += "/";
                    }
                }
                else
                {
                    remoteRepoBasePath = "";
                }
                logFile = txtLog.Text.Trim();
                testMode = chkTest.Checked;
                earliestDate = DateTime.MaxValue;
                latestDate = DateTime.MinValue;
                gapInterval = 10;
                itemList = new List<ItemInfo>();
                projectList = new List<ItemInfo>();
                lastCheckPath = "";
                lastChangeDir = "";
                flatten = chkFlatten.Checked;
                autoPush = chkAutoPush.Checked;
                projectNames = new List<string>();

                // Make sure the base path exists,
                CheckPath(extractPath);

                // Make sure base path is empty
                if (Directory.GetFiles(extractPath).Length != 0 || Directory.GetDirectories(extractPath).Length != 0)
                {
                    if (MessageBox.Show("Extract path " + extractPath + " is not empty.  Delete it?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                    Directory.Delete(extractPath, true);
                    lastCheckPath = "";
                    CheckPath(extractPath);
                }

                // Create database instance...
                IVSSDatabase vssDatabase = new VSSDatabase();

                // and open it
                vssDatabase.Open(txtVss.Text, txtSSUser.Text, txtSSPassword.Text);

                // Get the root item
                IVSSItem vssRoot = vssDatabase.get_VSSItem(rootPath, false);

                SetStatusLabel("Scanning VSS database");

                // Process the root item.  This will also recurse through everything else.
                DumpSubitems(vssRoot, null);

                // add a dummy record that will sort to the end
                ItemInfo item = new ItemInfo();
                item.VersionDate = latestDate.AddSeconds(gapInterval + 1);
                item.Spec = "~";
                item.VersionNumber = int.MaxValue;
                item.ItemType = 1;
                itemList.Add(item);
                item.Project = new ItemInfo(vssRoot, vssRoot.VSSVersion);

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
            try
            {
                DumpVssDatabase();
            }
            catch (Exception ex)
            {
                StatusMessage("{0}\r\n", ExceptionHandler.UnwindExceptions(ex));
                MessageBox.Show(ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
