using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomokuConsoleProject
{
    public class Board
    {

        public char[,] board;

        public Board()
        {
            Clear();
        }

        /// <summary>
        /// Возврат элемента по заданной координате
        /// </summary>
        /// <param name="x">Строка</param>
        /// <param name="y">Столбец</param>
        /// <returns></returns>
        public char GetElement(int x, int y)
        {
            if (x < 0 || x > 14 || y < 0 || y > 14)
            {
                return '\0';
            }

            return board[x, y];
        }

        /// <summary>
        /// Вывод массива в консоль
        /// </summary>
        public void DrawBoard()
        {
            for (int i = 0; i < 15; i++)
            {

                Console.Write("{0} \t", i);
                for (int j = 0; j < 15; j++)
                {
                    Console.Write(" {0}", board[i, j]);
                }

                Console.WriteLine("");
            }
        }

        /// <summary>
        /// Инициализирует пустые поля в доску
        /// </summary>
        public void Clear()
        {
            this.board = new char[15, 15];

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    board[i, j] = '_';
                }
            }
        }
    }

    /// <summary>
    /// Координаты доски 
    /// </summary>
    public struct Index
    {
        public Index(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
