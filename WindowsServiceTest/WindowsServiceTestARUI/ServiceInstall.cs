using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Configuration.Install;
using System.IO;

namespace WindowsServiceTestARUI
{
    class ServiceInstall
    {/// <summary>
     /// 安装.Net服务，返回是否安装成功
     /// </summary>
     /// <param name="svcPath">服务程序目标路径</param>
     /// <param name="logToConsole">是否将安装日志回显至控制台</param>
     /// <returns>是否安装成功</returns>
        public static bool InstallService(string svcPath, bool logToConsole)
        {
            var title = "Install " + Path.GetFileNameWithoutExtension(svcPath);
            var logtoConsole = "/LogtoConsole=" + logToConsole.ToString().ToLower();
            var assInstall = new AssemblyInstaller(svcPath, new string[] { logtoConsole });
            var installState = new Hashtable();
            try
            {
                assInstall.Install(installState);
                assInstall.Commit(installState);
                //Trace.Write(Trace.LogType.Installation, title, "Successful.");
                return true;
            }
            catch (Exception exp)
            {
                //Trace.Write(Trace.LogType.Installation, title, exp);
                try
                {
                    assInstall.Rollback(installState);
                }
                catch
                {
                }
                return false;
            }
            finally
            {
                assInstall.Dispose();
            }
        }

        /// <summary>
        /// 卸载.Net服务，返回是否卸载成功
        /// </summary>
        /// <param name="svcPath">服务程序目标路径</param>
        /// <param name="logToConsole">是否将卸载日志回显至控制台</param>
        /// <returns>是否卸载成功</returns>
        public static bool UninstallService(string svcPath, bool logToConsole)
        {
            var logtoConsole = "/LogtoConsole=" + logToConsole.ToString().ToLower();
            var assInstall = new AssemblyInstaller(svcPath, new string[] { logtoConsole });
            var installState = new Hashtable();
            try
            {
                assInstall.Uninstall(installState);
                assInstall.Commit(installState);
                return true;
            }
            catch (Exception exp)
            {
                //Trace.Write(Trace.LogType.Installation, "Uninstall " + Path.GetFileNameWithoutExtension(svcPath), exp);
                return false;
            }
            finally
            {
                assInstall.Dispose();
            }
        }

        #region API 方式安装、卸载

        #region API 函数
        [DllImport("advapi32.dll")]
        internal static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, int scParameter);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);

        [DllImport("advapi32.dll")]
        internal static extern bool DeleteService(IntPtr SVHANDLE);

        [DllImport("advapi32.dll")]
        internal static extern void CloseServiceHandle(IntPtr SCHANDLE);
        #endregion

        /// <summary>
        /// Win32 API卸载服务
        /// </summary>
        /// <param name="svcName">服务名称</param>
        public static bool Win32_UninstallService(string svcName)
        {
            int GENERIC_WRITE = 0x40000000, DELETE = 0x10000;
            IntPtr sc_hnd = IntPtr.Zero, svc_hnd = IntPtr.Zero;
            try
            {
                sc_hnd = OpenSCManager(null, null, GENERIC_WRITE);
                if (sc_hnd == IntPtr.Zero)
                {
                    //TraceWin32Error("Uninstall " + svcName, "OpenSCManager");
                    return false;
                }
                svc_hnd = OpenService(sc_hnd, svcName, DELETE);
                if (svc_hnd == IntPtr.Zero)
                {
                    //TraceWin32Error("Uninstall " + svcName, "OpenService");
                    return false;
                }
                return DeleteService(svc_hnd);
            }
            finally
            {
                if (svc_hnd != IntPtr.Zero) CloseServiceHandle(svc_hnd);
                if (sc_hnd != IntPtr.Zero) CloseServiceHandle(sc_hnd);
            }
        }
        #endregion 
    }
}
