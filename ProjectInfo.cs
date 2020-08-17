using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSS2Git
{
    class ProjectInfo
    {
        public string Path { get; set; }

        public string Repo { get; set; }

        public ProjectInfo(string path, string repo)
        {
            Path = path;
            Repo = repo;
        }
    }


}
