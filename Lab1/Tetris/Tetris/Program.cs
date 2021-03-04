using System;
using System.Collections.Generic;
using System.Threading;

namespace Tetris
{
    class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;
            Console.WindowHeight = FieldRows + 1;
            Console.WindowWidth = ConsoleColumns;
            Console.BufferHeight = FieldRows + 1;
            Console.BufferWidth = ConsoleColumns;

            Console.WriteLine("Left, rigth, down arrows or 'A', 'D', 'S' to move figure.");
            Console.WriteLine("Up arrow or 'W' to rotate figure.");
            Console.WriteLine("Tap Enter to start game.");
            ConsoleKey k = Console.ReadKey(true).Key;
            while (k != ConsoleKey.Enter)
            {
                k = Console.ReadKey(true).Key;
            }
            Console.Clear();
            
            CurrentFigure = Figures[Random.Next(Figures.Count)];
            NextFigure = Figures[Random.Next(Figures.Count)];

            DrawField();
            DrawCurrentFigure();
            DrawInfoField();
            int Frame = 0;
            while (true)
            {
                Frame++;
                if (Frame == 10)
                {
                    EraseCurrentFigure();
                    CurrentFigureRow++;
                    DrawCurrentFigure();
                    Frame = 0;
                }
                if (IsCollision(CurrentFigure))
                {
                    if (CurrentFigureRow == 1)
                    {
                        Console.SetCursorPosition(FieldColumns / 2 - 5, FieldRows / 2 - 1);
                        Console.WriteLine("              ");
                        Console.SetCursorPosition(FieldColumns / 2 - 5, FieldRows / 2);
                        Console.WriteLine("  Game over!  ");
                        Console.SetCursorPosition(FieldColumns / 2 - 5, FieldRows / 2 + 1);
                        Console.WriteLine("              ");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                    }
                    AttachFigureToField();
                    CurrentFigure = NextFigure;
                    NextFigure = Figures[Random.Next(Figures.Count)];
                    DrawInfoField();
                    CurrentFigureRow = 0;
                    CurrentFigureColumn = FieldColumns / 2 - 3;
                }
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = new ConsoleKey();
                    key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                        {
                            if (CurrentFigureColumn > 0)
                            {
                                bool noInterference = true;
                                for (int row = 0; row < CurrentFigure.GetLength(0); row++)
                                {
                                    if (CurrentFigure[row, 0] && Field[CurrentFigureRow + row, CurrentFigureColumn - 1])
                                    {
                                        noInterference = false;
                                        break;
                                    }
                                }
                                if (noInterference)
                                {
                                    EraseCurrentFigure();
                                    CurrentFigureColumn -= 2;
                                    DrawCurrentFigure();                                    
                                }
                            }
                            break;
                        }
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                        {
                            if (CurrentFigureColumn + CurrentFigure.GetLength(1) < FieldColumns)
                            {
                                bool noInterference = true;
                                for (int row = 0; row < CurrentFigure.GetLength(0); row++)
                                {
                                    if (CurrentFigure[row, CurrentFigure.GetLength(1) - 1] && 
                                            Field[CurrentFigureRow + row, CurrentFigureColumn + CurrentFigure.GetLength(1)])
                                    {
                                        noInterference = false;
                                        break;
                                    }
                                }
                                if (noInterference)
                                {
                                    EraseCurrentFigure();
                                    CurrentFigureColumn += 2;
                                    DrawCurrentFigure();
                                }
                            }
                            break;
                        }
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                        {
                            if (CurrentFigureRow < FieldRows - 1)
                            {
                                EraseCurrentFigure();
                                CurrentFigureRow++;
                                DrawCurrentFigure();
                            }
                            break;
                        }
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                        {
                            RotateFigure();
                            break;
                        }
                    }
                }
                Thread.Sleep(40);
            }
        }

        static int FieldRows = 22, FieldColumns = 30;
        static int InfoColumns = 20;
        static int ConsoleColumns = FieldColumns + InfoColumns;
        static bool[,] Field = new bool[FieldRows + 1, FieldColumns];
        static bool[,] CurrentFigure;
        static bool[,] NextFigure;
        static int Score;
        static int CurrentFigureRow = 0, CurrentFigureColumn = FieldColumns / 2 - 3;
        static Random Random = new Random();

        static List<bool[,]> Figures = new List<bool[,]>()
        {
            new bool[,]
            { 
                { true, true, true, true, false, false },
                { false, false, true, true, true, true }
            },
            new bool[,]
            {
                { false, false, true, true, true, true },
                { true, true, true, true, false, false }
            },
            new bool[,]
            {
                { true, true },
                { true, true },
                { true, true }
            },
            new bool[,]
            {
                { true, true, true, true },
                { true, true, true, true }
            },
            new bool[,]
            {
                { true, true, false, false, false, false },
                { true, true, true, true, true, true }
            },
            new bool[,]
            {
                { false, false, true, true, false, false },
                { true, true, true, true, true, true }
            },
            new bool[,]
            {
                { false, false, false, false, true, true },
                { true, true, true, true, true, true }
            }

        };

        static void RotateFigure()
        {
            bool[,] rotatedFigure = new bool[CurrentFigure.GetLength(1) / 2, CurrentFigure.GetLength(0) * 2];
            for (int row = 0; row < CurrentFigure.GetLength(0) * 2; row = row + 2)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1) / 2; col++)
                {
                    rotatedFigure[col, CurrentFigure.GetLength(0) * 2 - row - 1] = CurrentFigure[row / 2, col * 2];
                    rotatedFigure[col, CurrentFigure.GetLength(0) * 2 - row - 2] = CurrentFigure[row / 2, col * 2];
                }
            }
            EraseCurrentFigure();
            if (IsInBounds(rotatedFigure))
            {
                CurrentFigure = rotatedFigure;
            }
            DrawCurrentFigure();
        }

        static void EraseCurrentFigure()
        {
            Console.ResetColor();
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    Console.SetCursorPosition(CurrentFigureColumn + col, CurrentFigureRow + row);
                    Console.Write(" ");
                }
            }
        }

        static void DrawCurrentFigure()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (CurrentFigure[row, col])
                    {
                        Console.SetCursorPosition(col + CurrentFigureColumn, row + CurrentFigureRow);
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(" ");
                    } 
                }
            }
            Console.ResetColor();
        }

        static void DrawField()
        {
            for (int row = 0; row < FieldRows; row++)
            {
                for (int col = 0; col < FieldColumns; col++)
                {
                    if (Field[row, col])
                    {
                        Console.SetCursorPosition(col, row);
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.SetCursorPosition(col, row);
                        Console.Write(" ");
                    }
                }
            }
            Console.BackgroundColor = ConsoleColor.DarkGray;
            for (int col = 0; col < FieldColumns; col++)
            {
                Console.SetCursorPosition(col, FieldRows);
                Console.Write(" ");
            }
            Console.ResetColor();
        }

        static bool IsCollision(bool[,] figure)
        {
            if (CurrentFigureRow + CurrentFigure.GetLength(0) == FieldRows)
            {
                return true;
            }
            for (int row = 0; row < figure.GetLength(0); row++)
            {
                for (int col = 0; col < figure.GetLength(1); col++)
                {
                    if (Field[CurrentFigureRow + row + 1, CurrentFigureColumn + col] && CurrentFigure[row, col])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static bool IsInBounds(bool[,] figure)
        {
            if (CurrentFigureColumn + figure.GetLength(1) <= FieldColumns)
            {
                return true;
            }
            return false;
        }

        static void AttachFigureToField()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (CurrentFigure[row, col])
                    {
                        Field[CurrentFigureRow + row, CurrentFigureColumn + col] = true;
                    }
                }
            }
            RemoveLines();
        }

        static void RemoveLines()
        {
            for (int rowCheck = 0; rowCheck < FieldRows; rowCheck++)
            {
                bool isFull = true;
                for (int col = 0; col < FieldColumns - 1; col += 2)
                {
                    if (!Field[rowCheck, col])
                    {
                        isFull = false;
                        break;
                    }
                }
                if (isFull)
                {
                    Score++;
                    for (int rowMove = rowCheck; rowMove > 0; rowMove--)
                    {
                        for (int col = 0; col < FieldColumns; col++)
                        {
                            Field[rowMove, col] = Field[rowMove - 1, col];
                        }
                    }
                    rowCheck--;
                }
            }
            DrawInfoField();
            DrawField();
        }

        static void DrawInfoField()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            for (int row = 0; row < FieldRows; row++)
            {
                Console.SetCursorPosition(FieldColumns, row);
                Console.Write(" ");
            }
            for (int col = FieldColumns; col < ConsoleColumns; col++)
            {
                Console.SetCursorPosition(col, FieldRows);
                Console.Write(" ");
            }
            Console.ResetColor();
            Console.SetCursorPosition(FieldColumns + InfoColumns / 2 - 6, 3);
            Console.Write("Next Figure:");
            EraseNextFigure();
            DrawNextFigure();
            Console.ResetColor();
            Console.SetCursorPosition(FieldColumns + InfoColumns / 2 - 4, 13);
            Console.Write("Score: " + Score);
        }

        static void DrawNextFigure()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            for (int row = 0; row < NextFigure.GetLength(0); row++)
            {
                for (int col = 0; col < NextFigure.GetLength(1); col++)
                {
                    if (NextFigure[row, col])
                    {
                        Console.SetCursorPosition(FieldColumns + InfoColumns / 2 - NextFigure.GetLength(1) / 2 + col, 5 + row);
                        Console.Write(" ");
                    }
                }
            }
        }

        static void EraseNextFigure()
        {
            Console.ResetColor();
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    Console.SetCursorPosition(FieldColumns + InfoColumns / 2 - 3 + col, 5 + row);
                    Console.Write(" ");
                }
            }
        }
    }
}   
