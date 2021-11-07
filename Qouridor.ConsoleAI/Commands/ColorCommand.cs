using Quoridor.Core.Abstraction.Common;
using System;

namespace Qouridor.ConsoleAI.Commands
{
    public class ColorCommand : ICommand
    {
        Action<PawnColor> _action;
        PawnColor Color { get; set; }
        public ColorCommand(PawnColor color, Action<PawnColor> action)
        {
            Color = color;
            _action = action;
        }

        public void Execute()
        {
            _action?.Invoke(Color);
        }
    }
}
