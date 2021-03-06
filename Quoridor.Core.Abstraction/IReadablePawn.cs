using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction
{
    public interface IReadablePawn
    {
        public string Name { get; }
        public int NumberOfFences { get; }
        public PawnColor Color { get; }
    }
}
