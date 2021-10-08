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
        List<Point> visited = new List<Point>();
        List<Point> unvisited = new List<Point>();

        public bool PathExistsToAny(IBoard board, Point point, List<Point> points)
        {
            visited.Add(point);

        }
    }
}
