using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Core.Implementation.Models;

public class BotPawn : Pawn
{
    private static int _botNumber = 1;

    public BotPawn(int numberOfFences) : base($"Bot {_botNumber}", numberOfFences)
    {
        _botNumber++;
    }
}