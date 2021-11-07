using Qouridor.ConsoleAI.Commands;
using Quoridor.Core.Abstraction.Common;
using System;

namespace Qouridor.ConsoleAI.CommandHandlers
{
    public class ColorCommandHandler : ICommandHandler
    {
        Action<PawnColor> _action;

        public ColorCommandHandler(Action<PawnColor> action)
        {
            _action = action;
        }

        public ICommand IdentifyCommand(string command)
        {
            if (command == "white")
                return new ColorCommand(PawnColor.White, _action);
            else if (command == "black")
                return new ColorCommand(PawnColor.Black, _action);

            return null;
        }
    }
}
