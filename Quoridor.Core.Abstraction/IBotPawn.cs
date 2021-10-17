namespace Quoridor.Core.Abstraction
{
    public interface IBotPawn : IPawn
    {
        void Run(IGameEngine gameEngine);
    }
}
