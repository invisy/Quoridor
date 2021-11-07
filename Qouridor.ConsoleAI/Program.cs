using Quoridor.Core.Abstraction;
using Quoridor.Core.Implementation;
using System;

namespace Qouridor.ConsoleAI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IGameCreator gameEngine = new PlayerVsBotGameCreator();
            GameController controller = new GameController(gameEngine);

            Console.ReadLine();
        }
    }
}
