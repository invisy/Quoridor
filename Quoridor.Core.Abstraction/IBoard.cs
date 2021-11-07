using Quoridor.Core.Abstraction.Common;
using System;

namespace Quoridor.Core.Abstraction
{
    public interface IBoard : IReadableBoard
    {
        bool TrySetPawn(IPawn pawn, Point coordinate);
        bool TryPutFence(Point coordinate, FenceDirection fenceDirection);
        void RemoveFenceIfExists(Point coordinate);
    }
}