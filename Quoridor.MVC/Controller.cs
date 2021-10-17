using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using Quoridor.Core.Implementation;

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
            engine.TryPlaceFence(new Point(5,5), FenceDirection.HORIZONTAL);
            engine.TryMovePawn(new Point(4, 7));
            engine.TryMovePawn(new Point(4, 1));
            engine.TryMovePawn(new Point(4, 6));
            engine.TryMovePawn(new Point(4, 2));
            engine.TryMovePawn(new Point(5, 6));
            engine.TryMovePawn(new Point(4, 3));
            engine.TryMovePawn(new Point(5, 5));
            _view.DrawBoard(engine.Board);
        }
    }
}
