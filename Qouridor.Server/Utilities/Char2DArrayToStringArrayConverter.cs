namespace Quoridor.Server.Utilites
{
    public static class Char2DArrayToStringArrayConverter
    {
        public static String[] ToStringArray(this char[,] charArray)
        {
            string[] result = new string[charArray.GetLength(1)];

            for (int y = 0; y < charArray.GetLength(1); y++)
            {
                result[y] = "";
                for (int x = 0; x < charArray.GetLength(0); x++)
                {
                    result[y] += charArray[x, y];
                }
            }

            return result;
        }
    }
}
