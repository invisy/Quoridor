namespace Quoridor.Server.Utilities;

static class CoordinateAdapterForTiles
{
    public static int AdaptForTile(this int coordinate)
    {
        return coordinate * 2;
    }
}