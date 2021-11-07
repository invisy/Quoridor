using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Abstraction
{
    public interface IPathFinder
    {
        PathFinderResult PathExistsToAnyWinPoint(IReadableBoard board, IReadablePawn currentPlayer);
    }
}
