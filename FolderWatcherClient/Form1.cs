using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderWatcherClient
{
    public partial class Form1 : Form
    {
        public string FOLDER_PATH { get; set; }
        public FileSystemWatcher Watcher = null;

        public Form1()
        {
            InitializeComponent();

            tbxFolderPath.Text = FOLDER_PATH = @"C:\Temp";
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbxFolderPath.Text = FOLDER_PATH = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (Watcher == null)
            {
                Watcher = new FileSystemWatcher();
                Watcher.IncludeSubdirectories = true;
                Watcher.Path = FOLDER_PATH;
                Watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
                Watcher.Filter = "*.*";
                Watcher.Created += new FileSystemEventHandler(OnCreated);
                Watcher.Renamed += new RenamedEventHandler(OnRenamed);
                Watcher.Deleted += new FileSystemEventHandler(OnDeleted);
                //Watcher.Changed += new FileSystemEventHandler(OnChanged);
                Watcher.EnableRaisingEvents = true;

                btnStart.Enabled = false;

                tbxMessages.AppendText("Watching " + FOLDER_PATH + ".");
            }
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            AppendMessage(Environment.NewLine + "CREATE: " + e.Name);
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            AppendMessage(Environment.NewLine + "RENAME: " + e.Name);
        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            AppendMessage(Environment.NewLine + "DELETE: " + e.Name);
        }

        //private void OnChanged(object source, FileSystemEventArgs e)
        //{
        //    AppendMessage(Environment.NewLine + "CHANGE: " + e.Name);
        //}

        delegate void AppendMessageDelegate(string message);

        private void AppendMessage(string message)
        {
            if (this.tbxMessages.InvokeRequired)
            {
                AppendMessageDelegate d = new AppendMessageDelegate(AppendMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                this.tbxMessages.AppendText(message);
            }
        }
    }
}
