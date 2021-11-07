using System;

namespace Qouridor.ConsoleAI.Commands
{
    public class MoveCommand : ICommand
    {
        Action<MoveHorizontalNotation, int> _action;
        MoveHorizontalNotation X { get; }
        int Y { get; }

        public MoveCommand(MoveHorizontalNotation x, int y, Action<MoveHorizontalNotation, int> action)
        {
            X = x;
            Y = y;
            _action = action;
        }

        public void Execute()
        {
            _action?.Invoke(X, Y);
        }
    }
}
