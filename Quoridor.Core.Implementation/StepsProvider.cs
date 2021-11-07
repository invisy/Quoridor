using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Implementation
{
    public class StepsProvider : IStepsProvider
    {
        private int _boardSideLength = 0;
        IReadableBoard? _board;

        public List<Point> GetPossibleSteps(IReadableBoard board, Point startPoint)
        {
            List<Point> points = new List<Point>();

            _boardSideLength = board.Tiles.GetLength(0);
            _board = board;

            if (PassageUpExists(startPoint))
                points.Add(startPoint + (0, -1));
            if (PassageDownExists(startPoint))
                points.Add(startPoint + (0, 1));
            if (PassageLeftExists(startPoint))
                points.Add(startPoint + (-1, 0));
            if (PassageRightExists(startPoint))
                points.Add(startPoint + (1, 0));

            return points;
        }

        public List<Point> GetPossibleJumps(IReadableBoard board, Point startPoint)
        {
            List<Point> points = new List<Point>();

            _boardSideLength = board.Tiles.GetLength(0);
            _board = board;

            points.AddRange(GetPossibleUpSteps(startPoint));
            points.AddRange(GetPossibleDownSteps(startPoint));
            points.AddRange(GetPossibleLeftSteps(startPoint));
            points.AddRange(GetPossibleRightSteps(startPoint));

            return points;
        }

        private List<Point> GetPossibleUpSteps(Point point)
        {
            List<Point> points = new List<Point>();

            if (PassageUpExists(point) && _board.Tiles[point.X, point.Y - 1] != null)
            {
                Point jumpOverPlayer = new Point(point.X, point.Y - 1);

                if (PassageUpExists(jumpOverPlayer) && (_board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y - 1] == null))
                {
                    points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y - 1));
                }
                else
                {
                    if (PassageLeftExists(jumpOverPlayer) && _board.Tiles[jumpOverPlayer.X - 1, jumpOverPlayer.Y] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X - 1, jumpOverPlayer.Y));
                    }
                    if (PassageRightExists(jumpOverPlayer) && _board.Tiles[jumpOverPlayer.X + 1, jumpOverPlayer.Y] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X + 1, jumpOverPlayer.Y));
                    }
                }
            }

            return points;
        }

        private List<Point> GetPossibleDownSteps(Point point)
        {
            List<Point> points = new List<Point>();

            if (PassageDownExists(point))
            {
                if (_board.Tiles[point.X, point.Y + 1] != null)
                {
                    Point jumpOverPlayer = new Point(point.X, point.Y + 1);
                    if (PassageDownExists(jumpOverPlayer) && _board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y + 1] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y + 1));
                    }
                    else
                    {
                        if (PassageLeftExists(jumpOverPlayer) && _board.Tiles[jumpOverPlayer.X - 1, jumpOverPlayer.Y] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X - 1, jumpOverPlayer.Y));
                        }
                        if (PassageRightExists(jumpOverPlayer) && _board.Tiles[jumpOverPlayer.X + 1, jumpOverPlayer.Y] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X + 1, jumpOverPlayer.Y));
                        }
                    }
                }
            }

            return points;
        }

        private List<Point> GetPossibleLeftSteps(Point point)
        {
            List<Point> points = new List<Point>();

            if (PassageLeftExists(point))
            {
                if (_board.Tiles[point.X - 1, point.Y] != null)
                {
                    Point jumpOverPlayer = new Point(point.X - 1, point.Y);
                    if (PassageLeftExists(jumpOverPlayer) && _board.Tiles[jumpOverPlayer.X - 1, jumpOverPlayer.Y] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X - 1, jumpOverPlayer.Y));
                    }
                    else
                    {
                        if (PassageUpExists(jumpOverPlayer) && _board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y - 1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y - 1));
                        }
                        if (PassageDownExists(jumpOverPlayer) && _board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y + 1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y + 1));
                        }
                    }
                }
            }

            return points;
        }

        private List<Point> GetPossibleRightSteps(Point point)
        {
            List<Point> points = new List<Point>();

            if (PassageRightExists(point))
            {
                if (_board.Tiles[point.X + 1, point.Y] != null)
                {
                    Point jumpOverPlayer = new Point(point.X + 1, point.Y);
                    if (PassageRightExists(jumpOverPlayer) && _board.Tiles[jumpOverPlayer.X + 1, jumpOverPlayer.Y] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X + 1, jumpOverPlayer.Y));
                    }
                    else
                    {
                        if (PassageUpExists(jumpOverPlayer) && _board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y - 1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y - 1));
                        }
                        if (PassageDownExists(jumpOverPlayer) && _board.Tiles[jumpOverPlayer.X, jumpOverPlayer.Y + 1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y + 1));
                        }
                    }
                }
            }

            return points;
        }

        private bool PassageUpExists(Point playerCoordinate)
        {
            if(!PlayerIsOnTopSide(playerCoordinate))
            {
                Fence? leftFence = !PlayerIsOnLeftSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X - 1, playerCoordinate.Y - 1] : default;
                Fence? rightFence = !PlayerIsOnRightSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X, playerCoordinate.Y - 1] : default;

                bool blockedByLeftFence = (leftFence?.Direction == FenceDirection.Horizontal);
                bool blockedByRightFence = (rightFence?.Direction == FenceDirection.Horizontal);

                return !blockedByLeftFence && !blockedByRightFence;
            }
            
            return false;
        }
        private bool PassageDownExists(Point playerCoordinate)
        {
            if(!PlayerIsOnBottomSide(playerCoordinate))
            {
                Fence? leftFence = !PlayerIsOnLeftSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X - 1, playerCoordinate.Y] : default;
                Fence? rightFence = !PlayerIsOnRightSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X, playerCoordinate.Y] : default;

                bool blockedByLeftFence = (leftFence?.Direction == FenceDirection.Horizontal);
                bool blockedByRightFence = (rightFence?.Direction == FenceDirection.Horizontal);

                return !blockedByLeftFence && !blockedByRightFence;
            }

            return false;
        }
        private bool PassageLeftExists(Point playerCoordinate)
        {
            if(!PlayerIsOnLeftSide(playerCoordinate))
            {
                Fence? upperFence = !PlayerIsOnTopSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X - 1, playerCoordinate.Y - 1] : default;
                Fence? lowerFence = !PlayerIsOnBottomSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X - 1, playerCoordinate.Y] : default;

                bool blockedByUpperFence = (upperFence?.Direction == FenceDirection.Vertical);
                bool blockedByLowerFence = (lowerFence?.Direction == FenceDirection.Vertical);

                return !blockedByUpperFence && !blockedByLowerFence;
            }

            return false;
        }
        private bool PassageRightExists(Point playerCoordinate)
        {
            if(!PlayerIsOnRightSide(playerCoordinate))
            {
                Fence? upperFence = !PlayerIsOnTopSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X, playerCoordinate.Y - 1] : default;
                Fence? lowerFence = !PlayerIsOnBottomSide(playerCoordinate) ? _board.FenceCrossroads[playerCoordinate.X, playerCoordinate.Y] : default;

                bool blockedByUpperFence = (upperFence?.Direction == FenceDirection.Vertical);
                bool blockedByLowerFence = (lowerFence?.Direction == FenceDirection.Vertical);

                return !blockedByUpperFence && !blockedByLowerFence;
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
