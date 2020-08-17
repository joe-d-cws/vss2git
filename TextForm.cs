using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSS2Git
{
    public partial class TextForm : Form
    {
        public TextForm()
        {
            InitializeComponent();
        }

        public TextForm(string text)
            : this(text, "")
        {
        }

        public TextForm(IEnumerable text)
            : this(text, "")
        {
        }


        public TextForm(IEnumerable text, string title)
            : this()
        {
            StringBuilder sb = new StringBuilder();
            foreach (object line in text)
            {
                sb.AppendLine(line.ToString());
            }
            SetText(sb.ToString(), title);
        }
        
        public TextForm(string text, string title)
            : this()
        {
            SetText(text, title);
        }

        private void SetText(string text, string title)
        {
            this.Text = title;
            txtInfo.Text = text;
            txtInfo.SelectionStart = 0;
            txtInfo.SelectionLength = 0;
        }

        private void InitFileDialog(FileDialog fd, string defaultName, string title, params string[] extension)
        {
            if (extension.GetLength(0) == 0)
            {
                fd.Filter = "All files (*.*)|*.*";
            }
            else
            {
                fd.DefaultExt = extension[0];
                string filter = "";
                foreach (string ext in extension)
                {
                    if (!String.IsNullOrEmpty(ext))
                    {
                        if (ext.IndexOf('|') >= 0)
                        {
                            filter += ext + "|";
                        }
                        else
                        {
                            filter += String.Format("{0} files (*.{0})|*.{0}|", ext);
                        }
                    }
                }
                filter += "All files (*.*)|*.*";
                fd.Filter = filter;
            }
            fd.FilterIndex = 1;
            fd.RestoreDirectory = true;
            fd.Title = title;
            fd.FileName = defaultName;
        }

        private string GetSaveFileName(string defaultName, string title, params string[] extension)
        {
            SaveFileDialog sfd;

            sfd = new SaveFileDialog();
            InitFileDialog(sfd, defaultName, title, extension);
            sfd.CheckPathExists = true;
            sfd.OverwritePrompt = true;
            sfd.SupportMultiDottedExtensions = true;

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return null;
            }
            return sfd.FileName;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtInfo.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionHandler.UnwindExceptions(ex));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = GetSaveFileName("", "Save file...", "txt");
                if (!String.IsNullOrEmpty(fileName))
                {
                    File.WriteAllText(fileName, txtInfo.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionHandler.UnwindExceptions(ex));
            }
        }


    }
}
