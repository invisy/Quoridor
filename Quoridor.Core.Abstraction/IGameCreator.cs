using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction
{
    public interface IGameCreator
    {
        IGameEngine Create(PawnColor firstPlayerColor = PawnColor.White);
    }
}
