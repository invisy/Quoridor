using Quoridor.Core.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Quoridor.MVC
{
    class Controller
    {
        private View _view;

        public Controller()
        {
            _view = new ();
        }

        public void Start()
        {
            Board board = new Board(9);
            _view.DrawBoard(board);
        }
    }
}
