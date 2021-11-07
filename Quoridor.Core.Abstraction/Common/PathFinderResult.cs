namespace Quoridor.Core.Abstraction.Common
{
    public struct PathFinderResult
    {
        public bool PathExists { get; }
        public int PathLength { get; }

        public PathFinderResult(bool pathExists, int pathLength = 0)
        {
            PathExists = pathExists;
            PathLength = pathLength;
        }
    }
}
