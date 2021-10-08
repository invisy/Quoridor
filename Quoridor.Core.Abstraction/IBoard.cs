using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction;

public interface IBoard : IReadableBoard
{
    bool TrySetPawn(IPawn pawn, Point coordinate);
    bool TryPutFence(Point coordinate, FenceDirection fenceDirection);
    void RemoveFenceIfExists(Point position, FenceDirection direction);
}