namespace Quoridor.Core.Abstraction.Common;

public class Fence
{
    public IReadOnlyList<Point> FencePositions { get; }
    public Fence(List<Point> points)
    {
        FencePositions = points.AsReadOnly();
    }
}
