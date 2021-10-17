using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Implementation
{
    public class PathFinder : IPathFinder
    {
        IBoard _board;
        IStepValidator _stepValidator;

        List<Point> visited = new List<Point>();
        List<Point> unvisited = new List<Point>();

        public PathFinder(IBoard board, IStepValidator stepValidator)
        {
            _board = board;
            _stepValidator = stepValidator;
        }

        //Not optimized to A* yet
        public bool PathExistsToAny(Point point, List<Point> points)
        {
            visited = new List<Point>();
            unvisited = new List<Point>();
            unvisited.Add(point);

            while (unvisited.Count > 0)
            {
                if (points.Find(x => x.Equals(unvisited[0])) != null)
                    return true;
                Point currentPoint = unvisited[0];
                visited.Add(currentPoint);
                unvisited.Remove(currentPoint);

                List<Point> newPoints = _stepValidator.GetPossibleSteps(_board, currentPoint);

                foreach (Point newPoint in newPoints)
                {
                    if (visited.Find(x => x.Equals(newPoint)) == null &&
                        unvisited.Find(x => x.Equals(newPoint)) == null)
                    {
                        unvisited.Add(newPoint);
                    }
                }
            }
            return false;
        }
    }
}
