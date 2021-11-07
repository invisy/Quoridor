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

        private IPawn? _winner;
        private LinkedListNode<IPawn> _currentPlayer;

        private IPathFinder _pathFinder;
        private IStepsProvider _stepsProvider;

        public event Action? GameStarted;
        public event Action<Move>? BoardUpdated;
        public event Action? GameEnded;

        public IReadableBoard Board => (IReadableBoard)_board.Clone();
        public IReadablePawn CurrentPlayer => _currentPlayer.Value;
        public IReadablePawn? Winner => _winner;

        public Stack<Move> MoveHistory { get; } = new Stack<Move>();

        public GameEngine(IBoard board, IPathFinder pathFinder, IStepsProvider stepsProvider)
        {
            _board = board;
            _pathFinder = pathFinder;
            _stepsProvider = stepsProvider;
            
            foreach(var pawn in _board.GetPawns())
                _playerPawns.AddLast((IPawn)pawn);
            _currentPlayer = _playerPawns.First;
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

        private void SwitchPlayer()
        {
            BoardUpdated?.Invoke(MoveHistory.Peek());
            _currentPlayer = _currentPlayer.Next ?? _playerPawns.First;

            if (_winner == null && _currentPlayer.Value is IBotPawn)
            {
                IBotPawn bot = (IBotPawn)_currentPlayer.Value;
                bot.Run(this);
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
                        PathFinderResult pathFinderResult = _pathFinder.PathExistsToAnyWinPoint(Board, pawn);
                        if (!pathFinderResult.PathExists)
                        {
                            _board.TrySetPawn(_currentPlayer.Value, oldPosition);
                            return false;
                        }
                    }

                    PathFinderResult pathFinderResultForCurrent = _pathFinder.PathExistsToAnyWinPoint(Board, CurrentPlayer);
                    if (pathFinderResultForCurrent.PathLength == 0)
                    {
                        _winner = _currentPlayer.Value;
                        GameEnded?.Invoke();
                    }

                    MoveHistory.Push(new Move(position, CurrentPlayer, isJump));
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
                    PathFinderResult pathFinderResult = _pathFinder.PathExistsToAnyWinPoint(Board, pawn);
                    if (!pathFinderResult.PathExists)
                    {
                        _board.RemoveFenceIfExists(position);
                        return false;
                    }
                }

                _currentPlayer.Value.TryTakeFence();
                MoveHistory.Push(new Move(position, direction, CurrentPlayer));
                SwitchPlayer();
                return true;
            }

            return false;
        }
    }
}