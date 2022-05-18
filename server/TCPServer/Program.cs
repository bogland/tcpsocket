using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

Server server = new Server();
server.StartListening();
class Server
{
    Socket listener;
    byte[] buffer = new Byte[1024];
    List<Socket> clientList = new List<Socket> ();
    void AcceptCallback(IAsyncResult ar)
    {
        Console.WriteLine("accepted...");
        Socket handler = listener.EndAccept(ar);
        clientList.Add(handler);
        StateObject state = new StateObject();
        handler.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReadCallback), state);
    }
    void ReadCallback(IAsyncResult ar)
    {
        string data = Encoding.ASCII.GetString(buffer, 0, buffer.Length);

        Console.WriteLine("receive..." + data);
        foreach(var client in clientList)
        {
            byte[] msg = Encoding.ASCII.GetBytes(data);
            client.Send(msg);
        }
    }
    public void StartListening()
    {
        string data = null;
        // Data buffer for incoming data.  


        // Establish the local endpoint for the socket.  
        // Dns.GetHostName returns the name of the
        // host running the application.  
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        // Create a TCP/IP socket.  
        listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and
        // listen for incoming connections.  
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);
            // Start listening for connections.  
            while (true)
            {
                listener.BeginAccept(new AsyncCallback(AcceptCallback),listener);

                //handler.Send(msg);
                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }
}

public class StateObject
{
    // Size of receive buffer.  
    public const int BufferSize = 1024;

    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];

    // Received data string.
    public StringBuilder sb = new StringBuilder();

    // Client socket.
    public Socket workSocket = null;
}