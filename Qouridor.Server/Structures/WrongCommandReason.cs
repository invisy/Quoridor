namespace Qouridor.Server.Structures;

enum WrongCommandReason
{
    IsNullOrEmpty,
    CommandNotFound,
    InvalidArguments,
    UnableToPutFence,
    UnableToMovePawn
}