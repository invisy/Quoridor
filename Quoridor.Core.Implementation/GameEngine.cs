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

        public event Action? GameStarted;
        public event Action? BoardUpdated;
        public event Action? GameEnded;

        public IReadableBoard Board => _board;
        public IReadablePawn CurrentPlayer => _currentPlayer.Value;
        public IReadOnlyList<IReadablePawn> AllPlayers => _playerPawns.ToList<IReadablePawn>().AsReadOnly();
        public IReadablePawn? Winner => _winner;

        public GameEngine(IBoard board, IPathFinder pathFinder, IStepValidator stepValidator, IPawn player1, IPawn player2)
        {
            _board = board;
            _pathFinder = pathFinder;
            _stepValidator = stepValidator;
            InitializeTwoPlayers(player1, player2);
        }

        public GameEngine(IBoard board, IPathFinder pathFinder, IStepValidator stepValidator, IPawn player1, IPawn player2, IPawn player3, IPawn player4)
        {
            _board = board;
            _pathFinder = pathFinder;
            _stepValidator = stepValidator;
            InitializeFourPlayers(player1, player2, player3, player4);
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

            _board.TrySetPawn(player1, new Point(center, 0));
            _winPoints.Add(player1, GenerateWinPoints(new Point(0, max), new Point(max, max)));
            _playerPawns.AddLast(player1);
            _board.TrySetPawn(player2, new Point(center, _board.Tiles.GetLength(0) - 1));
            _winPoints.Add(player2, GenerateWinPoints(new Point(0, 0), new Point(max, 0)));
            _playerPawns.AddLast(player2);

            _currentPlayer = _playerPawns.Last;
        }

        private void InitializeFourPlayers(IPawn player1, IPawn player2, IPawn player3, IPawn player4)
        {
            int center = _board.Tiles.GetLength(0) / 2;
            int max = _board.Tiles.GetLength(0) - 1;

            _board.TrySetPawn(player1, new Point(center, 0));
            _winPoints.Add(player1, GenerateWinPoints(new Point(0, max), new Point(max, max)));
            _playerPawns.AddLast(player1);

            _board.TrySetPawn(player2, new Point(max, center));
            _winPoints.Add(player2, GenerateWinPoints(new Point(0, 0), new Point(0, max)));
            _playerPawns.AddLast(player2);

            _board.TrySetPawn(player3, new Point(center, _board.Tiles.GetLength(0) - 1));
            _winPoints.Add(player3, GenerateWinPoints(new Point(0, 0), new Point(max, 0)));
            _playerPawns.AddLast(player3);

            _board.TrySetPawn(player4, new Point(0, center));
            _winPoints.Add(player4, GenerateWinPoints(new Point(max, 0), new Point(max, max)));
            _playerPawns.AddLast(player4);

            _currentPlayer = _playerPawns.First;
        }

        private void SwitchPlayer()
        {
            _currentPlayer = _currentPlayer.Next ?? _playerPawns.First;
            BoardUpdated?.Invoke();

            if (_currentPlayer.Value is IBotPawn)
            {
                IBotPawn bot = (IBotPawn)_currentPlayer.Value;
                bot.Run(this);
                BoardUpdated?.Invoke();
            }
        }

        public bool TryMovePawn(Point position)
        {
            if (_winner != null)
                return false;

            Point oldPosition = _currentPlayer.Value.Position;
            if (_stepValidator.GetPossibleSteps(_board, _currentPlayer.Value.Position).Find(x => x.Equals(position)) != null)
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

                    if (_winPoints[_currentPlayer.Value].Find(x => x.Equals(position)) != null)
                    {
                        _winner = _currentPlayer.Value;
                        GameEnded?.Invoke();
                    }

                    SwitchPlayer();

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
                    if (!_pathFinder.PathExistsToAny(pawn.Position, _winPoints[pawn]))
                    {
                        _board.RemoveFenceIfExists(position);
                        return false;
                    }
                }

                _currentPlayer.Value.TryTakeFence();
                SwitchPlayer();

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