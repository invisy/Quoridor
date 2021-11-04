using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System;

namespace Quoridor.Core.Implementation
{
    public abstract class Pawn : IPawn
    {
        public string Name { get; }
        public PawnColor Color { get; }
        public int NumberOfFences { get; private set; }
        public Point Position { get; set; } = new Point(-1, -1);

        public bool IsOutOfBoard => Position.X == -1 && Position.Y == -1;

        public Pawn(string name, int numberOfFences, PawnColor color)
        {
            Name = name;
            Color = color;

            if (numberOfFences > 0)
                NumberOfFences = numberOfFences;
            else
                throw new ArgumentOutOfRangeException("Number of fences must be positive!");
        }

        public bool TryTakeFence()
        {
            if (NumberOfFences == 0)
                return false;

            NumberOfFences--;
            return true;
        }
    }
}