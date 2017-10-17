using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace JsonACK
{
    class Listener
    {
        static TcpListener myListener;
        string strACK = "{\"HedaACK\":{\"Body\":{\"retCode\":\"0\",\"retMsg\":\"CMS 1.0.0.1   \"},\"Header\":{\"MessageType\":\"MSG_SC_SYSTEM_IFNO_ACK\",\"Version\":\"1.0\"}}} \r\n";
        public Listener()
        {
            method1 me = new method1();
            me.method = "version";
            string ttemp = JsonConvert.SerializeObject(me);

            var ttt = JsonConvert.DeserializeObject<method1>(ttemp);


            Thread th = null;
            myListener = new TcpListener(IPAddress.Any, 9905) { ExclusiveAddressUse = false };
            myListener.Start(200);
            th = new Thread(new ThreadStart(StartListen));
            th.Start();
        }
        public void StartListen()
        {
            while (true)
            {
                try
                {
                    // TcpClient tc = myListener.AcceptTcpClient();
                    //byte[] buffer = new byte[tc.ReceiveBufferSize];
                    //NetworkStream stream = tc.GetStream();//获取网络流
                    //stream.Read(buffer, 0, buffer.Length);//读取网络流中的数据
                    //BinaryReader reader = new BinaryReader(stream);
                    //BinaryWriter writer = new BinaryWriter(stream);
                    //string receiveString = Encoding.ASCII.GetString(buffer);//转换成字符串

                    //if (receiveString.Substring(0, 3) == "GET")
                    //{
                    //    sendResponse(writer);
                    //    sendResponse(receiveString,tc.Client);
                    //}
                    //stream.Close();//关闭流 
                    //tc.Close();//关闭Client

                    Socket mySocket = myListener.AcceptSocket();
                    mySocket.NoDelay = true;
                    mySocket.ReceiveBufferSize = 8192;
                    mySocket.ReceiveTimeout = 1500;
                    byte[] bReceive = new byte[8900];
                    byte[] bSend = new byte[8900];
                    Thread.Sleep(200);
                    mySocket.Receive(bReceive,SocketFlags.None);
                    string sBuffer = Encoding.ASCII.GetString(bReceive);
                    string[] temps = sBuffer.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string temp = temps[temps.Length - 1];
                    temp = temp.Substring(0, temp.IndexOf('}') + 1);
                    //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://10.10.1.221:9905");
                    //WebResponse webResponse = webRequest.GetResponse();

                    method1 met = JsonConvert.DeserializeObject<method1>(temp);
                    if (met == null)
                    {
                        continue;
                    }
                    byte[] bSend1 = Encoding.ASCII.GetBytes(strACK);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("HTTP/1.1 200 OK \r\n");
                    sb.Append("Server:AR's Server \r\n");
                    sb.Append(string.Format("Date: {0} \r\n",DateTime.Now.ToShortDateString()));
                    sb.Append(string.Format("Content-Length: {0}\r\n", bSend1.Length));
                    sb.Append("content-type:application/json \r\n\r\n");
                    //sb.Append(strACK+"\r\n");
                    if (string.Equals(met.method, string.Format("version"), StringComparison.InvariantCultureIgnoreCase))
                    {
                        try
                        {
                            Console.WriteLine(met.method);
                            Console.WriteLine(strACK);
                            bSend = Encoding.ASCII.GetBytes(sb.ToString());
                            mySocket.Send(bSend);
                            mySocket.Send(bSend1);
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    mySocket.Close();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //Console.ReadKey();
            }
            //Console.ReadKey();
        }

        private void sendResponse(string str,Socket s)
        {
            String sResponse = string.Empty;
            sResponse += "HTTP/1.1 200 OK\r\n";
            sResponse += "Server:Anthony's Server\r\n";
            sResponse += "Expires:0\r\n";
            sResponse += "Pragma:Json Test\r\n";
            sResponse += "Content-Type: application/json\r\n";
            sResponse += "Hello\r\n";
            byte[] bSendData = Encoding.ASCII.GetBytes(sResponse);
            try
            {
                s.Send(bSendData);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void sendResponse(BinaryWriter writer)
        {
            String sResponse = string.Empty;
            sResponse += "HTTP/1.1 200 OK\r\n";
            sResponse += "Server:Anthony's Server\r\n";
            sResponse += "Expires:0\r\n";
            sResponse += "Pragma:Json Test\r\n";
            sResponse += "Content-Type: application/json\r\n";
            sResponse += "Hello\r\n";
            byte[] bSendData = Encoding.ASCII.GetBytes(sResponse);
            try
            {
                writer.Write(bSendData);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public class method1
        {
            public string method { get; set; }
        }
    }
}
