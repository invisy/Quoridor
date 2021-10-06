using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Input;

public class ConsolePlayerController : IPlayerController
{
    IGameEngine _game;
    ConsoleInputProcessor _inputProcessor;

    public ConsolePlayerController(IGameEngine game, ConsoleInputProcessor inputProcessor)
    {
        _game = game;
        _inputProcessor = inputProcessor;
    }

    public void Enable()
    {
        _inputProcessor.MovePlayerEvent += OnMovePlayer;
        _inputProcessor.PlaceWallEvent += OnPlaceWall;
    }
    public void Disable()
    {
        _inputProcessor.MovePlayerEvent -= OnMovePlayer;
        _inputProcessor.PlaceWallEvent -= OnPlaceWall;
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