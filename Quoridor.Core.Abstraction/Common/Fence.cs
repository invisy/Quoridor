namespace Quoridor.Core.Abstraction.Common;

public class Fence
{
    private List<Point> fencePoints = new();
    public IReadOnlyList<Point> FencePoints => fencePoints;
    public Fence(Point firstPoint, Point secondPoint)
    {
        fencePoints.Add(firstPoint);
        fencePoints.Add(secondPoint);
    }
}
