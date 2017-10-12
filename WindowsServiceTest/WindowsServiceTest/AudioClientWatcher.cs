using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Cjwdev.WindowsApi;
using System.Runtime.InteropServices;
using ExamService;
using Microsoft.Win32;
using System.IO;

namespace WindowsServiceTest
{
    public partial class AudioClientWatcher : ServiceBase
    {
        //public string exePath = @"X:\Users\enjoy\Desktop\AR\AudioClient\AudioClient\bin\Debug\AudioClient.exe";
        public const string EXENAME = "AudioClient.exe";
        public const string SERVICENAME = "AudioClientWatcher";
        System.Timers.Timer timer;

        public AudioClientWatcher()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            logger(true);
            timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
            runProgram();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            logger(string.Format("Service tick {0}.", DateTime.Now.ToString()));
            runProgram();
        }

        protected override void OnPause()
        {
            timer.Stop();
            base.OnPause();
        }

        protected override void OnContinue()
        {
            timer.Start();
            base.OnContinue();
        }

        protected override void OnStop()
        {
            timer.Close();
            logger(false);
        }

        private void runProgram()
        {
            try
            {
                string exePath = serviceInstallPath(SERVICENAME) +"\\" + EXENAME;
                //if (exePath.IndexOf("\\") > -1)
                {
                    var cmd = exePath.Split(new[] { '\\' });
                    string exeName = cmd[cmd.Length - 1];
                    if (!IsExistProcess(EXENAME.Split(new char[] { '.'})[0]))
                    {
                        logger(string.Format("Process[{0}] is starting.", exeName));
                        //ProcessStartInfo proInfo = new ProcessStartInfo(exePath);
                        //proInfo.WindowStyle = ProcessWindowStyle.Normal;
                        //proInfo.CreateNoWindow = true;
                        //Process.Start(proInfo);
                        //////////////////////////////////////////////////////////////////////////
                        AppStart(exePath);
                        //////////////////////////////////////////////////////////////////////////
                        //Interop.CreateProcess(exeName, exePath.Replace(exeName, ""));
                        //Interop.CreateProcess("cmd.exe", @"C:\Windows\System32\");
                        //////////////////////////////////////////////////////////////////////////
                        //UserProcess.PROCESS_INFORMATION processInfo;
                        //UserProcess up = new UserProcess();
                        //if (!up.StartProcessAndBypassUAC(exeName,"",out processInfo))
                        //{
                        //    logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                        //    logger("启动程序 " + exeName + " 失败");
                        //}
                    }
                }
            }
            catch (System.Exception ex)
            {
                logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                logger("启动程序失败."+ex.ToString());
            }
        }

        private bool IsExistProcess(string processName)
        {
            Process[] MyProcesses = Process.GetProcesses();
            foreach (Process MyProcess in MyProcesses)
            {
                if (MyProcess.ProcessName.CompareTo(processName) == 0)
                {
                    logger(string.Format("Process[{0}] is exist.",processName));
                    return true;
                }
            }
            return false;
        }

        private void logger(string message)
        {
            string logPath = serviceInstallPath(SERVICENAME) + "\\log.txt";
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(logPath, true))
            {
                sw.WriteLine(message);
            }
        }

        private void logger(bool ifTrue)
        {
            string logPath = serviceInstallPath(SERVICENAME) + "\\log.txt";
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("logPath", true))
            {
                if (ifTrue)
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  Start.");
                }
                else
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  Stop.");
                }
            }
        }

        public void AppStart(string appPath)
        {
            try
            {

                string appStartPath = appPath;
                IntPtr userTokenHandle = IntPtr.Zero;
                ApiDefinitions.WTSQueryUserToken(ApiDefinitions.WTSGetActiveConsoleSessionId(), ref userTokenHandle);

                ApiDefinitions.PROCESS_INFORMATION procInfo = new ApiDefinitions.PROCESS_INFORMATION();
                ApiDefinitions.STARTUPINFO startInfo = new ApiDefinitions.STARTUPINFO();
                startInfo.cb = (uint)Marshal.SizeOf(startInfo);

                ApiDefinitions.CreateProcessAsUser(
                    userTokenHandle,
                    appStartPath,
                    "",
                    IntPtr.Zero,
                    IntPtr.Zero,
                    false,
                    0,
                    IntPtr.Zero,
                    null,
                    ref startInfo,
                    out procInfo);

                if (userTokenHandle != IntPtr.Zero)
                    ApiDefinitions.CloseHandle(userTokenHandle);

                int _currentAquariusProcessId = (int)procInfo.dwProcessId;
            }
            catch (Exception ex)
            {
                logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                logger("AppStart failed! " + ex.ToString());
            }
        }

        private string serviceInstallPath(string ServiceName)
        {
            string servicePath = string.Empty;
            string key = @"SYSTEM\CurrentControlSet\Services\" + ServiceName;
            string path = Registry.LocalMachine.OpenSubKey(key).GetValue("ImagePath").ToString();
            path = path.Replace("\"", string.Empty);//替换掉双引号
            FileInfo fi = new FileInfo(path);
            return fi.Directory.ToString();
        }
    }
}
