using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction
{
    public interface IPawn : IReadablePawn
    {
        public bool IsOutOfBoard { get; }
        new Point Position { get; set; }
        bool TryTakeFence();
    }
}
