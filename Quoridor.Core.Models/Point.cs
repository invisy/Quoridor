using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Core.Models
{
    public class Point
    {
        public int X {  get; private set; }
        public int Y {  get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
