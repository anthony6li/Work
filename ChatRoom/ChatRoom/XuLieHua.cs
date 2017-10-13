using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatRoom
{
    [Serializable]
    public struct NetMsg
    {
        public IPAddress Fip;     //发送者的IP。
        public string msg;        //发送的消息。
        public IPAddress JieIP;   //接收者的ip。
        public int port;          //端口。
    }
    public class XuLie
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static byte[] ObjToByte(object obj)
        {
            byte[] tmp = null;
            MemoryStream fs = new MemoryStream();
            try
            {
                BinaryFormatter Xu = new BinaryFormatter();
                Xu.Serialize(fs, obj);
                tmp = fs.ToArray();
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                fs.Close();
            }
            return tmp;
        }

        /// <summary>
        /// 反列化
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static object ByteToObj(byte[] tmp)
        {
            MemoryStream fs = null;
            object obj = null;
            try
            {
                fs = new MemoryStream(tmp);
                fs.Position = 0;
                BinaryFormatter Xu = new BinaryFormatter();
                obj = Xu.Deserialize(fs);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                fs.Close();
            }
            return obj;
        }
    }


    public class ServerJieShou
    {
        private static TcpClient Client;
        public Thread th;
        private ArrayList Arr;
        private LogText log;
        private bool Tiao = true;
        private Timer time1;
        private TimerCallback time;

        public ServerJieShou(TcpClient sClient, ArrayList arr)
        {
            log = new LogText("连接");
            Client = sClient;
            Arr = arr;
            th = new Thread(new ThreadStart(ThSub));
            th.IsBackground = true;
            th.Start();
            time = new TimerCallback(XinTiao);
            time1 = new Timer(time, null, 15000, -1);
            

        }
        private void XinTiao(object state)
        {
            if (Tiao == true)
            {
                Tiao = false;
            }
            else
            {
                Client = null;
            }
        }

        private void ThSub()
        {
            try
            {
                while (Client != null)
                {
                    NetworkStream Net = Client.GetStream();
                    if (Net.DataAvailable == true) //有数据。
                    {
                        byte[] tmp = new byte[1024];
                        if (Net.CanRead == true)
                        {
                            MemoryStream memory = new MemoryStream();
                            memory.Position = 0;
                            int len = 1;
                            while (len != 0)
                            {
                                if (Net.DataAvailable == false) { break; }
                                len = Net.Read(tmp, 0, tmp.Length);
                                memory.Write(tmp, 0, len);
                            }
                            log.LogWriter("接收完毕");
                            NetMsg msg = (NetMsg)XuLie.ByteToObj(memory.ToArray());
                            log.LogWriter("序列化完毕");
                            TcpClient tcpclient = new TcpClient();
                            log.LogWriter("建立TCP对象");
                            if (msg.Fip != null) //非心跳包。
                            {
                                try
                                {
                                    tcpclient.Connect(msg.JieIP, msg.port);
                                    NetworkStream SubNet = tcpclient.GetStream();
                                    byte[] Tmp = XuLie.ObjToByte(msg);
                                    SubNet.Write(Tmp, 0, Tmp.Length);
                                }
                                catch (SocketException)
                                {
                                    msg.msg = "对方不在线";
                                    byte[] Tmp = XuLie.ObjToByte(msg);
                                    Net.Write(Tmp, 0, Tmp.Length);
                                }
                            }
                            else
                            {
                                if (msg.msg == "QUIT")
                                {
                                    Arr.Remove(Client);
                                    return;
                                }
                            }
                            tcpclient.Close();
                            GC.Collect();
                        }
                    }
                    else //没有数据。
                    {
                    }
                    Thread.Sleep(1000);
                }
            }
            catch
            {
                Arr.Remove(Client);
                th.Abort();
            }
        }
    }
}
