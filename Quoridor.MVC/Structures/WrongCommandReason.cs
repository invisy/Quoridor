namespace Quoridor.MVC.Structures
{
    public enum WrongCommandReason
    {
        IsNullOrEmpty,
        CommandNotFound,
        InvalidArguments,
        UnableToPutFence,
        UnableToMovePawn
    }
}