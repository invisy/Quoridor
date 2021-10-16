using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using Quoridor.Core.Implementation;

using Quoridor.MVC.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.MVC
{
    public class View
    {
        private char _tile = '.';
        private char _player = 'x';
        private char _fenceHorizontal = '—';
        private char _fenceVertical = '|';
        private char _passage = '░';

        public View()
        {
            Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public void Clear()
        {
            Console.Clear();
        }

        //Need to be seperated into methods
        public void DrawBoard(IBoard board)
        {
            int size = board.Tiles.GetLength(0);
            char[,] blankBoard = prepareBoard(size.AdaptForTile()-1);

            foreach (var el in board.Tiles)
            {
                if(el is Pawn)
                {
                    blankBoard[el.Position.X.AdaptForTile(), el.Position.Y.AdaptForTile()] = _player;
                }
            }
            //Need to be refactored
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
                            for (int i = absoluteX - 1; i <= absoluteX + 1; i++)
                            {
                                blankBoard[i, absoluteY] = _fenceHorizontal;
                            } 
                        }
                        else
                        {
                            for (int i = absoluteY - 1; i <= absoluteY + 1; i++)
                            {
                                blankBoard[absoluteX, i] = _fenceVertical;
                            }
                        }
                    }
                }
            }

            string[] boardToDraw = blankBoard.ToStringArray();

            Clear();
            foreach(string el in boardToDraw)
            {
                Console.WriteLine(el);
            }
        }

        private char[,] prepareBoard(int size)
        {
            char[,] board = new char[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (y%2 == 0 && x % 2 == 0)
                    {
                        board[x, y] = _tile;
                    }
                    else
                    {
                        board[x, y] = _passage;
                    }
                }
            }

            return board;
        }
    }
}
