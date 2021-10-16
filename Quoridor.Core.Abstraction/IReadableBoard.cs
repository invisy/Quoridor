using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Abstraction
{
    public interface IReadableBoard
    {
        IReadablePawn[,] Tiles { get; }
        Fence[,] FenceCrossroads { get; }
    }
}