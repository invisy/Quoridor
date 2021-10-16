using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Abstraction
{
    public interface IReadableBoard
    {
        IReadablePawn[,] Tiles { get; }
        IReadOnlyList<Fence> Fences { get; }
        IReadOnlyDictionary<FenceDirection, Fence[,]> Passages { get; }
    }
}