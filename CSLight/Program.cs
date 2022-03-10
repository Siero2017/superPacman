using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSLight
{   
    class Program
    {

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            bool isPlaing = true;
            int pacmanX, pacmanY;
            bool isAlive = true;

            int ghostX, ghostY;
            int ghostDX = 0, ghostDY = -1;

            int pacmanDX = 0, pacmanDY = 1;

            int allDots = 0;
            int collectDots = 0;

            char[,] map = ReadMap("Map1", out pacmanX, out pacmanY, out ghostX, out ghostY, ref allDots);
            DrawMap(map);            

            while (isPlaing)
            {
                Console.SetCursorPosition(0, 20);
                Console.WriteLine($"Собрано ягод: {collectDots}/{allDots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    ChangeDiraction(key, ref pacmanDX, ref pacmanDY);
                }

                if (map[pacmanX + pacmanDX, pacmanY + pacmanDY] != '#')
                {
                    CollectDots(map, pacmanX, pacmanY, ref collectDots);

                    Move(map, '@', ref pacmanX, ref pacmanY, pacmanDX, pacmanDY);
                }

                if (map[ghostX + ghostDX, ghostY + ghostDY] != '#')
                {
                    Move(map, '$', ref ghostX, ref ghostY, ghostDX, ghostDY);

                }
                else
                {
                    ChangeDiraction(new Random(), ref ghostDX, ref ghostDY);
                }

                if(ghostX == pacmanX && ghostY == pacmanY)
                {
                    isAlive = false;
                }

                System.Threading.Thread.Sleep(300);

                if(allDots == collectDots || isAlive == false)
                {
                    isPlaing = false;                    
                }
            }
            if(collectDots == allDots)
            {
                Console.Clear();
                Console.WriteLine("Победа!");
            }
            if(isAlive == false)
            {
                Console.Clear();
                Console.WriteLine("Вы проиграли, вас убили!");
            }
        }

        static void Move(char[,] map, char symbol, ref int X, ref int Y, int DX, int DY)
        {           
            Console.SetCursorPosition(Y, X);
            Console.Write(map[X,Y]);

            X += DX;
            Y += DY;

            Console.SetCursorPosition(Y, X);
            Console.Write(symbol);            
        }

        static void CollectDots(char[,] map, int pacmanX, int pacmanY, ref int collectDots)
        {
            if (map[pacmanX, pacmanY] == '.')
            {
                collectDots++;
                map[pacmanX, pacmanY] = ' ';
            }
        }

        static void ChangeDiraction(ConsoleKeyInfo key, ref int DX, ref int DY)
        {            
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    DX = -1; DY = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    DX = 0; DY = -1;
                    break;
                case ConsoleKey.DownArrow:
                    DX = 1; DY = 0;
                    break;
                case ConsoleKey.RightArrow:
                    DX = 0; DY = 1;
                    break;
            }
        }
        static void ChangeDiraction(Random rand, ref int DX, ref int DY)
        {
            int ghostDir = rand.Next(1, 5);
            switch (ghostDir)
            {
                case 1:
                    DX = -1; DY = 0;
                    break;
                case 2:
                    DX = 0; DY = -1;
                    break;
                case 3:
                    DX = 1; DY = 0;
                    break;
                case 4:
                    DX = 0; DY = 1;
                    break;
            }
        }

        static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i,j]);
                }
                Console.WriteLine();
            }
        }

        static char[,] ReadMap(string mapName, out int packmanX, out int packmanY, out int ghostX, out int ghostY, ref int allDots)
        {
            packmanX = 0;
            packmanY = 0;
            ghostX = 0;
            ghostY = 0;
            string[] newFile = File.ReadAllLines($"Maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if(map[i,j] == '@')
                    {
                        packmanX = i;
                        packmanY = j;
                        map[i, j] = '.';
                    }
                    else if(map[i,j] == '$')
                    {
                        ghostX = i;
                        ghostY = j;
                        map[i, j] = '.';
                    }
                    else if (map[i,j] == ' ')
                    {
                        map[i, j] = '.';
                        allDots++;
                    }
                }
            }
            return map;
        }
    }
}
