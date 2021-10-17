using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using Quoridor.Core.Implementation;
using System;

namespace Quoridor.MVC
{
    class Controller
    {
        private View _view;

        public Controller()
        {
            _view = new();
        }

        public void Start()
        {
            IGameCreator game = new TwoPlayersGameCreator();
            IGameEngine engine = game.Create();
            _view.DrawBoard(engine.Board);

            engine.TryPlaceFence(new Point(0, 3), FenceDirection.HORIZONTAL);
            engine.TryPlaceFence(new Point(2, 3), FenceDirection.HORIZONTAL);
            engine.TryPlaceFence(new Point(4, 3), FenceDirection.HORIZONTAL);
            engine.TryPlaceFence(new Point(6, 3), FenceDirection.HORIZONTAL);
            engine.TryPlaceFence(new Point(7, 3), FenceDirection.VERTICAL);
            engine.TryPlaceFence(new Point(7, 2), FenceDirection.HORIZONTAL);


            Console.Clear();
            _view.DrawBoard(engine.Board);
        }
    }
}
