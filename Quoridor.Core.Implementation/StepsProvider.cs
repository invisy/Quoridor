using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Implementation
{
    public class StepsProvider : IStepsProvider
    {
        private int _boardSideLength = 0;
        IReadableBoard? _board;

        public List<Point> GetPossibleSteps(IReadableBoard board, IReadablePawn pawn)
        {
            List<Point> points = new List<Point>();
            Point currentPlayerPosition = board.GetPawnPosition(pawn);

            _boardSideLength = board.Tiles.GetLength(0);
            _board = board;

            if (PassageUpExists(currentPlayerPosition, pawn))
                points.Add(currentPlayerPosition + (0, -1));
            if (PassageDownExists(currentPlayerPosition, pawn))
                points.Add(currentPlayerPosition + (0, 1));
            if (PassageLeftExists(currentPlayerPosition, pawn))
                points.Add(currentPlayerPosition + (-1, 0));
            if (PassageRightExists(currentPlayerPosition, pawn))
                points.Add(currentPlayerPosition + (1, 0));

            return points;
        }

        public List<Point> GetPossibleJumps(IReadableBoard board, IReadablePawn pawn)
        {
            List<Point> points = new List<Point>();
            Point currentPlayerPosition = board.GetPawnPosition(pawn);

            _boardSideLength = board.Tiles.GetLength(0);
            _board = board;

            points.AddRange(GetPossibleUpSteps(currentPlayerPosition, pawn));
            points.AddRange(GetPossibleDownSteps(currentPlayerPosition, pawn));
            points.AddRange(GetPossibleLeftSteps(currentPlayerPosition, pawn));
            points.AddRange(GetPossibleRightSteps(currentPlayerPosition, pawn));

            return points;
        }

        private List<Point> GetPossibleUpSteps(Point point, IReadablePawn currentPlayer)
        {
            List<Point> points = new List<Point>();

            if (PassageUpExists(point, currentPlayer) && _board.Tiles[point.X, point.Y - 1] != null)
            {
                Point jumpOverPlayer = new Point(point.X, point.Y - 1);

                if (PassageUpExists(jumpOverPlayer, currentPlayer) && (_board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y - 1] == null))
                {
                    points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y - 1));
                }
                else
                {
                    if (PassageLeftExists(jumpOverPlayer, currentPlayer) && _board.Tiles[jumpOverPlayer.X - 1, jumpOverPlayer.Y] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X - 1, jumpOverPlayer.Y));
                    }
                    if (PassageRightExists(jumpOverPlayer, currentPlayer) && _board.Tiles[jumpOverPlayer.X + 1, jumpOverPlayer.Y] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X + 1, jumpOverPlayer.Y));
                    }
                }
            }

            return points;
        }

        private List<Point> GetPossibleDownSteps(Point point, IReadablePawn currentPlayer)
        {
            List<Point> points = new List<Point>();

            if (PassageDownExists(point, currentPlayer))
            {
                if (_board.Tiles[point.X, point.Y + 1] != null)
                {
                    Point jumpOverPlayer = new Point(point.X, point.Y + 1);
                    if (PassageDownExists(jumpOverPlayer, currentPlayer) && _board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y + 1] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y + 1));
                    }
                    else
                    {
                        if (PassageLeftExists(jumpOverPlayer, currentPlayer) && _board.Tiles[jumpOverPlayer.X - 1, jumpOverPlayer.Y] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X - 1, jumpOverPlayer.Y));
                        }
                        if (PassageRightExists(jumpOverPlayer, currentPlayer) && _board.Tiles[jumpOverPlayer.X + 1, jumpOverPlayer.Y] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X + 1, jumpOverPlayer.Y));
                        }
                    }
                }
            }

            return points;
        }

        private List<Point> GetPossibleLeftSteps(Point point, IReadablePawn currentPlayer)
        {
            List<Point> points = new List<Point>();

            if (PassageLeftExists(point, currentPlayer))
            {
                if (_board.Tiles[point.X - 1, point.Y] != null)
                {
                    Point jumpOverPlayer = new Point(point.X - 1, point.Y);
                    if (PassageLeftExists(jumpOverPlayer, currentPlayer) && _board.Tiles[jumpOverPlayer.X - 1, jumpOverPlayer.Y] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X - 1, jumpOverPlayer.Y));
                    }
                    else
                    {
                        if (PassageUpExists(jumpOverPlayer, currentPlayer) && _board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y - 1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y - 1));
                        }
                        if (PassageDownExists(jumpOverPlayer, currentPlayer) && _board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y + 1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y + 1));
                        }
                    }
                }
            }

            return points;
        }

        private List<Point> GetPossibleRightSteps(Point point, IReadablePawn currentPlayer)
        {
            List<Point> points = new List<Point>();

            if (PassageRightExists(point, currentPlayer))
            {
                if (_board.Tiles[point.X + 1, point.Y] != null)
                {
                    Point jumpOverPlayer = new Point(point.X + 1, point.Y);
                    if (PassageRightExists(jumpOverPlayer, currentPlayer) && _board.Tiles[jumpOverPlayer.X + 1, jumpOverPlayer.Y] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X + 1, jumpOverPlayer.Y));
                    }
                    else
                    {
                        if (PassageUpExists(jumpOverPlayer, currentPlayer) && _board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y - 1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y - 1));
                        }
                        if (PassageDownExists(jumpOverPlayer, currentPlayer) && _board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y + 1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y + 1));
                        }
                    }
                }
            }

            return points;
        }

        private bool PassageUpExists(Point playerCoordinate, IReadablePawn currentPlayer)
        {
            if(!PlayerIsOnTopSide(playerCoordinate))
            {
                Fence? leftFence = !PlayerIsOnLeftSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X - 1, playerCoordinate.Y - 1] : default;
                Fence? rightFence = !PlayerIsOnRightSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X, playerCoordinate.Y - 1] : default;

                bool blockedByLeftFence = (leftFence?.Direction == FenceDirection.Horizontal);
                bool blockedByRightFence = (rightFence?.Direction == FenceDirection.Horizontal);
                bool blockedByAnotherPlayer = _board.Tiles[playerCoordinate.X, playerCoordinate.Y] != null && _board.Tiles[playerCoordinate.X, playerCoordinate.Y] != currentPlayer;

                return !blockedByLeftFence && !blockedByRightFence && !blockedByAnotherPlayer;
            }
            
            return false;
        }
        private bool PassageDownExists(Point playerCoordinate, IReadablePawn currentPlayer)
        {
            if(!PlayerIsOnBottomSide(playerCoordinate))
            {
                Fence? leftFence = !PlayerIsOnLeftSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X - 1, playerCoordinate.Y] : default;
                Fence? rightFence = !PlayerIsOnRightSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X, playerCoordinate.Y] : default;

                bool blockedByLeftFence = (leftFence?.Direction == FenceDirection.Horizontal);
                bool blockedByRightFence = (rightFence?.Direction == FenceDirection.Horizontal);
                bool blockedByAnotherPlayer = _board.Tiles[playerCoordinate.X, playerCoordinate.Y] != null && _board.Tiles[playerCoordinate.X, playerCoordinate.Y] != currentPlayer;

                return !blockedByLeftFence && !blockedByRightFence && !blockedByAnotherPlayer;
            }

            return false;
        }
        private bool PassageLeftExists(Point playerCoordinate, IReadablePawn currentPlayer)
        {
            if(!PlayerIsOnLeftSide(playerCoordinate))
            {
                Fence? upperFence = !PlayerIsOnTopSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X - 1, playerCoordinate.Y - 1] : default;
                Fence? lowerFence = !PlayerIsOnBottomSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X - 1, playerCoordinate.Y] : default;

                bool blockedByUpperFence = (upperFence?.Direction == FenceDirection.Vertical);
                bool blockedByLowerFence = (lowerFence?.Direction == FenceDirection.Vertical);
                bool blockedByAnotherPlayer = _board.Tiles[playerCoordinate.X, playerCoordinate.Y] != null && _board.Tiles[playerCoordinate.X, playerCoordinate.Y] != currentPlayer;

                return !blockedByUpperFence && !blockedByLowerFence && !blockedByAnotherPlayer;
            }

            return false;
        }
        private bool PassageRightExists(Point playerCoordinate, IReadablePawn currentPlayer)
        {
            if(!PlayerIsOnRightSide(playerCoordinate))
            {
                Fence? upperFence = !PlayerIsOnTopSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X, playerCoordinate.Y - 1] : default;
                Fence? lowerFence = !PlayerIsOnBottomSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X, playerCoordinate.Y] : default;

                bool blockedByUpperFence = (upperFence?.Direction == FenceDirection.Vertical);
                bool blockedByLowerFence = (lowerFence?.Direction == FenceDirection.Vertical);
                bool blockedByAnotherPlayer = _board.Tiles[playerCoordinate.X, playerCoordinate.Y] != null && _board.Tiles[playerCoordinate.X, playerCoordinate.Y] != currentPlayer;

                return !blockedByUpperFence && !blockedByLowerFence && !blockedByAnotherPlayer;
            }

            return false;
        }

        private bool PlayerIsOnLeftSide(Point playerPosition)
        {
            return (playerPosition.X == 0);
        }

        private bool PlayerIsOnRightSide(Point playerPosition)
        {
            return (playerPosition.X == _boardSideLength - 1);
        }

        private bool PlayerIsOnTopSide(Point playerPosition)
        {
            return (playerPosition.Y == 0);
        }

        private bool PlayerIsOnBottomSide(Point playerPosition)
        {
            return (playerPosition.Y == _boardSideLength - 1);
        }
    }
}
