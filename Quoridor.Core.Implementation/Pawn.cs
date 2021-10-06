using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Implementation;

public class Pawn : IPawn
{
    private IPlayerController PlayerController { get; }

    public string Name { get; }
    public int NumberOfFences { get; private set; }
    public Point Position { get; set; }

    public Pawn(string name, int numberOfFences, IPlayerController playerController)
    {
        Name = name;
        PlayerController = playerController;
        if (numberOfFences > 0)
            NumberOfFences = numberOfFences;
        else
            throw new ArgumentOutOfRangeException("Number of fences must be positive!");
    }

    public void TakeFence()
    {
        if (NumberOfFences > 0)
            NumberOfFences--;
        else
            throw new Exception("Number of fences must be positive!"); //FIXME
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
