using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction;

public interface IPawn : IReadablePawn
{
    Point Position {get; set;}
    void TakeFence();
    void EnableInput();
    void DisableInput();
}
