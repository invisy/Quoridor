using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction;

public interface IReadableBoard
{
    IReadablePawn[,] Tiles { get; }
    IReadOnlyList<Fence> Fences { get; }
    IReadOnlyDictionary<FenceDirection, Fence[,]> Passages { get; }
}
