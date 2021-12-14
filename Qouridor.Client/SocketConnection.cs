using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace Qouridor.Client;

class SocketConnection
{
    readonly int portNumber;

    readonly IPAddress ipAddress;

    public SocketConnection(string serverHostName, int portNumber)
    {
        this.portNumber = portNumber;

        ipAddress = Dns.GetHostEntry(serverHostName).AddressList[0];
        
    }
    
    public Socket Open()
    {
        Console.WriteLine("Connecting to server...");

        var socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(ipAddress, portNumber);

        Console.WriteLine("Connected successfully!");

        return socket;
    }

    public static void Send<T>(Socket socket, T data)
    {
        var dataBytes = JsonSerializer.SerializeToUtf8Bytes(data);

        socket.Send(dataBytes);
    }

    public static T Receive<T>(Socket socket)
    {
        var buffer = new byte[65536];
        var bytesCount = socket.Receive(buffer);

        return JsonSerializer.Deserialize<T>(new ReadOnlySpan<byte>(buffer, 0, bytesCount))!;
    }
}