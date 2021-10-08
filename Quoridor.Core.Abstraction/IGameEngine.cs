using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction;

public interface IGameEngine
{
    IReadableBoard Board { get; }
    IReadablePawn CurrentPlayer { get; }
    IReadOnlyList<IReadablePawn> AllPlayers { get; }
    IReadablePawn Winner { get; }
    event Action GameStarted;
    event Action BoardUpdated;
    event Action GameEnded;

    public void AddPlayer(IPawn pawn);
    public void Start();
    bool TryMovePawn(Point position);
    bool TryPlaceFence(Point position, FenceDirection direction);
}
