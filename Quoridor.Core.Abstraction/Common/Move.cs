using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction.Common
{
    public class Move
    {
        public IReadablePawn MoveInitiator { get; }
        public MoveType MoveType { get; }
        public FenceDirection FenceDirection { get; }
        public Point FencePosition { get; }
        public Point PlayerPosition { get; }

        public Move(Point position, FenceDirection direction, IReadablePawn moveInitiator)
        {
            MoveType = MoveType.PlaceFence;
            FencePosition = position;
            FenceDirection = direction;
            MoveInitiator = moveInitiator;
        }

        public Move(Point position, IReadablePawn moveInitiator, bool isJump = false)
        {
            if (isJump)
                MoveType = MoveType.Jump;
            else
                MoveType = MoveType.Step;
            PlayerPosition = position;
            MoveInitiator = moveInitiator;
        }
    }
}
