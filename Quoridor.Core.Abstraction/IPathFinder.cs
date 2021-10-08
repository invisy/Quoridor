using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction;

public interface IPathFinder
{
    bool PathExistsToAny(Point point, List<Point> points);
}
