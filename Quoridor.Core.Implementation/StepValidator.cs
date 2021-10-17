using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Implementation
{
    public class StepValidator : IStepValidator
    {
        private int _boardSideLength = 0;
        private IReadablePawn[,] _tiles;
        private Fence[,] _fences;

        public List<Point> GetPossibleSteps(IBoard board, Point startPoint)
        {
            List<Point> points = new List<Point>();

            _boardSideLength = board.Tiles.GetLength(0);
            _tiles = board.Tiles;
            _fences = board.FenceCrossroads;

            points.AddRange(GetPossibleUpSteps(startPoint));
            points.AddRange(GetPossibleDownSteps(startPoint));
            points.AddRange(GetPossibleLeftSteps(startPoint));
            points.AddRange(GetPossibleRightSteps(startPoint));

            return points;
        }

        private List<Point> GetPossibleUpSteps(Point point)
        {
            List<Point> points = new List<Point>();

            if (PassageUpExists(point))
            {
                if (_tiles[point.X, point.Y - 1] == null)
                    points.Add(new Point(point.X, point.Y - 1));
                else
                {
                    Point jumpOverPlayer = new Point(point.X, point.Y - 1);
                    if (PassageUpExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X, jumpOverPlayer.Y - 1] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y - 1));
                    }
                    else
                    {
                        if (PassageLeftExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X-1, jumpOverPlayer.Y] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X-1, jumpOverPlayer.Y));
                        }
                        if (PassageRightExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X + 1, jumpOverPlayer.Y] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X+1, jumpOverPlayer.Y));
                        }
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
                if (_tiles[point.X, point.Y + 1] == null)
                    points.Add(new Point(point.X, point.Y + 1));
                else
                {
                    Point jumpOverPlayer = new Point(point.X, point.Y + 1);
                    if (PassageDownExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X, jumpOverPlayer.Y + 1] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y + 1));
                    }
                    else
                    {
                        if (PassageLeftExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X - 1, jumpOverPlayer.Y] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X - 1, jumpOverPlayer.Y));
                        }
                        if (PassageRightExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X + 1, jumpOverPlayer.Y] == null)
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
                if (_tiles[point.X-1, point.Y] == null)
                    points.Add(new Point(point.X-1, point.Y));
                else
                {
                    Point jumpOverPlayer = new Point(point.X-1, point.Y);
                    if (PassageLeftExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X-1, jumpOverPlayer.Y] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X-1, jumpOverPlayer.Y));
                    }
                    else
                    {
                        if (PassageUpExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X, jumpOverPlayer.Y-1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y-1));
                        }
                        if (PassageDownExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X, jumpOverPlayer.Y+1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y+1));
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
                if (_tiles[point.X + 1, point.Y] == null)
                    points.Add(new Point(point.X + 1, point.Y));
                else
                {
                    Point jumpOverPlayer = new Point(point.X + 1, point.Y);
                    if (PassageRightExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X + 1, jumpOverPlayer.Y] == null)
                    {
                        points.Add(new Point(jumpOverPlayer.X + 1, jumpOverPlayer.Y));
                    }
                    else
                    {
                        if (PassageUpExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X, jumpOverPlayer.Y - 1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y - 1));
                        }
                        if (PassageDownExists(jumpOverPlayer) && _tiles[jumpOverPlayer.X, jumpOverPlayer.Y + 1] == null)
                        {
                            points.Add(new Point(jumpOverPlayer.X, jumpOverPlayer.Y + 1));
                        }
                    }
                }
            }

            return points;
        }

        private bool PassageUpExists(Point point)
        {
            if (point.Y > 0)
            {
                if ((point.X == 0 || _fences[point.X-1, point.Y-1] == null || _fences[point.X - 1, point.Y - 1].Direction == FenceDirection.VERTICAL) &&
                    (point.X == _boardSideLength-1 || _fences[point.X, point.Y - 1] == null || _fences[point.X, point.Y - 1].Direction == FenceDirection.VERTICAL))
                {
                    return true;
                }
            }

            return false;
        }
        private bool PassageDownExists(Point point)
        {
            if (point.Y != _boardSideLength-1)
            {
                if ((point.X == 0 || _fences[point.X - 1, point.Y] == null || _fences[point.X - 1, point.Y].Direction == FenceDirection.VERTICAL) &&
                    (point.X == _boardSideLength - 1 || _fences[point.X, point.Y] == null || _fences[point.X, point.Y].Direction == FenceDirection.VERTICAL))
                {
                    return true;
                }
            }

            return false;
        }
        private bool PassageLeftExists(Point point)
        {
            if (point.X > 0)
            {
                if ((point.Y == 0 || _fences[point.X - 1, point.Y - 1] == null || _fences[point.X - 1, point.Y - 1].Direction == FenceDirection.HORIZONTAL) &&
                    (point.Y == _boardSideLength - 1 || _fences[point.X-1, point.Y] == null || _fences[point.X-1, point.Y].Direction == FenceDirection.HORIZONTAL))
                {
                    return true;
                }
            }

            return false;
        }
        private bool PassageRightExists(Point point)
        {
            if (point.X != _boardSideLength - 1)
            {
                if ((point.Y == 0 || _fences[point.X, point.Y-1] == null || _fences[point.X, point.Y-1].Direction == FenceDirection.HORIZONTAL) &&
                    (point.Y == _boardSideLength - 1 || _fences[point.X, point.Y] == null || _fences[point.X, point.Y].Direction == FenceDirection.HORIZONTAL))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
