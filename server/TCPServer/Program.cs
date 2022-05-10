using System.Net;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");
var endPoint = new IPEndPoint(IPAddress.Any, 20000);
var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
socket.Bind(endPoint);
socket.Listen(100); //N명까지 허락
while (true)
{
    //SocketAsyncEventArgs arg = new SocketAsyncEventArgs();
    //arg.Completed += OnAccept;
    var socketClient = socket.Accept();
    socketClient.Send(new byte[] { 0, 1 });
}

//void OnAccept(object? sender, SocketAsyncEventArgs e)
//{
//    Console.WriteLine("{0} 접속됨", e.RemoteEndPoint.ToString());
//    var client = e.AcceptSocket;
//    Console.WriteLine("접속됨 {0}", client.RemoteEndPoint.ToString());
//}