using Qouridor.ConsoleAI.Commands;
using Quoridor.Core.Abstraction.Common;
using System;

namespace Qouridor.ConsoleAI.CommandHandlers
{
    public class JumpCommandHandler : ICommandHandler
    {
        Action<MoveHorizontalNotation, int> _action;

        public JumpCommandHandler(Action<MoveHorizontalNotation, int> action)
        {
            _action = action;
        }

        public ICommand IdentifyCommand(string command)
        {
            if (command.Length > 0)
            {
                string[] splittedCommand = command.Split(" ");
                if (splittedCommand.Length == 2 && splittedCommand[0] == "jump" && splittedCommand[1].Length == 2)
                {
                    char[] coordinates = splittedCommand[1].ToCharArray();
                    bool xParseStatus = Enum.TryParse<MoveHorizontalNotation>(coordinates[0].ToString(), out var x);
                    bool yParseStatus = int.TryParse(coordinates[1].ToString(), out int y);

                    if (xParseStatus && yParseStatus && y >= 1 && y <= 9)
                        return new JumpCommand(x, y, _action);
                }
            }

            return null;
        }
    }
}
