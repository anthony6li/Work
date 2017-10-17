using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatRoom
{
    /// <summary>
    /// 错误日志的输出。
    /// </summary>
    class LogText
    {
        private string AppPath;
        private StreamWriter StrW;
        private string FileName;
        public LogText(string FileName1)
        {
            AppPath = Application.StartupPath + @"\Log";
            try
            {
                if (Directory.Exists(AppPath) == false)
                {
                    Directory.CreateDirectory(AppPath);
                }
                if (File.Exists(AppPath + @"\" + FileName + ".log") == false)
                {
                    File.Create(AppPath + @"\" + FileName + ".log");
                }
                FileName = FileName1;
            }
            catch { }
        }

        public void LogWriter(string Text)
        {
            try
            {
                StrW = new StreamWriter(AppPath + @"\" + FileName + ".log", true);
                StrW.WriteLine("时间：{0} 描述：{1} \r\n", DateTime.Now.ToString(), Text);
                StrW.Flush();
                StrW.Close();
            }
            catch { }
        }
    }
}

