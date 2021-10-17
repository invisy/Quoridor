using Quoridor.Core.Abstraction;

namespace Quoridor.Core.Implementation
{
    public class PlayerVsBotGameCreator : IGameCreator
    {
        public IGameEngine Create()
        {
            int fencesNumber = 10;

            IBoard board = new Board(9);
            IStepValidator stepValidator = new StepValidator();
            IPathFinder pathFinder = new PathFinder(board, stepValidator);

            IPawn player1 = new LocalPlayerPawn("Player1", fencesNumber);
            IPawn player2 = new RandomBotPawn("Bot1", fencesNumber);

            IGameEngine gameEngine = new GameEngine(board, pathFinder, stepValidator, player1, player2);

            return gameEngine;
        }
    }
}
