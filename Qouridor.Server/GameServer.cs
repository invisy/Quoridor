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
    bool isGameFinished;
    
    const int Port = 3000;

    readonly SocketListener listener;

    IGameEngine currentGameEngine;

    public GameServer()
    {
        listener = new SocketListener(Port);
    }
  
    public void Start()
    {
        CreateTwoPlayersGame();
            
        listener.Start(Handle);
    }
    
    void CreateTwoPlayersGame()
    {
        isGameFinished = false;
        
        IGameCreator game = new TwoPlayersGameCreator();
        currentGameEngine = game.Create();
        currentGameEngine.GameEnded += GameEnded;
        currentGameEngine.Start();
    }
    
    void Handle(Socket player1Connection, Socket player2Connection)
    {
        ProcessPlayerFirstMove(player1Connection);
        ProcessPlayerFirstMove(player2Connection);

        while (true)
        {
            ProcessPlayerMove(player1Connection);

            if (isGameFinished)
            {
                FinishGame(player1Connection, player2Connection);
                return;
            }

            ProcessPlayerMove(player2Connection);
            
            if (isGameFinished)
            {
                FinishGame(player1Connection, player2Connection);
                return;
            }
        }
    }

    void ProcessPlayerFirstMove(Socket connection)
    {
        SendGameStarted(connection);
        RecievePlayerMove(connection);
    }
    
    void ProcessPlayerMove(Socket connection1)
    {
        SendBoardUpdated(connection1);
        RecievePlayerMove(connection1);
    }
    
    void FinishGame(Socket connection1, Socket connection2)
    {
        SendGameFinished(connection1, connection2);
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
                    Message = error.GetMessage()
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
                Board = GetTurnInfoMessage()
            }
        });
    }

   void SendBoardUpdated(Socket connection)
    {
        SocketListener.Send(connection, new Responses
        {
            BoardUpdatedResponse = new BoardUpdatedResponse
            {
                Board = GetTurnInfoMessage(),
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

    string GetTurnInfoMessage()
    {
        var gameStateMessage = GameStateRender.Render(currentGameEngine.Board.GetPawns(), currentGameEngine.CurrentPlayer);
        var boardViewMessage = BoardRenderer.Render(currentGameEngine.Board);

        return gameStateMessage + boardViewMessage;
    }
}
