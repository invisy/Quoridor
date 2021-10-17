using Quoridor.Core.Abstraction;
using System;
using System.Collections.Generic;

namespace Quoridor.MVC.Views
{
    class GameStateView
    {
        public void DrawState(IReadOnlyList<IReadablePawn> pawnList, IReadablePawn currentPlayer)
        {
            string state = "";

            foreach (var player in pawnList)
            {
                state += player.Name + " has " + player.NumberOfFences.ToString() + " fences" + "\t\t";
            }

            state += "Current turn: " + currentPlayer.Name;
            Console.WriteLine(state);
        }
    }
}
