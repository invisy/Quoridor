using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using Quoridor.Core.Implementation;

using Quoridor.MVC.Utilites;
using System;

namespace Quoridor.MVC
{
    public class View
    {
        private char _tile = '.';
        private char _player = 'x';
        private char _fenceHorizontal = '—';
        private char _fenceVertical = '|';
        private char _passage = '░';
        private char[,] _blankBoard;
        private string axisX = " 0 1 2 3 4 5 6 7 8";

        public View()
        {

        }

        public void DrawBoard(IReadableBoard board)
        {
            int size = board.Tiles.GetLength(0);
            PrepareBoard(size.AdaptForTile() - 1);
            PutPlayers(board);
            PutFences(board);

            string[] boardToDraw = _blankBoard.ToStringArray();

            Console.WriteLine(axisX);
            int i = 0;
            foreach (string el in boardToDraw)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine((i / 2).ToString() + el);
                }
                i++;
            }
        }

        private void PrepareBoard(int size)
        {
            _blankBoard = new char[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (y % 2 == 0 && x % 2 == 0)
                    {
                        _blankBoard[x, y] = _tile;
                    }
                    else
                    {
                        _blankBoard[x, y] = _passage;
                    }
                }
            }
        }

        private void PutPlayers(IReadableBoard board)
        {
            foreach (var el in board.Tiles)
            {
                if (el is Pawn)
                {
                    _blankBoard[el.Position.X.AdaptForTile(), el.Position.Y.AdaptForTile()] = _player;
                }
            }
        }

        private void PutFences(IReadableBoard board)
        {
            for (int y = 0; y < board.FenceCrossroads.GetLength(1); y++)
            {
                for (int x = 0; x < board.FenceCrossroads.GetLength(0); x++)
                {
                    if (board.FenceCrossroads[x, y] != null)
                    {
                        int absoluteX = x.AdaptForFence();
                        int absoluteY = y.AdaptForFence();
                        if (board.FenceCrossroads[x, y].Direction == FenceDirection.HORIZONTAL)
                        {
                            PutHorizontalFence(absoluteX, absoluteY);
                        }
                        else
                        {
                            PutVerticalFence(absoluteX, absoluteY);
                        }
                    }
                }
            }
        }

        private void PutHorizontalFence(int x, int y)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                _blankBoard[i, y] = _fenceHorizontal;
            }
        }

        private void PutVerticalFence(int x, int y)
        {
            for (int i = y - 1; i <= y + 1; i++)
            {
                _blankBoard[x, i] = _fenceVertical;
            }
        }
    }
}
