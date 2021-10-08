using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _board  = board;
            _stepValidator = stepValidator;
        }

        //Not optimized to A* yet
        public bool PathExistsToAny(Point point, List<Point> points)
        {
            unvisited.Add(point);
            while (unvisited.Count > 0)
            {
                if (points.Find(x => x.Equals(unvisited[0])) != null)
                    return true;

                visited.Add(unvisited[0]);

                List<Point> newPoints = _stepValidator.GetPossibleSteps(_board, point);

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
