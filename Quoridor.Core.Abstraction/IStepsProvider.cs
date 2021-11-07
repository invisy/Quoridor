using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Abstraction
{
    public interface IStepsProvider
    {
        List<Point> GetPossibleSteps(IReadableBoard board, Point startPoint);
        List<Point> GetPossibleJumps(IReadableBoard board, Point startPoint);
    }
}
