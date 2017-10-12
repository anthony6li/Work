using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Management;

namespace WindowsServiceTest
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceProcessInstaller1_Committed(object sender, InstallEventArgs e)
        {
            try
            {
                var service = new System.Management.ManagementObject(
                String.Format("WIN32_Service.Name='{0}'", "ServiceTestAR"));
                try
                {
                    var paramList = new object[11];
                    paramList[5] = true;//We only need to set DesktopInteract parameter
                    var output = service.InvokeMethod("Change", paramList);
                    //if zero is returned then it means change is done.
                    logger(string.Format("FAILED with code {0}", output));

                    //throw new Exception(string.Format("FAILED with code {0}", output));

                }
                catch (Exception ee)
                {
                    logger(string.Format("Failed： {0}", ee.ToString()));
                }
                finally
                {
                    service.Dispose();
                }
                //////////////////////////////////////////////////////////////////////////
                //ConnectionOptions myConOptions = new ConnectionOptions();
                //myConOptions.Impersonation = ImpersonationLevel.Impersonate;
                //ManagementScope mgmtScope = new ManagementScope(@"root\CIMV2",myConOptions);

                //mgmtScope.Connect();
                //ManagementObject wmiService = new ManagementObject("Win32_Service.Name=" + serviceInstaller1.ServiceName+"");

                //ManagementBaseObject InParam = wmiService.GetMethodParameters("Change");

                //InParam["DesktopInteract"] = true;

                //ManagementBaseObject OutParam = wmiService.InvokeMethod("Change", InParam, null);
            }
            catch (System.Exception ex)
            {
                logger(ex.ToString());
            }
        }

        private void logger(string message)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("x:\\log.txt", true))
            {
                sw.WriteLine(message);
            }
        }
    }
}
