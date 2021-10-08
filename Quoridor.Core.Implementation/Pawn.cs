using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Implementation;

public class Pawn : IPawn
{
    private IPlayerController PlayerController { get; }

    public string Name { get; }
    public int NumberOfFences { get; private set; }
    public Point Position { get; set; } = new Point(-1, -1);

    public bool IsOutOfBoard => Position.X == -1 && Position.Y == -1;

    public Pawn(string name, int numberOfFences, IPlayerController playerController)
    {
        Name = name;
        PlayerController = playerController;
        if (numberOfFences > 0)
            NumberOfFences = numberOfFences;
        else
            throw new ArgumentOutOfRangeException("Number of fences must be positive!");
    }

    public bool TryTakeFence()
    {
        if (NumberOfFences == 0)
            return false;

        NumberOfFences--;
        return true;
    }

    public void EnableInput()
    {
        PlayerController.Enable();
    }

    public void DisableInput()
    {
        PlayerController.Disable();
    }
}
