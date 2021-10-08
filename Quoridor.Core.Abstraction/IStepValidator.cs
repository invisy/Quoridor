using Quoridor.Core.Abstraction.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Core.Abstraction
{
    public interface IStepValidator
    {
        List<Point> GetPossibleSteps(IBoard board, Point startPoint);
    }
}
