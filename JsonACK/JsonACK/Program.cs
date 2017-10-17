using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Newtonsoft.Json;

namespace JsonACK
{
    class Program
    {
        static byte[] buffer = new byte[1024];


        public static void Main(string[] args)
        {
            

            Listener li = new Listener();
            //TestAsyncSocket();

            Console.WriteLine("Server is ready!");
            Console.Read();
        }

        public static void TestAsyncSocket()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //将该socket绑定到主机上面的某个端口
            //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.bind.aspx
            socket.Bind(new IPEndPoint(IPAddress.Any, 9905));

            //启动监听，并且设置一个最大的队列长度
            //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.listen(v=VS.100).aspx
            socket.Listen(4);

            //开始接受客户端连接请求
            //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.beginaccept.aspx
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
        }

        public static void ClientAccepted(IAsyncResult ar)
        {

            var socket = ar.AsyncState as Socket;

            //这就是客户端的Socket实例，我们后续可以将其保存起来
            var client = socket.EndAccept(ar);
            string strACK = "{\"HedaACK\":{\"Body\":{\"retCode\":\"0\",\"retMsg\":\"CMS 1.0.0.1   \"},\"Header\":{\"MessageType\":\"MSG_SC_SYSTEM_IFNO_ACK\",\"Version\":\"1.0\"}}}";

            String sResponse = "";

            sResponse += "HTTP/1.1 200 OK\r\n";
            sResponse += "Server: iSpy\r\n";
            sResponse += "Expires: 0\r\n";
            sResponse += "Pragma: no-cache\r\n";
            sResponse += "Content-Type: multipart/x-mixed-replace;boundary=--myboundary";
            sResponse += "\r\n\r\n";
            //给客户端发送一个欢迎消息
            client.Send(Encoding.ASCII.GetBytes(sResponse + DateTime.Now.ToString()));
            //client.Send(Encoding.ASCII.GetBytes(strACK + DateTime.Now.ToString()));


            ////实现每隔两秒钟给服务器发一个消息
            ////这里我们使用了一个定时器
            //var timer = new System.Timers.Timer();
            //timer.Interval = 2000D;
            //timer.Enabled = true;
            //timer.Elapsed += (o, a) =>
            //{
            //    //检测客户端Socket的状态
            //    if (client.Connected)
            //    {
            //        try
            //        {
            //            client.Send(Encoding.Unicode.GetBytes("Message from server at " + DateTime.Now.ToString()));
            //        }
            //        catch (SocketException ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    }
            //    else
            //    {
            //        timer.Stop();
            //        timer.Enabled = false;
            //        Console.WriteLine("Client is disconnected, the timer is stop.");
            //    }
            //};
            //timer.Start();


            //接收客户端的消息(这个和在客户端实现的方式是一样的）
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), client);

            //准备接受下一个客户端请求
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
        }

        public static void ReceiveMessage(IAsyncResult ar)
        {

            try
            {
                var socket = ar.AsyncState as Socket;

                //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.endreceive.aspx
                var length = socket.EndReceive(ar);
                //读取出来消息内容
                var message = Encoding.ASCII.GetString(buffer, 0, length);
                //显示消息
                Console.WriteLine(message);

                //接收下一个消息(因为这是一个递归的调用，所以这样就可以一直接收消息了）
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
