using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PysideAutoConvert
{
    

    public partial class Form1 : Form
    {
        public static string _pysidePath { get; private set; }
        public static string _watchPath { get; private set; }
        public Form1()
        {
            InitializeComponent();
            _pysidePath = textBox1.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CreateFileWatcher(_watchPath);
            this.WindowState = FormWindowState.Minimized;
        }

        public void CreateFileWatcher(string path)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.ui";

            // Add event handlers.
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Changed += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }


        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {

            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath);

            
            Process cmd = new Process();
            cmd.StartInfo.FileName = _pysidePath;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = e.FullPath + " -o " + Path.GetDirectoryName(e.FullPath) + "\\" + Path.GetFileNameWithoutExtension(e.FullPath) + ".py";
            cmd.Start();
            
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();          
            // Show the FolderBrowserDialog.  
            DialogResult result = fileDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                _pysidePath = fileDlg.FileName;
                textBox1.Text = _pysidePath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new　FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = false;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                _watchPath = folderDlg.SelectedPath;
                textBox2.Text = _watchPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }
    }
}
