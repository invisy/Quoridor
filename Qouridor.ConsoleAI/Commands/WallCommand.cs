using Quoridor.Core.Abstraction.Common;
using System;

namespace Qouridor.ConsoleAI.Commands
{
    public class WallCommand : ICommand
    {
        private Action<WallHorizontalNotation, int, FenceDirection> _action;
        WallHorizontalNotation X { get; }
        int Y { get; }
        FenceDirection Direction { get; }

        public WallCommand(WallHorizontalNotation x, int y, FenceDirection direction, Action<WallHorizontalNotation, int, FenceDirection> action)
        {
            X = x;
            Y = y;
            Direction = direction;
            _action = action;
        }

        public void Execute()
        {
            _action?.Invoke(X, Y, Direction);
        }
    }
}
