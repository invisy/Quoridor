using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Input;

public class BotController : IPlayerController
{
    IGameEngine _game;

    public BotController(IGameEngine game)
    {
        _game = game;
    }

    public void Enable()
    {
        //Ganerate random moves
    }
    public void Disable()
    {

    }

    private void OnMovePlayer(Point point)
    {
        //TODO
    }

    private void OnPlaceWall(Point point, FenceDirection direction)
    {
        //TODO
    }
}