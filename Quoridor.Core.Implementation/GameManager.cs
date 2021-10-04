using Quoridor.Core.Abstraction;
using Quoridor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Core.Implementation
{
    public class GameManager : IGameManager
    {
        private PlayerPawn currentPlayer;

        public GameManager(Board board, List<Pawn> playerPawns)
        {

        }

        public IReadOnlyList<Pawn> GetAllPlayers()
        {
            throw new NotImplementedException();
        }

        public Board GetBoard()
        {
            throw new NotImplementedException();
        }

        public Pawn GetCurrentPlayer()
        {
            throw new NotImplementedException();
        }

        public bool TryMove(Point position)
        {
            throw new NotImplementedException();
        }

        public bool TryPlaceWall(Point position, FenceDirection direction)
        {
            throw new NotImplementedException();
        }
    }
}
