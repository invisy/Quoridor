using System;
using Quoridor.MVC.Controllers;

namespace Quoridor.MVC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            GameController gameController = new GameController();
            gameController.Start();
        }
    }
}
