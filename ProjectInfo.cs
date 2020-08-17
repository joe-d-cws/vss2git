using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSS2Git
{
    public class ProjectInfo
    {
        public string Path { get; set; }

        public string Repo { get; set; }

        public string RemoteRepoPath { get; set; }

        public string RemoteRepoUrl { get; set; }

        public ProjectInfo(string path, string repo, string remoteRepoUrl, string remoteRepoPath)
        {
            Path = path;
            Repo = repo;
            RemoteRepoPath = remoteRepoPath;
            RemoteRepoUrl = remoteRepoUrl;
        }
    }

    public class ProjectInfoComparer : IComparer<ProjectInfo>
    {
        // sort order: path
        public int Compare(ProjectInfo i1, ProjectInfo i2)
        {
            if (i1 == null)
            {
                if (i2 == null)
                {
                    return 0;
                }
                return -1;
            }
            if (i2 == null)
            {
                return 1;
            }

            return String.Compare(i1.Path, i2.Path);
        }

    }

}
