using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Abstraction
{
    public interface IPathFinder
    {
        bool PathExistsToAny(IReadableBoard board, Point point, List<Point> points);
        int MinimalPathLengthToAny(IReadableBoard board, Point point, List<Point> points);
    }
}
