using System;
using System.Collections.Generic;
using System.Linq;

using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using Quoridor.Core.Implementation;
using Quoridor.MVC.Extensions;
using Quoridor.MVC.Structures;
using Quoridor.MVC.Utilites;
using Quoridor.MVC.Views;

namespace Quoridor.MVC
{
    class GameController
    {
        readonly BoardView boardView;
        readonly MenuView menuView;
        readonly WrongCommandView wrongCommandView;
        readonly WinnerView winnerView;

        IGameEngine currentGameEngine;

        public GameController()
        {
            boardView = new();
            menuView = new();
            wrongCommandView = new();
            winnerView = new();
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
            Console.Clear();
            boardView.DrawBoard(currentGameEngine.Board);
        }

        void ShowWrongCommandMessage(WrongCommandReason reason)
        {
            Console.Clear();
            wrongCommandView.DrawMessage(reason);
        }

        void ShowBoardWithErrorMessage(WrongCommandReason reason)
        {
            Console.Clear();
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
        }

        void CreatePlayerVsBotGame()
        { 
            IGameCreator game = new PlayerVsBotGameCreator();
            currentGameEngine = game.Create();
            currentGameEngine.GameEnded += GameEnded;
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
    }
}
