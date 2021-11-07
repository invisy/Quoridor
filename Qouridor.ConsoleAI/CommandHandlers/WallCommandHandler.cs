using Qouridor.ConsoleAI.Commands;
using Quoridor.Core.Abstraction.Common;
using System;

namespace Qouridor.ConsoleAI.CommandHandlers
{
    public class WallCommandHandler : ICommandHandler
    {
        Action<WallHorizontalNotation, int, FenceDirection> _action;

        public WallCommandHandler(Action<WallHorizontalNotation, int, FenceDirection> action)
        {
            _action = action;
        }

        public ICommand IdentifyCommand(string command)
        {
            if (command.Length > 0)
            {
                string[] splittedCommand = command.Split(" ");
                if (splittedCommand.Length == 2 && splittedCommand[0] == "wall" && splittedCommand[1].Length == 3)
                {
                    char[] coordinates = splittedCommand[1].ToCharArray();
                    bool xParseStatus = Enum.TryParse<WallHorizontalNotation>(coordinates[0].ToString(), out var x);
                    bool yParseStatus = int.TryParse(coordinates[1].ToString(), out int y);
                    bool directionIsValid = coordinates[2] == 'h' || coordinates[2] == 'v';
                    FenceDirection direction = coordinates[2] == 'h' ? FenceDirection.Horizontal : FenceDirection.Vertical;

                    if (xParseStatus && yParseStatus && y >= 1 && y <= 8 & directionIsValid)
                        return new WallCommand(x, y, direction, _action);
                }
            }

            return null;
        }
    }
}
