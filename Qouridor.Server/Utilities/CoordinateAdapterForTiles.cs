namespace Quoridor.Server.Utilites
{
    public static class CoordinateAdapterForTiles
    {
        public static int AdaptForTile(this int coordinate)
        {
            return coordinate * 2;
        }
    }
}
