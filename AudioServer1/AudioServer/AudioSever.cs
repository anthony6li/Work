
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioClient
{
    class AudioSever
    {
        private Form1 Parent;
        private TcpListener myListener = null;
        private static Random r = new Random();
        public string ServerRoot;


        private Thread th = null;
        private static readonly List<Socket> MySockets = new List<Socket>();
        private static int _socketindex;
        public int numErr = 0;
        public AudioSever(Form1 form)
        {
            Parent = form;
        }
        public bool Running
        {

            get
            {
                if (th == null)
                    return false;
                return th.IsAlive;
            }
        }
        public void startServer()
        {
            try
            {
                myListener = new TcpListener(IPAddress.Any, Parent.nPort) { ExclusiveAddressUse = false };
                myListener.Start(200);

                if (th != null)
                {
                    while (th.ThreadState == ThreadState.AbortRequested)
                    {
                        Application.DoEvents();
                    }
                }
                th = new Thread(new ThreadStart(StartListen));
                th.Start();
            }
            catch (Exception e)
            {

            }
        }
        public void StartListen()
        {

            while (Running && numErr < 5)
            {
                //Accept a new connection
                try
                {
                    Socket mySocket = myListener.AcceptSocket();
                    if (MySockets.Count() < _socketindex + 1)
                    {
                        MySockets.Add(mySocket);
                    }
                    else
                        MySockets[_socketindex] = mySocket;

                    if (mySocket.Connected)
                    {
                        mySocket.NoDelay = true;
                        mySocket.ReceiveBufferSize = 8192;
                        mySocket.ReceiveTimeout = 1500;

                        try
                        {
                            //make a byte array and receive data from the client 
                            string sBuffer;
                            string sHttpVersion;

                            Byte[] bReceive = new Byte[1024];
                            mySocket.Receive(bReceive);
                            sBuffer = Encoding.ASCII.GetString(bReceive);

                            if (sBuffer.Substring(0, 4) == "TALK")
                            {
                                var socket = mySocket;
                                var feed = new Thread(p => AudioIn(socket));
                                _socketindex++;
                                feed.Start();
                                continue;
                            }

                            if (sBuffer.Substring(0, 3) != "GET")
                            {
                                continue;
                            }

                            int iStartPos = sBuffer.IndexOf("HTTP", 1, StringComparison.Ordinal);

                            sHttpVersion = sBuffer.Substring(iStartPos, 8);


                            int cid = -1, vid = -1, camid = -1;
                            int w = -1, h = -1;

                            string qs = sBuffer.Substring(4);
                            qs = qs.Substring(0, qs.IndexOf(" ", StringComparison.Ordinal)).Trim('/').Trim('?');
                            string[] nvs = qs.Split('&');

                            foreach (string s in nvs)
                            {
                                string[] nv = s.Split('=');
                                switch (nv[0].ToLower())
                                {
                                    case "c":
                                        cid = Convert.ToInt32(nv[1]);
                                        break;
                                    case "w":
                                        w = Convert.ToInt32(nv[1]);
                                        break;
                                    case "h":
                                        h = Convert.ToInt32(nv[1]);
                                        break;
                                    case "camid":
                                        camid = Convert.ToInt32(nv[1]); //mjpeg
                                        break;
                                    case "micid":
                                        vid = Convert.ToInt32(nv[1]);
                                        break;

                                }
                            }
                            if (cid != -1)
                                SendLiveFeed(cid, w, h, sHttpVersion, ref mySocket);
                            else
                            {
                                if (camid != -1)
                                {
                                    CameraWindow cw = Parent.GetCameraWindow(Convert.ToInt32(camid));
                                    if (cw.Camobject.settings.active)
                                    {
                                        String sResponse = "";

                                        sResponse += "HTTP/1.1 200 OK\r\n";
                                        sResponse += "Server: iSpy\r\n";
                                        sResponse += "Expires: 0\r\n";
                                        sResponse += "Pragma: no-cache\r\n";
                                        sResponse += "Content-Type: multipart/x-mixed-replace;boundary=--myboundary";


                                        Byte[] bSendData = Encoding.ASCII.GetBytes(sResponse);
                                        SendToBrowser(bSendData, mySocket);
                                        cw.OutSockets.Add(mySocket);
                                        _socketindex++;
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (vid != -1)
                                    {
                                        VolumeLevel vl = Parent.GetMicrophone(Convert.ToInt32(vid));
                                        if (vl != null)
                                        {
                                            String sResponse = "";

                                            sResponse += "HTTP/1.1 200 OK\r\n";
                                            sResponse += "Server: iSpy\r\n";
                                            sResponse += "Expires: 0\r\n";
                                            sResponse += "Pragma: no-cache\r\n";
                                            sResponse += "Content-Type: multipart/x-mixed-replace;boundary=--myboundary";
                                            sResponse += "\r\n\r\n";
                                            Byte[] bSendData = Encoding.ASCII.GetBytes(sResponse);
                                            SendToBrowser(bSendData, mySocket);
                                            vl.OutSockets.Add(mySocket);

                                            _socketindex++;
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        const string resp = "iSpy server is running";
                                        SendHeader(sHttpVersion, "", resp.Length, " 200 OK", 0, ref mySocket);
                                        SendToBrowser(resp, ref mySocket);
                                    }
                                }
                            }
                            numErr = 0;
                        }
                        catch (SocketException ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Server Error (socket): " + ex.Message);
                            //MainForm.LogExceptionToFile(ex);
                            numErr++;
                        }
                        mySocket.Close();
                        mySocket = null;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Server Error (generic): " + ex.Message);
                    //MainForm.LogExceptionToFile(ex);
                    numErr++;
                }
            }
        }
    }
}
