namespace Qouridor.Server.Structures;

public enum WrongCommandReason
{
    IsNullOrEmpty,
    CommandNotFound,
    InvalidArguments,
    UnableToPutFence,
    UnableToMovePawn
}