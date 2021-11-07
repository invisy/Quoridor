using Priority_Queue;
using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quoridor.Core.Implementation
{
    public class PathFinder : IPathFinder
    {
        public class Node<T> : FastPriorityQueueNode
        {
            public T Value { get; }
            public int PathLength { get; }

            public Node(T value, int previousPathLength)
            {
                Value = value;
                PathLength = previousPathLength;
            }
        }

        IStepsProvider _stepsProvider;

        public PathFinder(IStepsProvider stepsProvider)
        {
            _stepsProvider = stepsProvider;
        }

        //A*
        public PathFinderResult PathExistsToAnyWinPoint(IReadableBoard board, IReadablePawn currentPawn)
        {
            HashSet<Point> allPoints = new HashSet<Point>();
            FastPriorityQueue<Node<Point>> unvisited = new FastPriorityQueue<Node<Point>>(board.Tiles.Length); //PriorityQueue in .NET 6

            Point point = board.GetPawnPosition(currentPawn);
            IEnumerable<Point> winPoints = board.GetWinPointsForPlayer(currentPawn);
            allPoints.Add(point);
            unvisited.Enqueue(new Node<Point>(point, 0), 0);

            while (unvisited.Count > 0)
            {
                Node<Point> currentPoint = unvisited.Dequeue();

                if (winPoints.Where(x => x.Equals(currentPoint.Value)).Cast<Point?>().FirstOrDefault() != null)
                    return new PathFinderResult(true, currentPoint.PathLength);


                List<Point> newPoints = _stepsProvider.GetPossibleSteps(board, currentPoint.Value);
                newPoints.AddRange(_stepsProvider.GetPossibleJumps(board, currentPoint.Value));

                foreach (Point newPoint in newPoints)
                {
                    if (!allPoints.Contains(newPoint))
                    {
                        int PathLength = currentPoint.PathLength + 1;
                        int priority = PathLength + Heuristic(newPoint, winPoints.ToList());

                        allPoints.Add(newPoint);
                        unvisited.Enqueue(new Node<Point>(newPoint, PathLength), priority);
                    }
                }
            }
            return new PathFinderResult(false);
        }

        private int Heuristic(Point point, List<Point> winPoints)
        {
            bool isHorizontal = winPoints[0].Y == winPoints[1].Y;
            int winSideCoordinate = isHorizontal ? winPoints[0].Y : winPoints[0].X;
            if (isHorizontal)
                return Math.Abs(point.Y - winSideCoordinate);
            else
                return Math.Abs(point.X - winSideCoordinate);
        }
    }
}
