using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomokuConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Game game = new Game(board);
            game.Start();               // Запускаем игру. Игра начинается с рандомной позиций
            Console.ReadLine();
        }
    }
}
