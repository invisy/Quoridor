using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System;
using System.Collections.Generic;

namespace Quoridor.Core.Implementation
{
    public class RandomBotPawn : Pawn, IBotPawn
    {
        Random random = new Random();
        public RandomBotPawn(string name, int numberOfFences) : base(name, numberOfFences)
        {

        }

        public void Run(IGameEngine gameEngine)
        {
            IReadablePawn currentPlayer = gameEngine.CurrentPlayer;
            IReadableBoard board = gameEngine.Board;

            switch(random.Next(0, 2))
            {
                case 0:
                    MakeRandomMove(gameEngine, random);
                    break;
                case 1:
                    if(!PutRandomFence(gameEngine, random))
                    {
                        MakeRandomMove(gameEngine, random);
                    }
                    break;
            }        
        }

        private void MakeRandomMove(IGameEngine gameEngine, Random random)
        {
            IStepValidator stepValidator = new StepValidator();
            List<Point> steps = stepValidator.GetPossibleSteps(gameEngine.Board, gameEngine.CurrentPlayer.Position);

            int stepId = random.Next(0, steps.Count);
            while (steps.Count > 0 && !gameEngine.TryMovePawn(steps[stepId]))
            {
                steps.RemoveAt(stepId);
                stepId = random.Next(0, steps.Count);
            }
        }

        private bool PutRandomFence(IGameEngine gameEngine, Random random)
        {
            bool result = false;
            List<Point> freeFenceCrossroads = ParseFreeFenceCrossroads(gameEngine.Board.FenceCrossroads);
            int fenceCrossroadId = random.Next(0, freeFenceCrossroads.Count);

            FenceDirection fenceDirection = (FenceDirection)random.Next(0, 2);
            result = gameEngine.TryPlaceFence(freeFenceCrossroads[fenceCrossroadId], fenceDirection);
            while (freeFenceCrossroads.Count > 0 && !result)
            {
                freeFenceCrossroads.RemoveAt(fenceCrossroadId);
                fenceCrossroadId = random.Next(0, freeFenceCrossroads.Count);
                fenceDirection = (FenceDirection)random.Next(0, 2);
                if(freeFenceCrossroads.Count > 0)
                    result = gameEngine.TryPlaceFence(freeFenceCrossroads[fenceCrossroadId], fenceDirection);
            }

            return result;
        }

        private List<Point> ParseFreeFenceCrossroads(Fence[,] fences)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < fences.GetLength(0); i++)
            {
                for (int k = 0; k < fences.GetLength(0); k++)
                {
                    if (fences[i, k] == null)
                        points.Add(new Point(i, k));
                }
            }

            return points;
        }
    }
}
