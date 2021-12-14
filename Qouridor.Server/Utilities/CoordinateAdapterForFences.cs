namespace Qouridor.Server.Utilities;

static class CoordinateAdapterForFences
{
    public static int AdaptForFence(this int coordinate)
    {
        return coordinate * 2 + 1;
    }
}