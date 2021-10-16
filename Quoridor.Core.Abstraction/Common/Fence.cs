using System.Collections.Generic;

namespace Quoridor.Core.Abstraction.Common
{
    public class Fence
    {
        private List<Point> fencePoints = new();
        public IReadOnlyList<Point> FencePoints => fencePoints;

        FenceDirection Direction { get; }
        public Fence(Point firstPoint, Point secondPoint, FenceDirection direction)
        {
            fencePoints.Add(firstPoint);
            fencePoints.Add(secondPoint);
            Direction = direction;
        }
    }
}
