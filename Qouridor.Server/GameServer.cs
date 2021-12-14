using System.Net.Sockets;

using Qouridor.Contracts.Requests;
using Qouridor.Contracts.Responses;
using Qouridor.Server.Renderers;
using Qouridor.Server.Structures;
using Quoridor.Core.Abstraction;
using Quoridor.Core.Implementation;
using Quoridor.Server.Extensions;
using Quoridor.Server.Utilites;

using Point = Quoridor.Core.Abstraction.Common.Point;

class GameServer
{
    bool isGameFinished = false;
    
    const int Port = 3000;

    readonly SocketListener listener;

    IGameEngine currentGameEngine;

    public GameServer()
    {
        listener = new SocketListener(Port);
    }
    
    void CreateTwoPlayersGame()
    {
        IGameCreator game = new TwoPlayersGameCreator();
        currentGameEngine = game.Create();
        currentGameEngine.GameEnded += GameEnded;
        currentGameEngine.Start();
    }
    
    public void Start()
    {
        CreateTwoPlayersGame();
            
        listener.Start(Handle);
    }

    void Handle(Socket player1Connection, Socket player2Connection)
    {
        SendGameStarted(player1Connection);
        RecievePlayerMove(player1Connection);
        
        SendGameStarted(player2Connection);
        RecievePlayerMove(player2Connection);

        while (true)
        {
            SendBoardUpdated(player1Connection);
            RecievePlayerMove(player1Connection);

            if (isGameFinished)
            {
                SendGameFinished(player1Connection, player2Connection);
                return;
            }

            SendBoardUpdated(player2Connection);
            RecievePlayerMove(player2Connection);
            
            if (isGameFinished)
            {
                SendGameFinished(player1Connection, player2Connection);
                return;
            }
        }
    }

    void RecievePlayerMove(Socket connection)
    {
        var request = SocketListener.Receive<Requests>(connection);

        if (request.MakeMoveRequest is null)
            throw new Exception("Make move game request was not received!");
        
        var command = request.MakeMoveRequest.Command;
        var error = ProcessGameCommand(command);

        if (error is not null)
        {
            SocketListener.Send(connection, new Responses
            {
                ErrorResponse = new ErrorResponse()
                {
                    Message = error.ToString()! //GetMessage()
                }
            });

            RecievePlayerMove(connection);
        }
    }

    void SendGameStarted(Socket connection)
    {
        SocketListener.Send(connection, new Responses
        {
            GameStartedResponse = new GameStartedResponse
            {
                Board = BoardRenderer.Render(currentGameEngine.Board)
            }
        });
    }

   void SendBoardUpdated(Socket connection)
    {
        SocketListener.Send(connection, new Responses
        {
            BoardUpdatedResponse = new BoardUpdatedResponse
            {
                Board = BoardRenderer.Render(currentGameEngine.Board),
            }
        });
    }
   
   void SendGameFinished(Socket connection1, Socket connection2)
   {
       var winner = currentGameEngine.Winner;
       var winnerMessage = WinnerMessageRenderer.Render(winner.Name);
       
       SocketListener.Send(connection1, new Responses
       {
           GameFinishedResponse = new GameFinishedResponse
           {
               WinnerMessage = winnerMessage
           }
       });
       
       SocketListener.Send(connection2, new Responses
       {
           GameFinishedResponse = new GameFinishedResponse
           {
               WinnerMessage = winnerMessage,
           }
       });
   }

    WrongCommandReason? ProcessGameCommand(string gameCommand)
    {
        if (!gameCommand.IsCommandValid())
        {
            return WrongCommandReason.IsNullOrEmpty;
        }

        var splittedCommand = gameCommand.Split(' ');

        var commandType = splittedCommand.First();
        var commandArguments = splittedCommand[1..];

        switch (commandType)
        {
            case "movepawn":
                if (!commandArguments.IsMovePawnArgumentsValid())
                {
                    return WrongCommandReason.InvalidArguments;
                }

                if (!TryMovePawn(commandArguments))
                {
                    return WrongCommandReason.UnableToMovePawn;
                }

                return null;

            case "placewall":
                if (!commandArguments.IsPlaceFenceArgumentsValid())
                {
                    return WrongCommandReason.InvalidArguments;
                }

                if (!TryPlaceFence(commandArguments))
                {
                    return WrongCommandReason.UnableToPutFence;
                }

                return null;

            case "jump":
                if (!commandArguments.IsMovePawnArgumentsValid())
                {
                    return WrongCommandReason.InvalidArguments;
                }

                if (!TryJump(commandArguments))
                {
                    return WrongCommandReason.UnableToMovePawn;
                }

                return null;

            default:
                return WrongCommandReason.CommandNotFound;
        }
    }
    
    bool TryPlaceFence(IList<string> fenceArguments)
    {
        var x = int.Parse(fenceArguments[0]);
        var y = int.Parse(fenceArguments[1]);

        var point = new Point(x, y);

        var direction = fenceArguments[2].ParseFenceDirection()!.Value;

        return currentGameEngine.TryPlaceFence(point, direction);
    }

    bool TryMovePawn(IList<string> pawnArguments)
    {
        var x = int.Parse(pawnArguments[0]);
        var y = int.Parse(pawnArguments[1]);

        var point = new Point(x, y);

        return currentGameEngine.TryMovePawn(point);
    }

    bool TryJump(IList<string> pawnArguments)
    {
        var x = int.Parse(pawnArguments[0]);
        var y = int.Parse(pawnArguments[1]);

        var point = new Point(x, y);

        return currentGameEngine.TryMovePawn(point, true);
    }
    
    void GameEnded()
    {
        isGameFinished = true;
    }
    
    /*string RenderWinnerMessage(Winner winner, string playerName) => winner switch
    {
        Winner.Player => $"Congrats {playerName}! You have won this super extra bot!",
        Winner.Bot => "Sorry, but you lost to bot(",
        Winner.Draw => "It is draw! But you were close"
    };*/
}
