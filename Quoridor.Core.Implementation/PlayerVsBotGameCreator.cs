using Quoridor.Core.Abstraction;

namespace Quoridor.Core.Implementation
{
    public class PlayerVsBotGameCreator : IGameCreator
    {
        public PlayerColor PlayerColor { get; set; }

        public IGameEngine Create()
        {
            int fencesNumber = 10;

            IBoard board = new Board(9);
            IStepValidator stepValidator = new StepValidator();
            IPathFinder pathFinder = new PathFinder(board, stepValidator);

            IPawn player1 = new LocalPlayerPawn("Player1", fencesNumber);
            IPawn player2 = new RandomBotPawn("Bot1", fencesNumber);

            IGameEngine gameEngine = PlayerColor == PlayerColor.Black 
                ? new GameEngine(board, pathFinder, stepValidator, player1, player2) 
                : new GameEngine(board, pathFinder, stepValidator, player2, player1);

            return gameEngine;
        }
    }
}
