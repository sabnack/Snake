using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Snake
{
    class Program
    {
        static int delay = 300;
        const int FieldX = 10;
        const int FieldY = 20;
        static bool EndGame = false;
        static string Move = "Right";

        static void Main(string[] args)
        {
            ConsoleKeyInfo _key;
            Console.CursorVisible = false;
            Thread Play = new Thread(Game);
            Play.Start();

            while (true)
            {

                _key = Console.ReadKey(true);

                if (_key.Key == ConsoleKey.RightArrow)
                {
                    if (Move != "Left") Move = "Right";
                }

                if (_key.Key == ConsoleKey.LeftArrow)
                {
                    if (Move != "Right") Move = "Left";
                }

                if (_key.Key == ConsoleKey.UpArrow)
                {
                    if (Move != "Down") Move = "Up";
                }

                if (_key.Key == ConsoleKey.DownArrow)
                {
                    if (Move != "Up") Move = "Down";
                }
                if (_key.Key == ConsoleKey.Escape)
                {
                    Console.SetCursorPosition(0, FieldX + 2);
                    Environment.Exit(0);
                }
                if (_key.Key == ConsoleKey.Spacebar)
                {
                    if (EndGame)
                    {
                        Play.Abort();
                        EndGame = false;
                        Move = "Right";
                        delay = 300;
                        Console.SetCursorPosition(0, 0);
                        Play = new Thread(Game);
                        Play.Start();
                    }
                }
                Thread.Sleep(delay);
            }

        }
        static void Game()
        {
            Random rand = new Random();
            int x = 0;
            int y = 0;
            int[] apple = new int[3];
            List<int>[] snake = new List<int>[2];
            for (int k = 0; k < 2; k++)
            {
                snake[k] = new List<int>();
            }

            snake[0].Add(0);
            snake[1].Add(0);

            apple[0] = rand.Next(1, FieldX);
            apple[1] = rand.Next(1, FieldY);

            Print(snake, apple, x, y, FieldX, FieldY);
            Console.SetCursorPosition(0, 0);

            while (true)
            {
                switch (Move)
                {
                    case "Right":
                        {
                            EndGame = TestCoordinates(snake, x, y + 1, FieldX, FieldY);
                            y++;
                            break;
                        }
                    case "Left":
                        {
                            EndGame = TestCoordinates(snake, x, y - 1, FieldX, FieldY);
                            y--;
                            break;
                        }
                    case "Up":
                        {
                            EndGame = TestCoordinates(snake, x - 1, y, FieldX, FieldY);
                            x--;
                            break;
                        }
                    case "Down":
                        {
                            EndGame = TestCoordinates(snake, x + 1, y, FieldX, FieldY);
                            x++;
                            break;
                        }
                }
                if (EndGame)
                {
                    Console.SetCursorPosition(0, FieldX + 2);
                    Console.WriteLine("Вы проиграли!");
                    break;
                }

                TestEat(snake, apple, x, y, FieldX, FieldY);
                SnakeBody(snake, apple, x, y);
            //    Console.Clear();
                Print(snake, apple, x, y, FieldX, FieldY);
                Console.SetCursorPosition(0, 0);
                if (snake[0].Count == FieldX * FieldY - 1)
                {
                    Console.SetCursorPosition(0, FieldX + 2);
                    Console.WriteLine("Вы выиграли!");
                    break;
                }
                Thread.Sleep(delay);
            }
        }

        static bool TestCoordinates(List<int>[] snake, int x, int y, int FieldX, int FieldY)
        {
            if (x == FieldX || y == FieldY || x < 0 || y < 0) return true;

            for (int k = 0; k < snake[0].Count; k++)
            {
                if (x == snake[0][k] && y == snake[1][k]) return true;
            }
            return false;
        }

        static void TestEat(List<int>[] snake, int[] apple, int x, int y, int FieldX, int FieldY)
        {
            Random rand = new Random();
            int tmpx = rand.Next(0, FieldX);
            int tmpy = rand.Next(0, FieldY);
            if (apple[0] == x && apple[1] == y)
            {
                for (int k = 0; k < snake[0].Count;)
                {
                    if (tmpx == snake[0][k] && tmpy == snake[1][k] || tmpx == x && tmpy == y)
                    {
                        tmpx = rand.Next(0, FieldX);
                        tmpy = rand.Next(0, FieldY);
                        k = 0;
                    }
                    else k++;
                }
                apple[2]++;
                apple[0] = tmpx;
                apple[1] = tmpy;
                if (snake[0].Count % 5 == 0 && snake[0].Count != 0) delay -= 25;
            }
        }

        static void SnakeBody(List<int>[] snake, int[] apple, int x, int y)
        {
            if (apple[2] + 1 > snake[0].Count)
            {
                snake[0].Add(x);
                snake[1].Add(y);
            }
            else
            {
                snake[0].Add(x);
                snake[1].Add(y);
                snake[0].RemoveAt(0);
                snake[1].RemoveAt(0);
            }
        }

        static void Print(List<int>[] snake, int[] apple, int x, int y, int FieldX, int FieldY)
        {
            int flag = 0;
            Console.Write("┌");
            for (int i = 0; i < FieldY; i++) Console.Write("─");
            Console.WriteLine("┐ Съеденно {0}", apple[2]);
            for (int i = 0; i < FieldX; i++)
            {
                Console.Write("│");
                for (int j = 0; j < FieldY; j++)
                {
                    if (apple[0] == i && apple[1] == j)
                    {
                        Console.Write("@");
                        flag++;
                    }
                    for (int k = 0; k < snake[0].Count; k++)
                    {
                        if (i == snake[0][k] && j == snake[1][k])
                        {
                            if (k == snake[0].Count - 1) Console.Write("O");
                            else Console.Write("*");
                            flag++;
                        }
                    }
                    if (flag > 0) flag = 0;
                    else Console.Write(" ");
                }
                Console.WriteLine("│");
            }
            Console.Write("└");
            for (int i = 0; i < FieldY; i++) Console.Write("─");
            Console.WriteLine("┘");
            //Console.Write("X ");
            //foreach (int i in snake[0])
            //    Console.Write("{0:00} ", i);
            //Console.Write("\nY ");
            //foreach (int i in snake[1])
            //    Console.Write("{0:00} ", i);
            //Console.Write("\nX {0:00} \nY {1:00}\n", apple[0], apple[1]);

        }
    }
}
