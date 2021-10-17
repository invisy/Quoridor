using Quoridor.Core.Abstraction;

namespace Quoridor.Core.Implementation
{
    public class RandomBotPawn : Pawn, IBotPawn
    {
        public RandomBotPawn(string name, int numberOfFences) : base(name, numberOfFences)
        {

        }

        public void Run(IGameEngine gameEngine)
        {
            //Bot logic
        }
    }
}
