using Quoridor.Core.Abstraction.Common;
using System;
using System.Collections.Generic;

namespace Quoridor.Core.Abstraction
{
    public interface IGameEngine
    {
        IReadableBoard Board { get; }
        IReadablePawn CurrentPlayer { get; }
        IReadOnlyList<IReadablePawn> AllPlayers { get; }
        Stack<Move> MoveHistory { get; }
        IReadablePawn? Winner { get; }
        event Action GameStarted;
        event Action<Move>? BoardUpdated;
        event Action GameEnded;
        bool TryMovePawn(Point position, bool isJump = false);
        bool TryPlaceFence(Point position, FenceDirection direction);
        public IEnumerable<Point> GetWinPointsForPlayer(IPawn pawn);
        void Start();
    }
}
