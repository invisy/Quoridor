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
        private IStepValidator _stepValidator;

        public event Action GameStarted;
        public event Action BoardUpdated;
        public event Action GameEnded;

        public IReadableBoard Board => _board;
        public IReadablePawn CurrentPlayer => _currentPlayer.Value;
        public IReadOnlyList<IReadablePawn> AllPlayers => _playerPawns.ToList<IReadablePawn>().AsReadOnly();
        public IReadablePawn? Winner => _winner;

        public GameEngine(IBoard board, IPathFinder pathFinder, IStepValidator stepValidator)
        {
            _board = board;
            _pathFinder = pathFinder;
            _stepValidator = stepValidator;
        }

        public void AddPlayer(IPawn pawn)
        {
            int center = _board.Tiles.GetLength(0) / 2 + 1;
            int max = _board.Tiles.GetLength(0) - 1;
            switch (_playerPawns.Count)
            {
                case 0:
                    _board.TrySetPawn(pawn, new Point(center, 0));
                    _winPoints.Add(pawn, GenerateWinPoints(new Point(0, 0), new Point(max, 0)));
                    _playerPawns.AddLast(pawn);
                    break;
                case 1:
                    _board.TrySetPawn(pawn, new Point(center, _board.Tiles.GetLength(0) - 1));
                    _winPoints.Add(pawn, GenerateWinPoints(new Point(0, max), new Point(max, max)));
                    _playerPawns.AddLast(pawn);
                    break;
                case 2:
                    _board.TrySetPawn(pawn, new Point(max, center));
                    _winPoints.Add(pawn, GenerateWinPoints(new Point(max, 0), new Point(max, max)));
                    _playerPawns.AddBefore(_playerPawns.Last, pawn);    // If there are 4 players we must play clockwise
                    break;
                case 3:
                    _board.TrySetPawn(pawn, new Point(0, center));
                    _winPoints.Add(pawn, GenerateWinPoints(new Point(0, 0), new Point(0, max)));
                    _playerPawns.AddLast(pawn);
                    break;
                case 4:
                    throw new Exception("You can add only 4 players!");
            }
        }

        public void Start()
        {
            if (_playerPawns.Count != 2 || _playerPawns.Count != 4)
                throw new Exception("Only 2 or 4 players can start game!");

            if (_currentPlayer != null)
                throw new Exception("Game already has started!");

            _currentPlayer = _playerPawns.First;
            _currentPlayer.Value.EnableInput();
            GameStarted?.Invoke();
        }

        private void SwitchPlayer()
        {
            _currentPlayer.Value.DisableInput();
            _currentPlayer = _currentPlayer.Next ?? _playerPawns.First;
            _currentPlayer.Value.EnableInput();
        }

        public bool TryMovePawn(Point position)
        {
            Point oldPosition = _currentPlayer.Value.Position;
            if (_stepValidator.GetPossibleSteps(_board, _currentPlayer.Value.Position).Where(x => x.Equals(position)).FirstOrDefault() != null)
            {
                if (_board.TrySetPawn(_currentPlayer.Value, position))
                {
                    foreach (IPawn pawn in _playerPawns)
                    {
                        if (!_pathFinder.PathExistsToAny(pawn.Position, _winPoints[pawn]))
                        {
                            _board.TrySetPawn(_currentPlayer.Value, oldPosition);
                            return false;
                        }
                    }

                    if (_winPoints[_currentPlayer.Value].Where(x => x.Equals(position)).FirstOrDefault() != null)
                    {
                        _winner = _currentPlayer.Value;
                        GameEnded?.Invoke();
                    }
                        
                    SwitchPlayer();
                    BoardUpdated?.Invoke();

                    return true;
                }
            }

            return false;
        }

        public bool TryPlaceFence(Point position, FenceDirection direction)
        {
            if (_board.TryPutFence(position, direction))
            {
                foreach (IPawn pawn in _playerPawns)
                {
                    if (!_pathFinder.PathExistsToAny(pawn.Position, _winPoints[pawn]))
                    {
                        _board.RemoveFenceIfExists(position);
                        return false;
                    }
                }
                SwitchPlayer();
                BoardUpdated?.Invoke();

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