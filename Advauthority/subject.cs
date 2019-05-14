using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace Advauthority
{
    public partial class subject : Form
    {
        private string[] args = new string[1];
        public subject(string[] args)
        {
            InitializeComponent();
            this.args = args;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFrom();
            if ("Authority" == args[0])
            {
                Authority();
            }
            else if ("AddStart" == args[0])
            {
                AddStart();
            }
            else if ("DelStart" == args[0])
            {
                DelStart();
            }
            exit();
        }
        public void LoadFrom()
        {
            this.Width = 0;
            this.Top = 0;
            this.Opacity = 0;
            this.Hide();
        }
        private void Authority()//设置APP的目录权限
        {
            string dirPath = Environment.CurrentDirectory;
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            DirectorySecurity dirSecurity = dir.GetAccessControl(AccessControlSections.All);
            InheritanceFlags inherits = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            FileSystemAccessRule everyoneFileSystemAccessRule = new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            FileSystemAccessRule usersFileSystemAccessRule = new FileSystemAccessRule("Users", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            bool isModified = false;
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, everyoneFileSystemAccessRule, out isModified);
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, usersFileSystemAccessRule, out isModified);
            dir.SetAccessControl(dirSecurity);
            Start();
            exit();
        }
        private void Start()//启动主程序
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = "ForceTer.exe";
            p.StartInfo.Arguments = "";
            p.Start();
        }
        private void AddStart()//添加启动
        {
            string dirPath = Environment.CurrentDirectory + "\\ForceTer.lnk";
            RegistryKey reg = Registry.LocalMachine;
            RegistryKey run = reg.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if (run.GetValue("ForceTer") == null)
            {
                run.SetValue("ForceTer", dirPath);
                reg.Close();
            }
            mkdir();
        }
        private void DelStart()//删除启动
        {
            RegistryKey reg = Registry.LocalMachine;
            RegistryKey run = reg.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            if (!(run.GetValue("ForceTer") == null))
            {
                run.DeleteValue("ForceTer");
                reg.Close();
            }
            rmdir();
        }
        private void rmdir()//删除文件
        {
            if (File.Exists("ForceTer.lnk")) File.Delete("ForceTer.lnk");
            while (true)
            {
                if (!File.Exists("ForceTer.lnk")) break;
            }
            exit();
        }
        private void mkdir()//添加文件
        {
            if (!File.Exists("ForceTer.lnk"))
            {
                var shellType = Type.GetTypeFromProgID("WScript.Shell");
                dynamic shell = Activator.CreateInstance(shellType);
                var shortcut = shell.CreateShortcut("ForceTer.lnk");
                shortcut.TargetPath = Environment.CurrentDirectory + "\\ForceTer.exe";
                shortcut.Arguments = "";
                shortcut.WorkingDirectory = Environment.CurrentDirectory;
                shortcut.Save();
            }
            while (true)
            {
                if (File.Exists("ForceTer.lnk")) break;
            }
            exit();
        }
        private void exit()
        {
            this.Dispose();
            this.Close();
        }
    }
}