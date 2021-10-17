using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Core.Abstraction
{
    public interface IBotPawn : IPawn
    {
        void Run(IGameEngine gameEngine);
    }
}
