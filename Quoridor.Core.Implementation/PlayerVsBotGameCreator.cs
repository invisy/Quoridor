﻿using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Implementation
{
    public class PlayerVsBotGameCreator : IGameCreator
    {
        public IGameEngine Create(PawnColor firstPlayerColor)
        {
            int fencesNumber = 10;

            IStepsProvider stepsProvider = new StepsProvider();
            IPathFinder pathFinder = new PathFinder(stepsProvider);

            PawnColor secondPlayerColor = (firstPlayerColor == PawnColor.White) ? PawnColor.Black : PawnColor.White;

            IPawn player1 = new LocalPlayerPawn("Player1", fencesNumber, firstPlayerColor);
            IPawn player2 = new MiniMaxBotPawn("Bot1", fencesNumber, secondPlayerColor, pathFinder, stepsProvider);
            
            IBoard board = new Board(9, player1, player2);
            IGameEngine gameEngine = new GameEngine(board, pathFinder, stepsProvider);

            return gameEngine;
        }
    }
}
