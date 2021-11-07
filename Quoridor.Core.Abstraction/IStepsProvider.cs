using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Abstraction
{
    public interface IStepsProvider
    {
        List<Point> GetPossibleSteps(IReadableBoard board, IReadablePawn pawn);
        List<Point> GetPossibleJumps(IReadableBoard board, IReadablePawn pawn);
    }
}
