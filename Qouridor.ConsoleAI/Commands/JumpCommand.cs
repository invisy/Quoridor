using System;

namespace Qouridor.ConsoleAI.Commands
{
    public class JumpCommand : ICommand
    {
        Action<MoveHorizontalNotation, int> _action;
        MoveHorizontalNotation X { get; }
        int Y { get; }

        public JumpCommand(MoveHorizontalNotation x, int y, Action<MoveHorizontalNotation, int> action)
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
