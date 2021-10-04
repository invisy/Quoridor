namespace Quoridor.Core.Models;

public class PlayerPawn : Pawn
{
    private static int _playerNumber = 1;

    public PlayerPawn(string name, int numberOfFences) : base(name, numberOfFences)
    {
        _playerNumber++;
    }

    public PlayerPawn(int numberOfFences) : base($"Player {_playerNumber}", numberOfFences)
    {
        _playerNumber++;
    }
}
