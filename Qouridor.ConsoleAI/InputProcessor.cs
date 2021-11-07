using Qouridor.ConsoleAI.CommandHandlers;
using Qouridor.ConsoleAI.Commands;
using System;
using System.Collections.Generic;

namespace Qouridor.ConsoleAI
{
    public class InputProcessor
    {
        private readonly List<ICommandHandler> _handlers = new();

        public InputProcessor Add(ICommandHandler handler)
        {
            _handlers.Add(handler);
            return this;
        }

        public bool Process()
        {
            string inputCommand = Console.ReadLine();

            foreach (ICommandHandler handler in _handlers)
            {
                ICommand command = handler.IdentifyCommand(inputCommand);
                if (command == null)
                    continue;

                command.Execute();
                return true;
            }

            return false;
        }
    }
}
