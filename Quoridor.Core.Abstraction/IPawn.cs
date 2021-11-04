using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction
{
    public interface IPawn : IReadablePawn
    {
        bool TryTakeFence();
    }
}
