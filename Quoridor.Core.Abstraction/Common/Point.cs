using System;

namespace Quoridor.Core.Abstraction.Common
{
    public readonly struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Point? other)
        {
            return X == other?.X && Y == other?.Y;
        }

        public static Point operator + (Point point, (int X, int Y) vector) 
        {
            return new Point(point.X + vector.X, point.Y + vector.Y);
        }
    }
}
