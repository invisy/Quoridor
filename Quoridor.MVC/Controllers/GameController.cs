using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using Quoridor.Core.Implementation;
using Quoridor.MVC.Extensions;
using Quoridor.MVC.Structures;
using Quoridor.MVC.Utilites;
using Quoridor.MVC.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Quoridor.MVC
{
    class GameController
    {
        readonly BoardView boardView;
        readonly MenuView menuView;
        readonly WrongCommandView wrongCommandView;
        readonly WinnerView winnerView;
        readonly GameStateView gameStateView;

        IGameEngine currentGameEngine;

        public GameController()
        {
            boardView = new();
            menuView = new();
            wrongCommandView = new();
            winnerView = new();
            gameStateView = new();
        }

        public void Start()
        {
            GoToMenu();
            GoToGame();
        }

        void GoToMenu()
        {
            ShowMenu();
            ProcessMenuCommand();
        }

        void GoToGame()
        {
            Console.Clear();
            ShowState();
            ShowBoard();
            ProcessGameCommand();
        }

        void ShowMenu()
        {
            Console.Clear();
            menuView.DrawMenu();
        }

        void ShowBoard()
        {
            boardView.DrawBoard(currentGameEngine.Board);
        }

        void ShowState()
        {
            gameStateView.DrawState(currentGameEngine.AllPlayers, currentGameEngine.CurrentPlayer);
        }

        void ShowWrongCommandMessage(WrongCommandReason reason)
        {
            Console.Clear();
            wrongCommandView.DrawMessage(reason);
        }

        void ShowBoardWithErrorMessage(WrongCommandReason reason)
        {
            Console.Clear();
            ShowState();
            ShowBoard();
            wrongCommandView.DrawMessage(reason);
        }

        void ProcessMenuCommand()
        {
            var menuCommand = Console.ReadLine().ToLower();

            if (!menuCommand.IsCommandValid())
            {
                ShowWrongCommandMessage(WrongCommandReason.IsNullOrEmpty);
                ProcessMenuCommand();
                return;
            }

            switch (menuCommand)
            {
                case "start pvp":
                    CreateTwoPlayersGame();
                    return;
                case "start pve":
                    CreatePlayerVsBotGame();
                    return;
                case "start eve":
                    CreateBotVsBotGame();
                    return;
                default:
                    ShowWrongCommandMessage(WrongCommandReason.CommandNotFound);
                    ProcessMenuCommand();
                    return;
            }
        }

        void ProcessGameCommand()
        {
            var gameCommand = Console.ReadLine().ToLower();

            if (!gameCommand.IsCommandValid())
            {
                ShowBoardWithErrorMessage(WrongCommandReason.IsNullOrEmpty);
                ProcessGameCommand();
                return;
            }

            var splittedCommand = gameCommand.Split(' ');

            var commandType = splittedCommand.First();
            var commandArguments = splittedCommand[1..];

            switch (commandType)
            {
                case "movepawn":
                    if (!commandArguments.IsMovePawnArgumentsValid())
                    {
                        ShowBoardWithErrorMessage(WrongCommandReason.InvalidArguments);
                        ProcessGameCommand();
                    }

                    if (!TryMovePawn(commandArguments))
                    {
                        ShowBoardWithErrorMessage(WrongCommandReason.UnableToMovePawn);
                        ProcessGameCommand();
                    }

                    GoToGame();
                    return;

                case "placewall":
                    if (!commandArguments.IsPlaceFenceArgumentsValid())
                    {
                        ShowBoardWithErrorMessage(WrongCommandReason.InvalidArguments);
                        ProcessGameCommand();
                    }

                    if (!TryPlaceFence(commandArguments))
                    {
                        ShowBoardWithErrorMessage(WrongCommandReason.UnableToPutFence);
                        ProcessGameCommand();
                    }

                    GoToGame();
                    return;

                case "jump":
                    if (!commandArguments.IsMovePawnArgumentsValid())
                    {
                        ShowBoardWithErrorMessage(WrongCommandReason.InvalidArguments);
                        ProcessGameCommand();
                    }

                    if (!TryJump(commandArguments))
                    {
                        ShowBoardWithErrorMessage(WrongCommandReason.UnableToMovePawn);
                        ProcessGameCommand();
                    }

                    GoToGame();
                    return;

                case "end":
                    Start();
                    return;
                default:
                    ShowBoardWithErrorMessage(WrongCommandReason.CommandNotFound);
                    ProcessGameCommand();
                    return;
            }
        }

        void GameEnded()
        {
            var winner = currentGameEngine.Winner;

            Console.Clear();
            winnerView.DrawWinner(winner.Name);

            Console.ReadKey();
            Start();
        }

        void CreateTwoPlayersGame()
        {
            IGameCreator game = new TwoPlayersGameCreator();
            currentGameEngine = game.Create();
            currentGameEngine.GameEnded += GameEnded;
            currentGameEngine.Start();
        }

        void CreatePlayerVsBotGame()
        {
            IGameCreator game = new PlayerVsBotGameCreator();
            currentGameEngine = game.Create(PawnColor.White);
            currentGameEngine.GameEnded += GameEnded;
            currentGameEngine.Start();
        }

        void CreateBotVsBotGame()
        {
            IGameCreator game = new BotVsBotGameCreator();
            currentGameEngine = game.Create();
            currentGameEngine.GameEnded += GameEnded;
            currentGameEngine.Start();
        }

        bool TryPlaceFence(IList<string> fenceArguments)
        {
            var x = int.Parse(fenceArguments[0]);
            var y = int.Parse(fenceArguments[1]);

            var point = new Point(x, y);

            var direction = fenceArguments[2].ParseFenceDirection().Value;

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
    }
}
