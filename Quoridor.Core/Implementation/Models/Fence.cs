using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Core.Implementation.Models;

public class Fence
{
    public IReadOnlyList<Point> FencePositions { get; private set; } 
    public Fence(List<Point> points)
    {
        FencePositions = points.AsReadOnly();
    }
}
