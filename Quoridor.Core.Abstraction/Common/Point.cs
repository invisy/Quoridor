using System;

namespace Quoridor.Core.Abstraction.Common
{
    public class Point : IEquatable<Point>
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
    }
}
