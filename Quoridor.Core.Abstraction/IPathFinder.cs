using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction;

public interface IPathFinder
{
    bool PathExistsToAny(IBoard board, Point point, List<Point> points);
}
