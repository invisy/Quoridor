using Quoridor.Core.Abstraction.Common;
using System.Collections.Generic;

namespace Quoridor.Core.Abstraction
{
    public interface IStepValidator
    {
        List<Point> GetPossibleSteps(IBoard board, Point startPoint);
    }
}
