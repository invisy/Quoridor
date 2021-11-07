using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Implementation
{
    public class BotVsBotGameCreator : IGameCreator
    {
        public IGameEngine Create(PawnColor firstPlayerColor)
        {
            int fencesNumber = 10;

            IBoard board = new Board(9);
            IStepsProvider stepsProvider = new StepsProvider();
            IPathFinder pathFinder = new PathFinder(stepsProvider);

            PawnColor secondPlayerColor = (firstPlayerColor == PawnColor.White) ? PawnColor.Black : PawnColor.White;

            IPawn player1 = new MiniMaxBotPawn("Bot1", fencesNumber, firstPlayerColor, pathFinder, stepsProvider);
            IPawn player2 = new MiniMaxBotPawn("Bot2", fencesNumber, secondPlayerColor, pathFinder, stepsProvider);

            IGameEngine gameEngine = new GameEngine(board, pathFinder, stepsProvider, player1, player2);

            return gameEngine;
        }
    }
}
