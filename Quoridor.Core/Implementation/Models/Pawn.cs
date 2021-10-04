using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Core.Implementation.Models;

public abstract class Pawn
{
    public string Name { get; private set; }
    public int NumberOfFences { get; private set; }

    public Pawn(string name, int numberOfFences)
    {
        Name = name;
        if (numberOfFences > 0)
            NumberOfFences = numberOfFences;
        else
            throw new ArgumentOutOfRangeException("Number of fences must be positive!");
    }

    internal void TakeFence()
    {
        if(NumberOfFences > 0)
            NumberOfFences--;
        else
            throw new Exception("Number of fences must be positive!"); //FIXME
    }
}
