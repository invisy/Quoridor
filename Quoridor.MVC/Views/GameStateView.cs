using Quoridor.Core.Abstraction;
using System;
using System.Collections.Generic;

namespace Quoridor.MVC.Views
{
    class GameStateView
    {
        public void DrawState(IEnumerable<IReadablePawn> pawnList, IReadablePawn currentPlayer)
        {
            string state = "";

            foreach (var player in pawnList)
            {
                state += player.Name + " has " + player.NumberOfFences.ToString();
                state += (player.NumberOfFences == 1 ? " fence" : " fences") + "\t\t";
            }

            state += "Current turn: " + currentPlayer.Name;
            Console.WriteLine(state);
        }
    }
}
