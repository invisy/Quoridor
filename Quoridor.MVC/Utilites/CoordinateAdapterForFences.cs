using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.MVC.Utilites
{
    public static class CoordinateAdapterForFences
    {
        public static int AdaptForFence(this int coordinate)
        {
            return coordinate * 2 + 1;
        }
    }
}
