using Qouridor.ConsoleAI.CommandHandlers;
using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System;

namespace Qouridor.ConsoleAI
{
    public class GameController
    {
        private readonly IGameEngine _gameEngine;

        public GameController(IGameCreator gameCreator)
        {
            _gameEngine = gameCreator.Create(RequestPlayerColor());
            _gameEngine.BoardUpdated += BoardUpdated;
            _gameEngine.Start();
            ProcessGame();
        }

        private PawnColor RequestPlayerColor()
        {
            PawnColor pawnColor = default(PawnColor);

            ICommandHandler colorCommandHandler = new ColorCommandHandler((color) => { pawnColor = color; });

            InputProcessor inputProcessor = new InputProcessor()
                .Add(colorCommandHandler);

            while (!inputProcessor.Process())
            {
                PrintInvalidCommandError();
            }

            if (pawnColor == PawnColor.White)
                return PawnColor.Black;

            return PawnColor.White;
        }

        private void ProcessGame()
        {
            ICommandHandler moveCommandHandler = new MoveCommandHandler((x, y) => 
                {
                    if (!_gameEngine.TryMovePawn(new Point((int)x, y-1)))
                        MoveIsNotValid();
                });

            ICommandHandler jumpCommandHandler = new JumpCommandHandler((x, y) =>
                {
                    if (!_gameEngine.TryMovePawn(new Point((int)x, y - 1), true))
                        MoveIsNotValid();
                });

            ICommandHandler wallCommandHandler = new WallCommandHandler((x, y, direction) =>
            {
                if (!_gameEngine.TryPlaceFence(new Point((int)x, y - 1), direction))
                    MoveIsNotValid();
            });

            InputProcessor inputProcessor = new InputProcessor()
                .Add(moveCommandHandler)
                .Add(jumpCommandHandler)
                .Add(wallCommandHandler);

            while (_gameEngine.Winner == null)
            {
                while (!inputProcessor.Process())
                {
                    PrintInvalidCommandError();
                }
            }
        }

        private void PrintInvalidCommandError()
        {
            Console.WriteLine("This command is invalid! Please, try again.");
        }

        private void MoveIsNotValid()
        {
            Console.WriteLine("This move is invalid! Please, try again.");
        }

        private void BoardUpdated(Move move)
        {
            MoveParser moveParser = new();
            if (move.MoveInitiator is IBotPawn)
                Console.WriteLine(moveParser.Parse(move));
        }
    }
}
