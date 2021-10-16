using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Abstraction
{
    public interface IPathFinder
    {
        bool PathExistsToAny(Point point, List<Point> points);
    }
}
