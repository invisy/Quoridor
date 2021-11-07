using System;

namespace Quoridor.MVC.Views
{
    class WinnerView
    {
        public void DrawWinner(string winner)
        {
            Console.WriteLine($"Game ended");
            Console.WriteLine($"Congratulations to {winner}!");
        }
    }
}
