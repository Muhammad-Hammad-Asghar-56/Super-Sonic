using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace super_sonic
{
    class axis
    {
        public int sx;
        public int sy;
        public void sonic_moveforward(char[,] maze ,int coinsmultipler,ref int coins, ref int health)
        {
            if (maze[  sx,   sy + 1] == ' ' || maze[  sx,   sy + 1] == 'O' || maze[  sx,   sy + 1] == '@')
            {
                if (maze[  sx,   sy + 1] == '@')
                {
                    coins = coins + (coinsmultipler * 2);
                }
                maze[  sx,   sy] = ' ';
                Console.SetCursorPosition(  sy,   sx);
                Console.Write(" ");
                maze[  sx,   sy + 1] = 'S';
                Console.SetCursorPosition(  sy + 1,   sx);
                Console.Write("S");
                  sy++;
            }
            if (maze[  sx,   sy + 1] == 'E')
            {
                maze[  sx,   sy] = ' ';
                Console.SetCursorPosition(  sy,   sx);
                Console.Write(" ");

                maze[  sx,   sy + 1] = 'S';
                Console.SetCursorPosition(  sy + 1,   sx);
                Console.Write("S");

                  sy++;
                health = health - 50;
            }
        }
        public void sonic_moveback(char[,] maze, int coinsmultipler, ref int coins, ref int health)
        {
            if (sy > 1)
            {
                if (maze[sx, sy - 1] == ' ' || maze[sx, sy - 1] == 'O' || maze[sx, sy - 1] == '@')
                {
                    if (maze[sx, sy - 1] == '@')
                    {
                        coins = coins + (coinsmultipler * 2);
                    }
                    maze[sx, sy] = ' ';
                    Console.SetCursorPosition(sy, sx);
                    Console.Write(" ");
                    maze[sx, sy - 1] = 'S';
                    Console.SetCursorPosition(sy - 1, sx);
                    Console.Write("S");
                    sy--;
                }

                if (maze[sx, sy - 1] == 'E')
                {
                    maze[sx, sy] = ' ';
                    Console.SetCursorPosition(sy, sx);
                    Console.Write(" ");

                    maze[sx, sy - 1] = 'S';
                    Console.SetCursorPosition(sy, sx);
                    Console.Write("S");

                    sy--;
                    health = health - 50;
                }
            }
        }
        public void sonic_movejump(char[,] maze, int coinsmultipler, ref int coins, ref int health)
        {
            if (  sx > 4)
            {
                if (maze[  sx + 1,   sy] == '#' || maze[  sx + 1,   sy] == '%' ||   sx == 19)
                {
                    if ((maze[  sx - 4,   sy + 4] == ' ' || maze[  sx - 4,   sy + 4] == 'O' || maze[  sx - 4,   sy + 4] == '@') && ((  sx - 4) > 0 && (  sy + 4 < 170)))
                    {
                        if (maze[  sx - 4,   sy + 4] == '@' && maze[  sx - 4,   sy + 4] == 'O')
                        {
                            coins = coins + 10;
                        }
                        maze[  sx,   sy] = ' ';
                        Console.SetCursorPosition(  sy,   sx);
                        Console.Write(" ");

                        maze[  sx - 4,   sy + 4] = 'S';
                        Console.SetCursorPosition(  sy + 4,   sx - 4);
                        Console.Write("S");

                          sx =   sx - 4;
                          sy =   sy + 4;
                    }
                }
            }
        }
        public void alwaysdown(char[,] maze)
        {
            if ( sx < 19)
            {
                if (maze[ sx + 1,  sy] == ' ')
                {
                    maze[ sx,  sy] = ' ';
                    Console.SetCursorPosition( sy,  sx);
                    Console.Write(" ");

                     sx++;

                    maze[ sx,  sy] = 'S';
                    Console.SetCursorPosition( sy,  sx);
                    Console.Write("S");
                }
            }
        }        
    }
}

