using Quoridor.Core.Abstraction.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qouridor.ConsoleAI
{
    public class MoveParser
    {
        public string Parse(Move move)
        {
            if (move.MoveType == MoveType.Step)
                return $"move {(MoveHorizontalNotation)move.PlayerPosition.X}{move.PlayerPosition.Y + 1}";
            else if (move.MoveType == MoveType.Jump)
                return $"jump {(MoveHorizontalNotation)move.PlayerPosition.X}{move.PlayerPosition.Y + 1}";
            else if (move.MoveType == MoveType.PlaceFence)
            {
                string wallDirection = move.FenceDirection == FenceDirection.Horizontal ? "h" : "v";
                return $"wall {(WallHorizontalNotation)move.FencePosition.X}{move.FencePosition.Y + 1}{wallDirection}";
            }
            
            return string.Empty;
        }
    }
}
