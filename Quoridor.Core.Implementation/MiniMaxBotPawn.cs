using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quoridor.Core.Implementation
{
    public class MiniMaxBotPawn : Pawn, IBotPawn
    {
        private int DEPTH = 2;

        private IGameEngine _engine;
        private IPathFinder _pathFinder;
        private IStepsProvider _stepsProvider;

        private IPawn _enemy;
        public MiniMaxBotPawn(string name, int numberOfFences, PawnColor color, IPathFinder pathFinder, IStepsProvider stepsProvider) : base(name, numberOfFences, color)
        {
            _pathFinder = pathFinder;
            _stepsProvider = stepsProvider;
        }

        public void Run(IGameEngine gameEngine)
        {
            _engine = gameEngine;
            IReadablePawn currentPlayer = gameEngine.CurrentPlayer;
            IBoard board = (IBoard)gameEngine.Board;

            IEnumerable<IReadablePawn> pawns = gameEngine.Board.GetPawns();
            _enemy = FindEnemy(pawns) as IPawn;

            int bestResult = int.MinValue;
            Move bestMove = null;

            IEnumerable<Move> allMoves = GetAllMoves(board, this);

            /*if (allMoves.Count() < 20)
                DEPTH = DEPTH + 2;*/

            foreach (Move move in allMoves)
            {
                (int, Action) moveSimulation = SimulateMoveExecution(board, move);
                Action cancelMove = moveSimulation.Item2;

                if (moveSimulation.Item1 != int.MinValue)
                {
                    int result = MiniMax(board, DEPTH - 1, _enemy, MiniMaxEvaluation(-moveSimulation.Item1, _enemy));

                    if (result > bestResult)
                    {
                        bestResult = result;
                        bestMove = move;
                    }
                }

                cancelMove();
            }

            ExecuteMove(bestMove);
        }

        private int MiniMax(IBoard board, int depth, IReadablePawn pawn, int currentEvaluation)
        {
            IPawn currentEnemyPlayer = (IPawn)pawn == this ? _enemy : this;
            int bestResult = int.MinValue;

            if (depth == 0)
                return currentEvaluation;

            IEnumerable<Move> allMoves = GetAllMoves(board, pawn);
            foreach (Move move in allMoves)
            {
                (int, Action) moveSimulation = SimulateMoveExecution(board, move);
                Action cancelMove = moveSimulation.Item2;

                if (moveSimulation.Item1 != int.MinValue)
                {
                    int result = MiniMax(board, depth - 1, currentEnemyPlayer, MiniMaxEvaluation(-moveSimulation.Item1, currentEnemyPlayer));

                    if (result > bestResult)
                        bestResult = result;
                }

                cancelMove();
            }

            return bestResult;
        }

        private int MiniMaxEvaluation(int pathDifference, IReadablePawn pawn)
        {
            return pathDifference;
        }

        private (int,Action) SimulateMoveExecution(IBoard board, Move move)
        {
            int evaluation = int.MinValue;
            Action cancellationFunction = () => { };

            IPawn currentPlayer = (IPawn)move.MoveInitiator;
            IPawn currentEnemyPlayer = (IPawn)move.MoveInitiator == this ? _enemy : this;

            if (move.MoveType == MoveType.Step || move.MoveType == MoveType.Jump)
            {
                Point oldPosition = board.GetPawnPosition(currentPlayer);
                board.TrySetPawn(currentPlayer, move.PlayerPosition);

                PathFinderResult myDistance = _pathFinder.PathExistsToAnyWinPoint(board, currentPlayer);
                PathFinderResult enemyDistance = _pathFinder.PathExistsToAnyWinPoint(board, currentEnemyPlayer);

                if (myDistance.PathExists && enemyDistance.PathExists)
                    evaluation = enemyDistance.PathLength - myDistance.PathLength;
                else
                    evaluation = int.MinValue;  // this move will be never called
                cancellationFunction = () => board.TrySetPawn(currentPlayer, oldPosition);
            }
            else if (move.MoveType == MoveType.PlaceFence)
            {
                board.TryPutFence(move.FencePosition, move.FenceDirection);

                PathFinderResult myDistance = _pathFinder.PathExistsToAnyWinPoint(board, currentPlayer);
                PathFinderResult enemyDistance = _pathFinder.PathExistsToAnyWinPoint(board, currentEnemyPlayer);

                if (myDistance.PathExists && enemyDistance.PathExists)
                    evaluation = enemyDistance.PathLength - myDistance.PathLength;
                else
                    evaluation = int.MinValue;  // this move will be never called
                cancellationFunction = () => board.RemoveFenceIfExists(move.FencePosition);
            }

            return (evaluation, cancellationFunction);
        }

        private void ExecuteMove(Move move)
        {
            if (move.MoveType == MoveType.PlaceFence)
                _engine.TryPlaceFence(move.FencePosition, move.FenceDirection);
            else if(move.MoveType == MoveType.Step)
                _engine.TryMovePawn(move.PlayerPosition);
            else if (move.MoveType == MoveType.Jump)
                _engine.TryMovePawn(move.PlayerPosition, true);
        }

        private IEnumerable<Move> GetAllMoves(IBoard currentBoard, IReadablePawn pawn)
        {
            List<Move> moves = new();

            List<Point> steps = _stepsProvider.GetPossibleSteps(currentBoard, pawn);

            foreach (Point step in steps)
                moves.Add(new Move(step, pawn));

            List<Point> jumps = _stepsProvider.GetPossibleJumps(currentBoard, pawn);
            foreach (Point jump in jumps)
                moves.Add(new Move(jump, pawn, true));

            if (pawn.NumberOfFences > 0)
            {
                List<Point> freeFenceCrossroads = ParseFreeFenceCrossroads(currentBoard.FenceCrossroads);

                foreach (Point fenceCrossroad in freeFenceCrossroads)
                {
                    if (currentBoard.TryPutFence(fenceCrossroad, FenceDirection.Horizontal))
                    {
                        moves.Add(new Move(fenceCrossroad, FenceDirection.Horizontal, pawn));
                        currentBoard.RemoveFenceIfExists(fenceCrossroad);
                    }
                    else if (currentBoard.TryPutFence(fenceCrossroad, FenceDirection.Vertical))
                    {
                        moves.Add(new Move(fenceCrossroad, FenceDirection.Vertical, pawn));
                        currentBoard.RemoveFenceIfExists(fenceCrossroad);
                    }
                }
            }

            return moves;
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

        private IReadablePawn FindEnemy(IEnumerable<IReadablePawn> players)
        {
            List<IReadablePawn> playersList = players.ToList();
            if (playersList.Count == 2)
            {
                if (playersList[0] == this)
                    return playersList[1];
                else
                    return playersList[0];
            }
            else
                throw new ArgumentException("Now AI works only with 2 players!");
        }
    }
}
