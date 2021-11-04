using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Implementation
{
    public class PlayerVsBotGameCreator : IGameCreator
    {
        public IGameEngine Create()
        {
            int fencesNumber = 10;

            IBoard board = new Board(9);
            IStepsProvider stepsProvider = new StepsProvider();
            IPathFinder pathFinder = new PathFinder(stepsProvider);

            PawnColor secondPlayerColor = (firstPlayerColor == PawnColor.White) ? PawnColor.Black : PawnColor.White;

            IPawn player1 = new LocalPlayerPawn("Player1", fencesNumber, firstPlayerColor);
            IPawn player2 = new RandomBotPawn("Bot1", fencesNumber, secondPlayerColor);

            IGameEngine gameEngine = new GameEngine(board, pathFinder, stepValidator, player1, player2);

            return gameEngine;
        }
    }
}
