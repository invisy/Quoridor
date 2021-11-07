using Quoridor.MVC.Structures;

namespace Quoridor.MVC.Extensions
{
    public static class WrongCommandReasonExtensions
    {
        public static string GetMessage(this WrongCommandReason reason)
        {
            return reason switch
            {
                WrongCommandReason.IsNullOrEmpty => "Empty command was provided.",
                WrongCommandReason.CommandNotFound => "Provided command is not allowed.",
                WrongCommandReason.InvalidArguments => "Provided arguments are invalid.",
                WrongCommandReason.UnableToPutFence => "Unable to put fence with such coordinates.",
                WrongCommandReason.UnableToMovePawn => "Unable to move pawn to such coordinates.",
                _ => string.Empty,
            };
        }
    }
}