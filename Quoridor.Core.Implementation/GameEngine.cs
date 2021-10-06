using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Implementation;

public class GameEngine : IGameEngine
{
    private readonly IBoard _board;
    private readonly LinkedList<IPawn> _playerPawns;
    private readonly Dictionary<IPawn, List<Point>> _winPoints = new();

    private LinkedListNode<IPawn> _currentPlayer;

    public event EventHandler GameStarted;
    public event EventHandler BoardUpdated;
    public event EventHandler GameEnded;

    public IReadableBoard Board => _board;
    public IReadablePawn CurrentPlayer => _currentPlayer.Value;
    public IReadOnlyList<IReadablePawn> AllPlayers => _playerPawns.ToList<IReadablePawn>().AsReadOnly();
    public IReadablePawn Winner { get; }

    public GameEngine(IBoard board, List<IPawn> playerPawns)
    {
        _board = board;
        _playerPawns = new LinkedList<IPawn>(playerPawns);
        Start();
    }

    private void Start()
    {
        _currentPlayer = _playerPawns.First;
        _currentPlayer.Value.EnableInput();
    }

    private void SwitchPlayer()
    {
        _currentPlayer.Value.DisableInput();
        _currentPlayer = _currentPlayer.Next ?? _playerPawns.First;
        _currentPlayer.Value.EnableInput();
    }

    public bool TryMovePawn(Point position)
    {
        throw new NotImplementedException(); //TODO
        SwitchPlayer();
    }

    public bool TryPlaceFence(Point position, FenceDirection direction)
    {
        throw new NotImplementedException(); //TODO
        SwitchPlayer();
    }
}