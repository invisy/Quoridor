using System;

namespace Quoridor.MVC.Views
{
    class MenuView
    {
        public void DrawMenu()
        {
            Console.WriteLine("Welcome to Quoridor game!");
            Console.WriteLine("1. To start new game with computer type 'start pve'");
            Console.WriteLine("2. To start new game with second player type 'start pvp'");
            Console.WriteLine("In game commands:");
            Console.WriteLine("1. To make move type 'movepawn X Y'");
            Console.WriteLine("2. To place wall type 'placewall X Y (h/v)'");
            Console.WriteLine("3. To end game and go to menu type 'end'");
        }
    }
}
