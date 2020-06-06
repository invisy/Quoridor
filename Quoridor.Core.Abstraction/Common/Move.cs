using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Core.Abstraction.Common
{
    public class Move
    {
        MoveType MoveType { get; }
        PawnColor Color { get; }
        FenceDirection? FenceDirection { get; } = null;
        Point? FencePosition { get; } = null;
        Point? PlayerPosition { get; } = null;

        public Move(Point position, FenceDirection direction, PawnColor color)
        {
            MoveType = MoveType.PlaceFence;
            FencePosition = position;
            FenceDirection = direction;
            Color = color;
        }

        public Move(Point position, PawnColor color, bool isJump = false)
        {
            if (isJump)
                MoveType = MoveType.Jump;
            else
                MoveType = MoveType.Step;
            PlayerPosition = position;
            Color = color;
        }
    }
}
