using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomokuConsoleProject
{
    /// <summary>
    /// Класс в котором определена логика игры.
    /// </summary>
    public class Game
    {
        public Board Board;

        private char playerOnX = "x";
        private char playerOnO = "o";

        public Game()
        {
        }

        public Game(Board board)
        {
            this.Board = board;
        }

        public void Start()
        {
            Random rand = new Random();
            int x = rand.Next(0, 14);           // Строка 
            int y = rand.Next(0, 14);           // Столбец
            InitializeValue(x, y, playerOnX);   // Вставляем в массив позицию игрока X
            MoveO(x, y, playerOnX);             // Ход игрока О
        }

        /// <summary>
        /// Инициализирует "камень игрока" в "доску".
        /// </summary>
        /// <param name="x">Индекс массива. Номер строки</param>
        /// <param name="y">Индекс массива. Номер столбца</param>
        /// <param name="player">Камень игрока</param>
        public void InitializeValue(int x, int y, char player)
        {
            Board.board[x, y] = player; // инициализируем камень игрока в доску

            // Проверяем не закончилась ли игра?
            if (GameOver(x, y, player))
            {
                Board.DrawBoard();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\t Выиграл игрок \"{0}\"!", player);
            }
        }

        public bool GameOver(int x, int y, char playerChar)
        {
            if (MaxLength(x, y, playerChar) >= 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет нету ли пустых позиции для хода
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            int exist = 0;
            foreach (var k in Board.board)
            {
                if (k == "_")
                {
                    exist++;
                    if (exist > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Ход игрока "x"  
        /// </summary>
        public void MoveX(int x, int y, char player)
        {

            if (!GameOver(x, y, player))
            {
                try
                {
                    Index point = GetBestMove(playerOnX);               // Расчет хорошего хода
                    InitializeValue(point.X, point.Y, playerOnX);       // Сделать ход
                    MoveO(point.X, point.Y, playerOnX);                 // Передать ход другому игроку
                }
                catch (Exception)
                {
                    if (IsFull())
                    {
                        Board.DrawBoard();
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\t Ничья");
                    }
                }
            }
        }

        /// <summary>
        /// Ход игрока "o"
        /// </summary>
        public void MoveO(int x, int y, char player)
        {
            if (!GameOver(x, y, player))
            {
                try
                {
                    Index point = GetBestMove(playerOnO);               // Расчет хорошего хода
                    InitializeValue(point.X, point.Y, playerOnO);       // Сделать ход
                    MoveX(point.X, point.Y, playerOnO);                 // Передать ход другому игроку
                }
                catch (Exception)
                {
                    if (IsFull())
                    {
                        Board.DrawBoard();
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\t Ничья");
                    }
                }
            }
        }

        /// <summary>
        /// Максимальная длина из направлений
        /// </summary>
        /// <param name="x">Строка</param>
        /// <param name="y">Столбец</param>
        /// <param name="player">Игрок</param>
        /// <returns>Максимальную длину из направлений</returns>
        public int MaxLength(int x, int y, char player)
        {

            // Линиям присваивается количество камней одного цвета составляющих непрерывный ряд от координаты последнего хода в направлений линий.
            int vertical = CheckCell(x, y, -1, 0, player) + CheckCell(x, y, 1, 0, player);      // (x - 1, y) + (x + 1, y) ↓ + ↑
            int horizontal = CheckCell(x, y, 0, -1, player) + CheckCell(x, y, 0, 1, player);    // (x, y - 1) + (x, y + 1) ← + →   
            int leftDiagonal = CheckCell(x, y, -1, -1, player) + CheckCell(x, y, 1, 1, player); // ↖ + ↘
            int rightDiagonal = CheckCell(x, y, -1, 1, player) + CheckCell(x, y, 1, -1, player);// ↗ + ↙

            int[] gomoku = new int[] { vertical, horizontal, leftDiagonal, rightDiagonal };

            int result = 0;

            for (int i = 0; i < gomoku.Length; i++)
            {
                result = Math.Max(result, gomoku[i]);
            }

            return result;

        }

        /// <summary>
        /// Проверка ячеек для определения занятых линий
        /// </summary>
        /// <returns></returns>
        public int CheckCell(int x, int y, int a, int b, char player)
        {
            int result = 0;
            for (int i = 1; i < 5; i++)
            {
                if (Board.GetElement(x + (a * i), y + (b * i)) == player)
                {
                    result++;
                }
                else
                {
                    i = 5;
                }
            }

            return result;
        }

        /// <summary>
        /// Поменять значение игрока
        /// </summary>
        public string ReversePlayerChar(char player)
        {
            if (player == "x")
            {
                return "o";
            }

            return "x";
        }

        /// <summary>
        /// Расчет лучшего ответного хода.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public Index GetBestMove(char player)
        {
            Index? bestMove = null;
            IComparable CurrentMove = null;     // Нынешний ход
            int currentplayer = 0;
            int nextplayer = 0;

            Index cells = new Index();

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (Board.GetElement(i, j) == "_")
                    {
                        IComparable possibleMoves;      // Возможный ход
                        currentplayer = 1 + MaxLength(i, j, player);                    // Непрерывный ряд камней игрока N + 1 
                        nextplayer = 1 + MaxLength(i, j, ReversePlayerChar(player));    // Непрерывный ряд камней противника + 1

                        if (currentplayer == 5)     // Получилось 5 в ряд, выигрышный ход
                        {
                            currentplayer = int.MaxValue;
                        }

                        possibleMoves = Math.Max(currentplayer, nextplayer); // Если ряд противника больше > игрока, закрыть ряд противника. Если меньше продолжить свой ряд.
                        if (possibleMoves.CompareTo(CurrentMove) > 0)   // Если ряд в возможном ходу больше > предущего расчета хода, происвоить новый ход;
                        {
                            cells.X = i;  // Индексы лучшего хода.
                            cells.Y = j;
                            bestMove = cells;
                            CurrentMove = possibleMoves; // Новый ход
                        }
                    }
                }
            }

            if (bestMove == null) // Если на доске не осталось пустых позиции
            {
                throw new Exception();
            }

            return (Index)bestMove;
        }
    }

}
