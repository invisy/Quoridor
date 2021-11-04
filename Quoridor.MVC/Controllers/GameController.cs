using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using Quoridor.Core.Implementation;
using Quoridor.MVC.Extensions;
using Quoridor.MVC.Structures;
using Quoridor.MVC.Utilites;
using Quoridor.MVC.Views;
using System;
using System.Collections.Generic;
using System.Linq;

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
                case "white":
                    CreatePlayerVsBotGame(PlayerColor.White);
                    return;
                case "black":
                    CreatePlayerVsBotGame(PlayerColor.Black);
                    return;
                default:
                    ShowWrongCommandMessage(WrongCommandReason.CommandNotFound);
                    ProcessMenuCommand();
                    return;
            }
        }

        void ProcessGameCommand()
        {
            var gameCommand = Console.ReadLine();

            if (!gameCommand.IsCommandValid())
            {
                ShowBoardWithErrorMessage(WrongCommandReason.IsNullOrEmpty);
                ProcessGameCommand();
                return;
            }

            var splitCommand = gameCommand.ToLower().Split(' ');

            var commandType = splitCommand.ElementAt(0);
            IEnumerable<char> commandArguments = splitCommand.ElementAtOrDefault(1);

            switch (commandType)
            {
                case "jump":
                case "move":
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

                case "wall":
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

        void CreatePlayerVsBotGame(PlayerColor color)
        {
            var game = new PlayerVsBotGameCreator {PlayerColor = color};

            currentGameEngine = game.Create();
            currentGameEngine.GameEnded += GameEnded;
            currentGameEngine.Start();
        }

        bool TryPlaceFence(IEnumerable<char> fenceArguments)
        {
            var x = (int)Enum.Parse<HorizontalNotation>(fenceArguments.ElementAt(0).ToString());
            var y = int.Parse(fenceArguments.ElementAt(1).ToString()) - 1;

            var direction = fenceArguments.ElementAt(2).ParseFenceDirection().Value;

            return currentGameEngine.TryPlaceFence(new Point(x, y), direction);
        }

        bool TryMovePawn(IEnumerable<char> pawnArguments)
        {
            var x = (int)Enum.Parse<HorizontalNotation>(pawnArguments.ElementAt(0).ToString());
            var y = int.Parse(pawnArguments.ElementAt(1).ToString()) - 1;
            
            return currentGameEngine.TryMovePawn(new Point(x, y));
        }
    }
}
