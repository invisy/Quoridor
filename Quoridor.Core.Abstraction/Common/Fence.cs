namespace Quoridor.Core.Abstraction.Common
{
    public class Fence
    {
        public FenceDirection Direction { get; }

        public Fence(FenceDirection direction)
        {
            Direction = direction;
        }
    }
}