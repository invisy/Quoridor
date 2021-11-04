using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction
{
    public interface IReadableBoard
    {
        IReadablePawn[,] Tiles { get; }
        Fence[,] FenceCrossroads { get; }
        Point GetPawnPosition(IReadablePawn pawn);
    }
}