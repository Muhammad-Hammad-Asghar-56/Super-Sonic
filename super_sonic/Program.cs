using System;
using System.IO;
using System.Threading;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZInput;
using super_sonic;
namespace super_sonic
{
    class Program
    {
        static char[,] maze = new char[20, 172];

        static int displaycolumn = 0;
        static int turn = 0;
        static string slider = "up";
        static int enemymovementturn = 0;
        static axis super_sonic = new axis();
        static int gamelevel = 150;
        static bool flage = true; // double jump flage
        static int health = 50;
        static int coinsmultipler = 1;
        static int coins;
        static char[] bird = new char[20];


        static void Main(string[] args)
        {
            ConsoleHelper.SetCurrentFont("Consolas", 8);
            load();
            super_sonic = findsonic();

            while (true)
            {
                intial();
                rules();
                display();
                while (startgame == true)
                {
                    slidermove();
                    birdmovement();
                    sonicmovement();
                    enemy();
                    Thread.Sleep(gamelevel);
                    super_sonic.alwaysdown(maze);
                    healthing();
                    if (Keyboard.IsKeyPressed(Key.Escape))
                    {
                        Console.Clear();
                        store();
                        startgame = false;
                        break;
                    }
                    if (health < 5)
                    {
                        maze[super_sonic.sx, super_sonic.sy] = ' ';
                        super_sonic.sx = 18;
                        super_sonic.sy = 2;
                        maze[super_sonic.sx, super_sonic.sy] = 'S';
                        health = 50;
                        Console.Clear();
                        break;

                    }
                    if (super_sonic.sy == 171)
                    {
                        maze[super_sonic.sx, super_sonic.sy] = ' ';
                        super_sonic.sx = 18;
                        super_sonic.sy = 2;
                        maze[super_sonic.sx, super_sonic.sy] = 'S';
                        goal();
                    }
                    Console.SetCursorPosition(2, 25);
                    Console.Write("HEALTH    {0}", health);
                    Console.SetCursorPosition(2, 26);
                    Console.Write("SCORE     {0}", coins);
                }
            }
        }
        //********************************************************************************************
        //                           find the super sonic after the game start
        //********************************************************************************************
        static axis findsonic()
        {
            axis ob = new axis();
            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 172; col++)
                {
                    if (maze[row, col] == 'S')
                    {
                        ob.sy = col;
                        ob.sx = row;
                        return ob;
                    }
                }
            }
            return ob;
        }
        static void intialfill()
        {
            // fill the out line of the board

            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 172; col++)
                {
                    if (row == 0)
                    {
                        maze[row, col] = '#';
                    }
                    if (row == 19)
                    {
                        maze[row, col] = '#';
                    }
                    if (col == 0)
                    {
                        maze[row, col] = '#';
                    }
                    if (col == 171)
                    {
                        maze[row, col] = '#';
                    }
                }
            }
        }
        //                       LOAD AND STORE THE MAZE  (FILE HANDLING)
        static void store()
        {
            StreamWriter file = new StreamWriter("D:\\2nd Semester\\game\\super_sonic\\maze.txt");
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 172; j++)
                {
                    file.Write("{0},", maze[i, j]);
                }
                file.WriteLine("");
            }
            file.Flush();
            file.Close();
        }
        static void load()
        {
            string line = " ";
            StreamReader file = new StreamReader("D:\\2nd Semester\\game\\super_sonic\\maze.txt");
            for (int i = 0; i < 20; i++)
            {
                line = file.ReadLine();
                Console.WriteLine(line);
                populate_array(line, i);
            }
            file.Close();
        }
        static void populate_array(string Line, int number)
        {
            for (int i = 0; i < 172; i++)
            {
                maze[number, i] = getfield(Line, i);
            }
            Console.WriteLine("{0}", Line);
        }
        static char getfield(string line, int field)
        {
            char ch = ' ';
            int comma = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ',')
                {
                    comma++;
                }
                else if (comma == field)
                {
                    return line[i];
                }
            }
            return ch;
        }
        static void random()
        {
            Random rand = new Random();
            int freespace = 0;
            int air;
            //********************************** Free space **********************************
            for (int i = 0; i < 30; i++)
            {
                freespace = rand.Next();
                freespace = freespace % 171;
                if (freespace < 10)
                {
                    continue;
                }
                maze[19, freespace] = ' ';
                maze[19, freespace + 1] = ' ';
            }
            //********************************** Enemy space **********************************
            for (int i = 0; i < 30; i++)
            {
                freespace = rand.Next();
                freespace = freespace % 171;
                if (freespace < 10)
                {
                    continue;
                }
                maze[18, freespace] = 'E';
                maze[18, 171 - freespace] = 'E';
            }
            //********************************** moving sliders **********************************
            for (int i = 0; i < 30; i++)
            {
                freespace = rand.Next();
                freespace = freespace % 171;
                if (freespace < 10)
                {
                    continue;
                }
                maze[18, freespace] = '%';
                maze[18, freespace + 1] = '%';

                maze[19, freespace] = ' ';
                maze[19, freespace + 1] = ' ';
            }
            //********************************* flying bird **************************************
            for (int i = 0; i < 50; i++)
            {
                freespace = rand.Next();
                air = rand.Next() % 16;
                if (air == 0)
                {
                    continue;
                }
                freespace = freespace % 171;
                maze[air, freespace] = '<';
            }
        }
        static void display()
        {
            for (int row = 0; row < 20; row++)
            {
                for (int col = displaycolumn; col < 172; col++)
                {
                    if (maze[row, col] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(maze[row, col]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(maze[row, col]);
                    }
                }
                Console.WriteLine("");
            }
        }
        //------------------------------- slider & bird move --------------------------
        static void slidermove()
        {
            if (slider == "up")
            {
                for (int row = 1; row < 18; row++)
                {
                    for (int col = 1; col < 171; col++)
                    {
                        if (maze[row + 1, col] == 'S' && maze[row + 2, col] == '%')
                        {
                            maze[row, col] = 'S';
                            Console.SetCursorPosition(col, row);
                            Console.Write("S");

                            maze[row + 1, col] = ' ';
                            Console.SetCursorPosition(col, (row + 1));
                            Console.Write(" ");
                            super_sonic.sx--;
                        }
                        if (maze[row + 1, col] == '%')
                        {
                            maze[row, col] = maze[row + 1, col];
                            Console.SetCursorPosition(col, row);
                            Console.Write(maze[row + 1, col]);

                            maze[row + 1, col] = ' ';
                            Console.SetCursorPosition(col, row + 1);
                            Console.Write(" ");
                        }
                    }
                }
            }
            if (slider == "down")
            {
                for (int row = 18; row > 0; row--)
                {
                    for (int col = 1; col < 171; col++)
                    {
                        if (maze[row - 1, col] == '%')
                        {
                            maze[row, col] = maze[row - 1, col];
                            Console.SetCursorPosition(col, row);
                            Console.Write(maze[row - 1, col]);

                            maze[row - 1, col] = ' ';
                            Console.SetCursorPosition(col, row - 1);
                            Console.Write(" ");
                        }
                    }
                }
            }

            for (int col = 1; col < 171; col++)
            {
                if (maze[18, col] == '%')
                {
                    slider = "up";
                }
                if (maze[2, col] == '%')
                {
                    slider = "down";
                }
            }
        }
        static void birdmovement()
        {
            for (int row = 0; row < 20; row++)
            {
                bird[row] = maze[row, 1];
                if (maze[row, 1] == '<')
                {
                    maze[row, 1] = ' ';
                    Console.SetCursorPosition(1, row);
                    Console.Write(" ");
                }
            }

            //                                            move to left side
            for (int row = 18; row > 0; row--)
            {
                for (int col = 1; col < 171; col++)
                {
                    if (maze[row, col + 1] == '<' && maze[row, col] == ' ')
                    {
                        maze[row, col] = maze[row, col + 1];
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.SetCursorPosition(col, row);
                        Console.Write(maze[row, col + 1]);
                        Console.ForegroundColor = ConsoleColor.White;

                        maze[row, col + 1] = ' ';
                        Console.SetCursorPosition(col + 1, row);
                        Console.Write(" ");
                    }
                }
            }
            //                           PLACE on the right side
            for (int row = 0; row < 20; row++)
            {
                maze[row, 170] = bird[row];

                if (row >= 0)
                {
                Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(170, row);
                    Console.Write("{0}", bird[row]);
                Console.ForegroundColor = ConsoleColor.White;
                }
            }
            //_________________________________________________ REMOVE EXTRA BIRDS _________________________________________________________
            for (int row = 18; row > 0; row--)
            {
                for (int col = 1; col < 171; col++)
                {
                    if((maze[row,col]=='E' || maze[row, col] == '#' || maze[row, col] == '<') && (maze[row,col+1]=='<'))
                    {
                        maze[row, col + 1] = ' ';
                        Console.SetCursorPosition(col + 1, row);
                        Console.Write(" ");
                    }
                }
            }
                    //_________________________________________________ ADD MORE BIRDS _____________________________________________________________
                    Random r = new Random();
            int j = 0, z = 0;
                j = r.Next();
                j = j % 170;
                z = r.Next();
                z = z % 17;
            if (z > 0)
            {
                maze[z, j] = '<';
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(j, z);
                Console.Write(maze[z, j]);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        //                                        Sonic movement
       
        public static void sonicmovement()
        {
            axis super = new axis();
            if (Keyboard.IsKeyPressed(Key.RightArrow))
            {
                callOfForwordfunctioninClass();
                flage = true;
            }
            if (Keyboard.IsKeyPressed(Key.LeftArrow))
            {
                callOfBackwardfunctioninClass();
                flage = true;
            }
            if (Keyboard.IsKeyPressed(Key.Space))
            {
                if (flage == true)
                {
                    callOfJumpfunctioninClass();
                    flage = false;
                }
                else
                {
                    flage = true;
                }
            }
        }
        static void callOfForwordfunctioninClass()
        {
            super_sonic.sonic_moveforward(maze, coinsmultipler,ref coins,ref health);
        }
        static void callOfBackwardfunctioninClass()
        {
                    super_sonic.sonic_moveback(maze, coinsmultipler, ref coins, ref health);
        }
        static void callOfJumpfunctioninClass()
        {
                super_sonic.sonic_movejump(maze, coinsmultipler, ref coins, ref health);
        }
        //*************************************************************************************
        //                                        Enemy Movement
        //*************************************************************************************
        static void enemy()
    {
        if (enemymovementturn % 3 == 0)
        {
                for (int col = super_sonic.sy; col < (super_sonic.sy + 7); col++)
                {
                    if (maze[18, col] == 'E')
                    {
                        if (maze[19, col - 1] == '#' && maze[18, col - 1] == ' ')
                        {
                            maze[18, col - 1] = 'E';
                            Console.SetCursorPosition(col - 1, 18);
                            Console.Write("E");
                            maze[18, col] = ' ';
                            Console.SetCursorPosition(col, 18);
                            Console.Write(" ");
                        }
                    }
                }
                if (super_sonic.sy > 7)
                {
                    for (int col = super_sonic.sy; col > (super_sonic.sy - 7); col--)
                    {
                        if (maze[18, col] == 'E')
                        {
                            if (maze[19, col + 1] == '#' && maze[18, col + 1] == ' ')
                            {
                                maze[18, col + 1] = 'E';
                                Console.SetCursorPosition(col + 1, 18);
                                Console.Write("E");
                                maze[18, col] = ' ';
                                Console.SetCursorPosition(col, 18);
                                Console.Write(" ");
                            }
                        }
                    }
                }
            }
            enemymovementturn++;


        }
        //*************************************************************************************
        //                                            Jujment
        //*************************************************************************************
        static void healthing()
        {
            if (super_sonic.sx == 19)
            {
                health = health - 50;
            }
            else
            {
                if (maze[super_sonic.sx, super_sonic.sy + 1] == '<')
                {
                    health = health - 25;
                    maze[super_sonic.sx, super_sonic.sy + 1] = ' ';
                    Console.SetCursorPosition(super_sonic.sy + 1, super_sonic.sx);
                    Console.WriteLine(" ");
                }
            }
        }
        static int menu()
        {
            int option=0;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            Console.WriteLine("^                              MAIN MENU                                                      ^");
            Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(" YOUR OPTION IS :");
            Console.WriteLine("1: Strat the Game");
            Console.WriteLine("2: Resumme Game ");
            Console.WriteLine("3: Level");
            Console.WriteLine("4: Player health ");
            Console.WriteLine("5: Exit the game");
            option = int.Parse(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            return option;
        }
        static void level()
        {
            int op = 0;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Clear();
            Console.WriteLine("                     ____  _   _ ____  _____ _____           ____   ____  _   _ ___ ____ ");
            Console.WriteLine("                    / ___|| | | |  _ || ____|  _ ,|         | ___| / _  || | | |_ _| ___|");
            Console.WriteLine("                    |___  | | | | |_) |  _| | |_) |         |___ || | | || |>| || | |    ");
            Console.WriteLine("                     ___)|| |_| |  __/| |___|  _ <           ___) | |_| || |>  || | |___ ");
            Console.WriteLine("                    |____||_____|_|   |_____|_| |_|         |____| |___ ||_| |_|___|____|");
            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            Console.WriteLine("^                              MAIN MENU                                                      ^");
            Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("mainmenu > level");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("1: Easuper_sonic.sy");
            Console.WriteLine("2: Medium");
            Console.WriteLine("3: Hard");
            Console.WriteLine("");
            Console.WriteLine("4: Back to the main menu");
            Console.ForegroundColor = ConsoleColor.White;
            op = int.Parse(Console.ReadLine());
            if (op == 1)
            {
                gamelevel = 300;
            }
            if (op == 2)
            {
                gamelevel = 200;
            }
            if (op == 3)
            {
                gamelevel = 50;
            }
        }
        static void players()
        {
            int op = 0;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Clear();
            Console.WriteLine("                     ____  _   _ ____  _____ _____           ____   ____  _   _ ___ ____ ");
            Console.WriteLine("                    / ___|| | | |  _ || ____|  _ ,|         | ___| / _  || | | |_ _| ___|");
            Console.WriteLine("                    |___  | | | | |_) |  _| | |_) |         |___ || | | || |>| || | |    ");
            Console.WriteLine("                     ___)|| |_| |  __/| |___|  _ <           ___) | |_| || |>  || | |___ ");
            Console.WriteLine("                    |____||_____|_|   |_____|_| |_|         |____| |___ ||_| |_|___|____|");
            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            Console.WriteLine("^                              MAIN MENU                                                      ^");
            Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("main menu > Palyers");
            Console.WriteLine("");
            Console.WriteLine("1 : TAILS  :  20  HP            coin : 1x");
            Console.WriteLine("2 : OMEGA  :  50  HP            coin : 2x");
            Console.WriteLine("3 : STROM  :  100 HP            coin : 3x");
            Console.WriteLine("4 : SONIC  :  200 HP            coin : 4x");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("5: Back to the main menu");
            op = int.Parse(Console.ReadLine());
            if (op == 1)
            {
                health = 20;
                coinsmultipler = 1;
            }
            if (op == 2)
            {
                health = 50;
                coinsmultipler = 2;
            }
            if (op == 3)
            {
                health = 100;
                coinsmultipler = 3;
            }
            if (op == 4)
            {
                health = 200;
                coinsmultipler = 4;
            }
        }
        static bool startgame = false;
        static void intial()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("                     ____  _   _ ____  _____ ____            ____  _____   _  _ ___ ____ ");
            Console.WriteLine("                    | ___|| | | |  _ || ____|  _ |          | ___||  _  | | | |_ _ | ___|");
            Console.WriteLine("                    |___  | | | | |_)||  _| | |_) |         |___ || | | | | |>| || | |    ");
            Console.WriteLine("                     ___)|| |_| |  __|| |___|  _ <           ___) | |_| | | |>  || | |___ ");
            Console.WriteLine("                    |____||_____|_|   |_____|_| |_|         |____||_____| |_| |_|___|____|");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            int option = 0;
            while (option < 5)
            {
                option = menu();
                if (option == 1)
                {
                    startgame = true;
                    break;
                }
                if (option == 2)
                {
                    load();
                    findsonic();
                    Console.Clear();
                }
                if (option == 3)
                {
                    level();
                    Console.Clear();
                }
                if (option == 4)
                {
                    players();
                    Console.Clear();
                }
            }
            Console.Clear();
        }
        static void goal()
        {
            Console.SetCursorPosition(20, 10);
            Console.WriteLine("                                                                                                   ");
            Console.WriteLine("                                                                                                   ");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!! C O N G R A T U L A T I O N !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("         YOUR                CLEAR                     THE              LEVEL                      ");

            Console.ReadKey();
        }
        static void rules()
        {
            Console.Write("");
            Console.Write("");
            Console.SetCursorPosition(20, 10);
            Console.Write("R E A D        C A R E F U L L Y");
            Console.Write("representation:  ");
            Console.SetCursorPosition(20, 11);
            Console.SetCursorPosition(20, 12);
            Console.Write("                S  :  PLAYER ");
            Console.SetCursorPosition(20, 13);
            Console.Write("                <  :  Birds ");
            Console.SetCursorPosition(20, 14);
            Console.Write("                @  :  COINS ");
            Console.SetCursorPosition(20, 15);
            Console.Write("                E  :  ENEMY ");
            Console.SetCursorPosition(20, 16);
            Console.Write("           %%%%%%% :  SLIDER ");
            Console.SetCursorPosition(20, 17);
            Console.Write
            ("HEALTH Criteria:  ");

            Console.SetCursorPosition(20, 19);
            Console.Write("                <  :  -25 HP ");
            Console.SetCursorPosition(20, 20);
            Console.Write("                E  :  -50 HP");
            Console.SetCursorPosition(20, 21);
            Console.Write("     SPACE(##   ##):   -50");
            Console.ReadKey();
            Console.Clear();
        }

    }
}
