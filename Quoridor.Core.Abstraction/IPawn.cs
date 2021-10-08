using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction;

public interface IPawn : IReadablePawn
{
    public bool IsOutOfBoard { get; }
    Point Position {get; set;}
    bool TryTakeFence();
    void EnableInput();
    void DisableInput();
}
