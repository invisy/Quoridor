using System.Net;

using Qouridor.Contracts.Requests;
using Qouridor.Contracts.Responses;

namespace Qouridor.Client;
    
class GameClient
{
    const int Port = 3000;

    readonly SocketConnection socketConnection;

    public GameClient()
    {
        socketConnection = new SocketConnection(Dns.GetHostName(), Port);
    }

    public void Start()
    {
        var connection = socketConnection.Open();

        Console.WriteLine($"Starting your game! Waiting for players..");

        var gameStartedResponse = SocketConnection.Receive<Responses>(connection);
        
        Console.Clear();
        Console.WriteLine($"Game started! Time to make your first move!");
        Console.WriteLine(gameStartedResponse.GameStartedResponse.Board);

        while (true)
        {
            Console.WriteLine($"Make your move! movepawn <x> <y>, placewall <x> <y> <v|h>, jump <x> <y>");

            var command = Console.ReadLine()!;

            SocketConnection.Send(connection, new Requests
            {
                MakeMoveRequest = new MakeMoveRequest { Command = command }
            });

            var response = SocketConnection.Receive<Responses>(connection);

            if (response.BoardUpdatedResponse is not null)
            {
                Console.Clear();
                Console.WriteLine(response.BoardUpdatedResponse.Board);
            }
            if (response.ErrorResponse is not null)
            {
                Console.WriteLine(response.ErrorResponse.Message);
            }
            else if (response.GameFinishedResponse is not null)
            {
                Console.Clear();
                Console.WriteLine(response.GameFinishedResponse.WinnerMessage);

                Console.ReadKey();
                return;
            }
        }
    }
}
