using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System;

namespace Quoridor.Core.Implementation
{
    public class Board : IBoard
    {
        private readonly IPawn?[,] _tiles;
        private readonly Fence?[,] _fenceCrossroads;

        public IReadablePawn[,] Tiles => (IPawn[,])_tiles.Clone();
        public Fence[,] FenceCrossroads => (Fence[,])_fenceCrossroads.Clone();

        public Board(int sideSize)
        {
            if (sideSize % 2 == 1 && sideSize > 1)
            {
                _tiles = new IPawn[sideSize, sideSize];
                _fenceCrossroads = new Fence[sideSize - 1, sideSize - 1];
            }
            else
                throw new ArgumentOutOfRangeException("Only odd positive value (except 1) is allowed!");
        }

        public bool TrySetPawn(IPawn pawn, Point coordinate)
        {
            if (PointIsOutOfTiles(coordinate) || TileIsOccupied(coordinate))
                return false;

            if (!pawn.IsOutOfBoard && _tiles[pawn.Position.X, pawn.Position.Y] != pawn)
                throw new Exception("Player coordinates doesn`t match board information about them!");

            if (!pawn.IsOutOfBoard)
                _tiles[pawn.Position.X, pawn.Position.Y] = null;
            pawn.Position = coordinate;
            _tiles[coordinate.X, coordinate.Y] = pawn;

            return true;
        }

        //Need to be refactored
        public bool TryPutFence(Point coordinate, FenceDirection fenceDirection)
        {
            if ((!PointIsOutOfFenceCrossroads(coordinate)) && FenceCrossroadIsClear(coordinate))
            {
                if (fenceDirection == FenceDirection.Horizontal &&
                    (coordinate.X == 0 || _fenceCrossroads[coordinate.X - 1, coordinate.Y] == null || _fenceCrossroads[coordinate.X - 1, coordinate.Y].Direction == FenceDirection.Vertical) &&
                     (coordinate.X == _fenceCrossroads.GetLength(0) - 1 || _fenceCrossroads[coordinate.X + 1, coordinate.Y] == null || _fenceCrossroads[coordinate.X + 1, coordinate.Y].Direction == FenceDirection.Vertical))
                {
                    _fenceCrossroads[coordinate.X, coordinate.Y] = new Fence(fenceDirection);
                    return true;
                }
                else if (fenceDirection == FenceDirection.Vertical &&
                         (coordinate.Y == 0 || _fenceCrossroads[coordinate.X, coordinate.Y - 1] == null || _fenceCrossroads[coordinate.X, coordinate.Y - 1].Direction == FenceDirection.Horizontal) &&
                          (coordinate.Y == _fenceCrossroads.GetLength(0) - 1 || _fenceCrossroads[coordinate.X, coordinate.Y + 1] == null ||
                                                        _fenceCrossroads[coordinate.X, coordinate.Y + 1].Direction == FenceDirection.Horizontal))
                {
                    _fenceCrossroads[coordinate.X, coordinate.Y] = new Fence(fenceDirection);
                    return true;
                }
            }

            return false;
        }
        public void RemoveFenceIfExists(Point coordinate)
        {
            if (!PointIsOutOfFenceCrossroads(coordinate))
            {
                _fenceCrossroads[coordinate.X, coordinate.Y] = null;
            }
        }

        private bool TileIsOccupied(Point point)
        {
            return _tiles[point.X, point.Y] != null;
        }

        private bool PointIsOutOfTiles(Point point)
        {
            return point.X < 0 || point.Y < 0 || point.X >= _tiles.GetLength(0) || point.Y >= _tiles.GetLength(0);
        }

        private bool PointIsOutOfFenceCrossroads(Point point)
        {
            return point.X < 0 || point.Y < 0 || point.X >= _fenceCrossroads.GetLength(0) || point.Y >= _fenceCrossroads.GetLength(0);
        }

        private bool FenceCrossroadIsClear(Point point)
        {
            return _fenceCrossroads[point.X, point.Y] == null;
        }
    }
}
