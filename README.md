# Work
各种测试工程

|Author|Anthony Lee|
|---|---
|E-mail|anthony6li@163.com

## AudioClient：基于iSpy开源的语音通话Demo。
![界面图](https://github.com/anthony6li/ARImages/blob/master/ReadMe%E7%94%A8%E5%9B%BE/AudioClient.gif "AudioClient界面")

## AudioClient1：基于AudioClient拆分出来的Listener端。

## AudioServer1：基于AudioClient拆分出来的Speaker端。

## ChatRoom：网上查到的聊天室Demo，还需要优化。

## JsonACK：自己编写的c# 服务端，功能薄弱。
``` c
 var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
 socket.Bind(new IPEndPoint(IPAddress.Any, 9905));
 socket.Listen(4)
 socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
 ...
 var client = socket.EndAccept(ar);
 client.send(byte[] buffer);
 client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), client);
```

## JsonTestServer：Json接口测试功能第一版，测试用例是封装好写死的。单独的JsonTestTool工程是测试工具第二版，是灵活的版本，功能强大一些，用XML纪录测试用例。

## WindowsServiceTest：守护服务；Windows Service界面化安装；服务启动Winform已实现。
