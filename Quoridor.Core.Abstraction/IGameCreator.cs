namespace Quoridor.Core.Abstraction
{
    public enum PlayerColor
    {
        White,
        Black
    }

    public interface IGameCreator
    {
        IGameEngine Create();
    }
}
