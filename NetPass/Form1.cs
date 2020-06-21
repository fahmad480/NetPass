using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetPass
{
    public partial class Form1 : Form
    {
        private bool isMinimized = false;

        public Form1()
        {
            InitializeComponent();
        }

        public IntPtr FindWindow(string titleName)
        {
            unsafe
            {
                Process[] processes = Process.GetProcesses(".");
                for (int i = 0; i < (int)processes.Length; i++)
                {
                    Process p = processes[i];
                    if (p.MainWindowTitle.ToUpper().Contains(titleName.ToUpper()))
                    {
                        return p.MainWindowHandle;
                    }
                }
                return new IntPtr();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if(btnStart.Text == "STOP")
            {
                MessageBox.Show("Stop terlebih dahulu sebelum keluar dari aplikasi", "Stop NetPass");
            } else
            {
                if (MessageBox.Show("Yakin ingin keluar?", "Keluar NetPass", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "START")
            {
                Process process = new Process();
                ProcessStartInfo info = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    Verb = "runas"
                };
                process.StartInfo = info;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                using (StreamWriter sw = process.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine("@ECHO OFF");
                        sw.WriteLine("PUSHD '%~dp0'".Replace("'", "\""));
                        sw.WriteLine("set _arch=x86".Replace("'", "\""));
                        sw.WriteLine("IF '%PROCESSOR_ARCHITECTURE%'=='AMD64' (set _arch=x86_64)".Replace("'", "\""));
                        sw.WriteLine("IF DEFINED PROCESSOR_ARCHITEW6432 (set _arch=x86_64)".Replace("'", "\""));
                        sw.WriteLine("PUSHD '%_arch%'".Replace("'", "\""));
                        sw.WriteLine("".Replace("'", "\""));
                        sw.WriteLine("@ECHO OFF");
                        sw.WriteLine("start /min 'goodbyedpii' 'resources/x86_64/goodbyedpi.exe' -1 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253".Replace("'", "\""));
                        sw.WriteLine("".Replace("'", "\""));
                        sw.WriteLine("POPD".Replace("'", "\""));
                        sw.WriteLine("POPD".Replace("'", "\""));
                    }
                }
                btnStart.Text = "STOP";
                timer1.Enabled = true;
                MessageBox.Show("Tunggu 30 detik sebelum mengkases netflix/situs yang diblokir lainnya", "NetPass");
            } else if(btnStart.Text == "STOP")
            {
                Process process = new Process();
                ProcessStartInfo info = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Verb = "runas"
                };
                process.StartInfo = info;
                process.Start();
                using (StreamWriter sw = process.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine("@ECHO OFF");
                        sw.WriteLine("echo This script should be run with administrator privileges.".Replace("'", "\""));
                        sw.WriteLine("echo Right click - run as administrator.".Replace("'", "\""));
                        sw.WriteLine("echo Press any key if you're running it as administrator.".Replace("'", "\""));
                        sw.WriteLine("pause".Replace("'", "\""));
                        sw.WriteLine("sc stop 'GoodbyeDPI'".Replace("'", "\""));
                        sw.WriteLine("sc delete 'GoodbyeDPI'".Replace("'", "\""));
                        sw.WriteLine("sc stop 'WinDivert1.4'".Replace("'", "\""));
                        sw.WriteLine("sc delete 'WinDivert1.4'".Replace("'", "\""));
                    }
                }
                Process[] processesByName = Process.GetProcessesByName("goodbyedpi");
                for (int i = 0; i < (int)processesByName.Length; i++)
                {
                    processesByName[i].Kill();
                }
                btnStart.Text = "START";
                timer1.Enabled = false;
                this.isMinimized = false;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Process.Start("https://farzain.com/donate.php");
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.isMinimized)
            {
                timer1.Enabled = false;
            }
            else
            {
                IntPtr hWnd = this.FindWindow("goodbyedpii");
                if (!hWnd.Equals(IntPtr.Zero))
                {
                    this.isMinimized = true;
                    Form1.ShowWindow(hWnd, 0);
                    return;
                }
            }
        }
    }
}
