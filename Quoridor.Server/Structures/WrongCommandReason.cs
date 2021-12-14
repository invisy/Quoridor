namespace Quoridor.Server.Structures;

enum WrongCommandReason
{
    IsNullOrEmpty,
    CommandNotFound,
    InvalidArguments,
    UnableToPutFence,
    UnableToMovePawn
}