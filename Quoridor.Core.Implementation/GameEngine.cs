using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quoridor.Core.Implementation
{
    public class GameEngine : IGameEngine
    {
        private readonly IBoard _board;
        private readonly LinkedList<IPawn> _playerPawns = new();
        private readonly Dictionary<IPawn, List<Point>> _winPoints = new();

        private IPawn? _winner;
        private LinkedListNode<IPawn> _currentPlayer;

        private IPathFinder _pathFinder;
        private IStepsProvider _stepsProvider;

        public event Action? GameStarted;
        public event Action? BoardUpdated;
        public event Action? GameEnded;

        public IReadableBoard Board => (IReadableBoard)_board.Clone();
        public IReadablePawn CurrentPlayer => _currentPlayer.Value;
        public IReadOnlyList<IReadablePawn> AllPlayers => _playerPawns.ToList<IReadablePawn>().AsReadOnly();
        public IReadablePawn? Winner => _winner;

        public Stack<Move> MoveHistory { get; } = new Stack<Move>();

        public GameEngine(IBoard board, IPathFinder pathFinder, IStepsProvider stepsProvider, IPawn player1, IPawn player2)
        {
            _board = board;
            _pathFinder = pathFinder;
            _stepsProvider = stepsProvider;
            InitializeTwoPlayers(player1, player2);
        }

        public IEnumerable<Point> GetWinPointsForPlayer(IPawn pawn)
        {
            return _winPoints[pawn].AsReadOnly();
        }

        public void Start()
        {
            GameStarted?.Invoke();
            if (_currentPlayer.Value is IBotPawn)
            {
                IBotPawn bot = (IBotPawn)_currentPlayer.Value;
                bot.Run(this);
            }
        }

        private void InitializeTwoPlayers(IPawn player1, IPawn player2)
        {
            int center = _board.Tiles.GetLength(0) / 2;
            int max = _board.Tiles.GetLength(0) - 1;

            if(player1.Color == PawnColor.Black && player2.Color == PawnColor.White)
            {
                IPawn tmp = player1;
                player1 = player2;
                player2 = tmp;
            }

            _board.TrySetPawn(player1, new Point(center, _board.Tiles.GetLength(0) - 1));
            _winPoints.Add(player1, GenerateWinPoints(new Point(0, 0), new Point(max, 0)));
            _playerPawns.AddLast(player1);
            _board.TrySetPawn(player2, new Point(center, 0));
            _winPoints.Add(player2, GenerateWinPoints(new Point(0, max), new Point(max, max)));
            _playerPawns.AddLast(player2);

            _currentPlayer = _playerPawns.First;
        }

        private void SwitchPlayer()
        {
            _currentPlayer = _currentPlayer.Next ?? _playerPawns.First;
            BoardUpdated?.Invoke();

            if (_winner == null && _currentPlayer.Value is IBotPawn)
            {
                IBotPawn bot = (IBotPawn)_currentPlayer.Value;
                bot.Run(this);
                BoardUpdated?.Invoke();
            }
        }

        public bool TryMovePawn(Point position, bool isJump)
        {
            if (_winner != null)
                return false;

            Point oldPosition = _board.GetPawnPosition(_currentPlayer.Value);
            bool moveIsPossible = false;
            if (isJump)
                moveIsPossible = _stepsProvider.GetPossibleJumps(Board, oldPosition).Where(x => x.Equals(position)).Cast<Point?>().FirstOrDefault() != null;
            else
                moveIsPossible = _stepsProvider.GetPossibleSteps(Board, oldPosition).Where(x => x.Equals(position)).Cast<Point?>().FirstOrDefault() != null;

            if (moveIsPossible)
            {
                if (_board.TrySetPawn(_currentPlayer.Value, position))
                {
                    foreach (IPawn pawn in _playerPawns)
                    {
                        if (!_pathFinder.PathExistsToAny(Board, _board.GetPawnPosition(pawn), _winPoints[pawn]))
                        {
                            _board.TrySetPawn(_currentPlayer.Value, oldPosition);
                            return false;
                        }
                    }

                    if (_winPoints[_currentPlayer.Value].Where(x => x.Equals(position)).Cast<Point?>().FirstOrDefault() != null)
                    {
                        _winner = _currentPlayer.Value;
                        GameEnded?.Invoke();
                    }
                    SwitchPlayer();
                    MoveHistory.Push(new Move(position, CurrentPlayer, isJump));
                    return true;
                }
            }

            return false;
        }

        public bool TryPlaceFence(Point position, FenceDirection direction)
        {
            if (_winner != null)
                return false;

            if (_currentPlayer.Value.NumberOfFences == 0)
                return false;

            if (_board.TryPutFence(position, direction))
            {
                foreach (IPawn pawn in _playerPawns)
                {
                    if (!_pathFinder.PathExistsToAny(Board, _board.GetPawnPosition(pawn), _winPoints[pawn]))
                    {
                        _board.RemoveFenceIfExists(position);
                        return false;
                    }
                }

                _currentPlayer.Value.TryTakeFence();
                SwitchPlayer();
                MoveHistory.Push(new Move(position, direction, CurrentPlayer));
                return true;
            }

            return false;
        }

        private List<Point> GenerateWinPoints(Point start, Point end)
        {
            List<Point> result = new List<Point>();
            if (start.X == end.X)
                for (int i = start.Y; i <= end.Y; i++)
                    result.Add(new Point(start.X, i));
            else if (start.Y == end.Y)
                for (int i = start.X; i <= end.X; i++)
                    result.Add(new Point(i, start.Y));
            return result;
        }
    }
}