using Qouridor.ConsoleAI.Commands;

namespace Qouridor.ConsoleAI.CommandHandlers
{
    public interface ICommandHandler
    {
        ICommand IdentifyCommand(string command);
    }
}
