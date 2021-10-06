using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction;

public interface IGameEngine
{
    IReadableBoard Board { get; }
    IReadablePawn CurrentPlayer { get; }
    IReadOnlyList<IReadablePawn> AllPlayers { get; }
    IReadablePawn Winner { get; }
    event EventHandler GameStarted;
    event EventHandler BoardUpdated;
    event EventHandler GameEnded;
    bool TryMovePawn(Point position);
    bool TryPlaceFence(Point position, FenceDirection direction);
}
