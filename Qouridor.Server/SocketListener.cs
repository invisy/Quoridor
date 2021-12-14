using System.Net;
using System.Net.Sockets;
using System.Text.Json;

class SocketListener
{
    readonly int portNumber;

    readonly Socket listenerSocket;
    readonly IPAddress ipAddress;

    public SocketListener(int portNumber)
    {
        this.portNumber = portNumber;

        ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
        listenerSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    }

    public void Start(Action<Socket, Socket> handleConnection)
    {
        Console.WriteLine("Server started.");

        listenerSocket.Bind(new IPEndPoint(ipAddress, portNumber));
        listenerSocket.Listen(4);

        while (true)
        {
            Console.WriteLine("Waiting for connection...");

            var player1ConnectionSocket = listenerSocket.Accept();
            Console.WriteLine("First player joined.");
            var player2ConnectionSocket = listenerSocket.Accept();
            Console.WriteLine("Second player joined.");

            handleConnection(player1ConnectionSocket, player2ConnectionSocket);
            Console.WriteLine("Connection handled.");

            player1ConnectionSocket.Shutdown(SocketShutdown.Both);
            player2ConnectionSocket.Shutdown(SocketShutdown.Both);
            player2ConnectionSocket.Close();
            player1ConnectionSocket.Close();
            Console.WriteLine("Connections closed.");
        }
    }

    public static void Send<T>(Socket socket, T data)
    {
        var dataBytes = JsonSerializer.SerializeToUtf8Bytes(data);
        socket.Send(dataBytes);
    }

    public static T Receive<T>(Socket socket)
    {
        var buffer = new byte[1024];
        var bytesCount = socket.Receive(buffer);

        return JsonSerializer.Deserialize<T>(new ReadOnlySpan<byte>(buffer, 0, bytesCount));
    }
}
