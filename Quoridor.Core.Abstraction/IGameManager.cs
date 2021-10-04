using Quoridor.Core.Models;

namespace Quoridor.Core.Abstraction
{
    public interface IGameManager
    {
        public bool TryMove(Point position);
        public bool TryPlaceWall(Point position, FenceDirection direction);
        public Pawn GetCurrentPlayer();
        public IReadOnlyList<Pawn> GetAllPlayers();
        public Board GetBoard();
    }
}
