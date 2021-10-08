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

        public void DrawBoard(Board board)
        {
            int size = board.Tiles.GetLength(0);
            char[,] blankBoard = prepareBoard(size);

            foreach (var element in board.Tiles)
            {
                if(element is Pawn)
                {
                    blankBoard[element.Position.X, element.Position.Y] = _player;
                }
            }

            foreach (var element in board.Fences)
            {
                //ToDo
            }

            string[] boardToDraw = blankBoard.ToStringArray();

            Clear();
            foreach(string el in boardToDraw)
            {
                Console.WriteLine(el);
            }
        }

        private void drawBoard()
        {

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
