using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.SourceSafe.Interop;

namespace VSS2Git
{
    public class ItemInfo
    {
        public ItemInfo(IVSSItem vssItem, IVSSVersion vssVersion)
        {
            IVSSItem ver = vssVersion.VSSItem.get_Version(vssVersion.VersionNumber);
            ItemType = vssItem.Type;
            Parent = ver.Parent.Physical; 
            Physical = ver.Physical;
            Spec = ver.Spec;
            VersionNumber = ver.VersionNumber;
            VersionDate = vssVersion.Date;
            Action = vssVersion.Action;
            Comment = vssVersion.Comment;
            Name = ver.Name;
            LocalSpec = ver.LocalSpec;
            Label = vssVersion.Label;
            LabelComment = vssVersion.LabelComment;
            UserName = vssVersion.Username;
            VSSItem = ver;
            FileCount = 0;
            Project = null;
            ProjectPath = "";
            Created = false;
        }

        public ItemInfo()
        {
            ItemType = 1;
            Parent = "";
            Physical = "";
            Spec = "";
            VersionNumber = int.MaxValue;
            VersionDate = DateTime.MaxValue;
            Action = "";
            Comment = "";
            Name = "";
            LocalSpec = "";
            Label = "";
            LabelComment = "";
            UserName = "";
            VSSItem = null;
            FileCount = 0;
            Project = null;
            ProjectPath = "";
            Created = false;
        }

        public ItemInfo FindProject()
        {
            ItemInfo cur = this.Project;
            ItemInfo result = null;

            while (cur != null)
            {
                if (cur.FileCount != 0)
                {
                    result = cur;
                }

                cur = cur.Project;
            }

            if (result == null)
            {
                return this.Project;
            }

            return result;
        }

        public string FindProjectName()
        {
            ItemInfo project = this.FindProject();
            if (project != null)
            {
                return project.Spec;
            }
            return "";
        }

        public int ItemType { get; set; }
        public string Parent { get; set; }
        public string Physical { get; set; }
        public string Spec { get; set; }
        public int VersionNumber { get; set; }
        public DateTime VersionDate { get; set; }
        public string Action { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
        public string LocalSpec { get; set; }
        public string Label { get; set; }
        public string LabelComment { get; set; }
        public string UserName { get; set; }
        public IVSSItem VSSItem { get; private set; }

        public int FileCount { get; set; }

        public ItemInfo Project { get; set; }
        public string ProjectPath { get; set; }

        public bool Created { get; set; }
    }

    public class ItemInfoComparer : IComparer<ItemInfo>
    {
        // sort order: type, date, file name, version
        public int Compare(ItemInfo i1, ItemInfo i2)
        {
            int result;

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

            //result = i1.ItemType.CompareTo(i2.ItemType);
            //if (result != 0)
            //{
            //    return result;
            //}

            result = i1.VersionDate.CompareTo(i2.VersionDate);
            if (result != 0)
            {
                return result;
            }

            result = String.Compare(i1.Spec, i2.Spec);
            if (result != 0)
            {
                return result;
            }

            return i1.VersionNumber.CompareTo(i2.VersionNumber);
        }

    }
}
