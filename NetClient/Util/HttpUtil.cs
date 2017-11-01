using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Util
{
    class HttpUtil
    {
        CookieContainer cookie = new CookieContainer();

        public string HttpPost(string Url, string postDataStr)
        {
            string retString = string.Empty;
            try
            {
                //Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                //byte[] postDataByte = encode.GetBytes(postDataStr);
                byte[] postDataByte = System.Text.Encoding.Default.GetBytes(postDataStr);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "multi-part form data";
                request.ContentLength = postDataByte.Length;                
                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(postDataByte, 0, postDataByte.Length);
                myRequestStream.Close();
                
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream myResponseStream = response.GetResponseStream();
                //StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.Default);
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                //JObject jo = JObject.Parse(retString);
                //string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();

                return retString;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return "";
            }
            
        }

        public string HttpPostUTF8(string Url, string postDataStr)
        {
            string retString = string.Empty;
            try
            {
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                byte[] postDataByte = encode.GetBytes(postDataStr);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postDataByte.Length;
                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(postDataByte, 0, postDataByte.Length);
                myRequestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                
                return retString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }

        }

        public string HttpGet(string Url, string postDataStr)
        {
            string retString = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                //StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return "";
            }

        }

        public string HttpGet(string Url)
        {
            string retString = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.KeepAlive = true;
                request.Accept = "zh-CN";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0; SE 2.X MetaSr 1.0) like Gecko";
                request.Referer = "http://www.zxxk.com/soft/6749647.html";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                //StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }

        }
    }
}
