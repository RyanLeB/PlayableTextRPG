using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayableTextRPG
{
    internal class Program
    {
        // player & enemy Variables


        // movement 
        static ConsoleKeyInfo playerController;
        static int playerPositionX;
        static int playerPositionY;
        static int enemyPositionX;
        static int enemyPositionY;


        // health

        static int maxPlayerHealth;
        static int playerHealth;
        static int maxEnemyHealth;
        static int enemyHealth;
        static bool enemyAlive;

        // damage
        static int playerDamage;
        static int enemyDamage;

        // collectable
        static int currentSeeds;
        static bool youWin;
        static bool gameOver;
        static bool levelComplete;

        // map Variables

        // map
        static string path;
        static string RPGMap = @"RPGMap.txt";
        static string[] floor;
        static char[,] layout;

        // coordinates
        static int mapX;
        static int mapY;
        static int maximumX;
        static int maximumY;





        static void Main(string[] args)
        {
            OnStart();
            Console.WriteLine("Welcome to my playable text RPG... Press any key to continue!");
            Console.ReadKey(true);
            Console.Clear();

            
            while (gameOver != true)
            {
                DrawMap();
                PlayerInput();
                EnemyMovement();
            }
            Console.Clear();
            if (youWin == true)
            {
                Console.WriteLine("You win!");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("You died...");
                Console.ReadKey(true);
            }
        }


        static void OnStart()
        {
            // Initialization

            maxPlayerHealth = 6;
            maxEnemyHealth = 3;
            playerHealth = maxPlayerHealth;
            enemyHealth = maxEnemyHealth;

            playerDamage = 1;
            enemyDamage = 1;
            EnemyAlive();


            currentSeeds = 0;
            
            gameOver = false;
            levelComplete = false;
            
            path = RPGMap;
            floor = File.ReadAllLines(path);
            layout = new char[floor.Length, floor[0].Length];
            CreateMap();

            mapX = layout.GetLength(1);
            mapY = layout.GetLength(0);

            maximumX = mapX - 1;
            maximumY = mapY - 1;


        }
        



        static void CreateMap()
        {
            for (int i = 0; i < floor.Length; i++)
            {
                for (int j = 0; j < floor[i].Length; j++)
                {
                    layout[i, j] = floor[i][j];
                }
            }
            
            


        }
        
        
        static void DrawMap()
        {
            
            for (int k = 0; k < mapY; k++) 
            {
                for (int l = 0; l < mapX; l++)
                {
                    char tile = layout[k, l];

                    if (tile == '=' && levelComplete == false)
                    {
                        playerPositionX = l;
                        playerPositionY = k - 1;
                        levelComplete = true;
                        layout[k, l] = '#';
                    }

                    if (tile == 'E' &&  levelComplete == false)
                    {
                        if (tile == 'E')
                        {
                            enemyPositionX = l;
                            enemyPositionY = k;
                        }
                    }
                    Console.Write(tile);
                }   
                Console.WriteLine();

            }

            PlayerPosition();
            EnemyPosition();
            Console.SetCursorPosition(0, 0);



        }
        
        
        
        
        static void PlayerPosition()
        {
            Console.SetCursorPosition(playerPositionX, playerPositionY);
            Console.WriteLine("!");    
        }
        
        static void PlayerInput()
        {
            bool moved;

            int movementX;
            int movementY;

            moved = false;

            playerController = Console.ReadKey(true);

            if (moved == false)
            {
                // Up

                if (playerController.Key == ConsoleKey.UpArrow || playerController.Key == ConsoleKey.W)
                {
                    movementY = playerPositionY - 1;

                    if (movementY <= 0)
                    {
                        movementY = 0;
                    }
                    if (movementY == enemyPositionY && playerPositionX == enemyPositionX)
                    {
                        enemyHealth -= 1;
                        if (enemyHealth <= 0)
                        {
                            enemyPositionX = 0;
                            enemyPositionY = 0;
                            enemyAlive = false;
                        }

                        return;
                    }
                    if (layout[movementY, playerPositionX] == '#')
                    {
                        movementY = playerPositionY;
                        playerPositionY = movementY;
                        return;
                    }
                    else
                    {
                        moved = true;
                        playerPositionY = movementY;
                        if (playerPositionY <= 0)
                        {
                            playerPositionY = 0;
                        }


                    }


                }

                // Down

                if (playerController.Key == ConsoleKey.DownArrow || playerController.Key == ConsoleKey.S)
                {
                    movementY = playerPositionY + 1;

                    if (movementY >= maximumY)
                    {
                        movementY = maximumY;
                    }
                    if (movementY == enemyPositionY && playerPositionX == enemyPositionX)
                    {
                        enemyHealth -= 1;
                        if (enemyHealth <= 0)
                        {
                            enemyPositionX = 0;
                            enemyPositionY = 0;
                            enemyAlive = false;
                        }

                        return;
                    }
                    if (layout[movementY, playerPositionX] == '#')
                    {
                        movementY = playerPositionY;
                        playerPositionY = movementY;
                        return;
                    }
                    else
                    {
                        moved = true;
                        playerPositionY = movementY;
                        if (playerPositionY >= maximumY)
                        {
                            playerPositionY = maximumY;
                        }


                    }


                }



                // Left

                if (playerController.Key == ConsoleKey.LeftArrow || playerController.Key == ConsoleKey.A)
                {
                    movementX = playerPositionX - 1;

                    if (movementX <= 0)
                    {
                        movementX = 0;
                    }
                    if (movementX == enemyPositionX && playerPositionY == enemyPositionY)
                    {
                        enemyHealth -= 1;
                        if (enemyHealth <= 0)
                        {
                            enemyPositionX = 0;
                            enemyPositionY = 0;
                            enemyAlive = false;
                        }

                        return;
                    }
                    if (layout[playerPositionY, movementX] == '#')
                    {
                        movementX = playerPositionX;
                        playerPositionX = movementX;
                        return;
                    }
                    else
                    {
                        moved = true;
                        playerPositionX = movementX;
                        if (playerPositionX <= 0)
                        {
                            playerPositionX = 0;
                        }


                    }


                }



                // Right

                if (playerController.Key == ConsoleKey.RightArrow || playerController.Key == ConsoleKey.D)
                {
                    movementX = playerPositionX + 1;

                    if (movementX >= maximumX)
                    {
                        movementX = maximumX;
                    }
                    if (movementX == enemyPositionX && playerPositionY == enemyPositionY)
                    {
                        enemyHealth -= 1;
                        if (enemyHealth <= 0)
                        {
                            enemyPositionX = 0;
                            enemyPositionY = 0;
                            enemyAlive = false;
                        }

                        return;
                    }
                    if (layout[playerPositionY, movementX] == '#')
                    {
                        movementX = playerPositionX;
                        playerPositionX = movementX;
                        return;
                    }
                    else
                    {
                        moved = true;
                        playerPositionX = movementX;
                        if (playerPositionX >= maximumX)
                        {
                            playerPositionX = maximumX;
                        }
                    }

                }

                // Winning door

                if (layout[playerPositionY, playerPositionX] == '%')
                {
                    youWin = true;
                    gameOver = true;

                }

                // Collectable seeds
                if (layout[playerPositionY, playerPositionX] == '&')
                {
                    currentSeeds += 1;
                    layout[playerPositionY, playerPositionX] = '~';
                }

                // Exit game

                if (playerController.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(1);
                }






            }

        }

        
        static void EnemyPosition()
        {
            Console.SetCursorPosition(enemyPositionX, enemyPositionY);
            Console.WriteLine("E");
        }
        
        static void EnemyMovement()
        {
            int enemyMovementX;
            int enemyMovementY;

            // random roll to move
            Random randomRoll = new Random();


            // enemy will have 1 of 5 options to move
            int rollResult = randomRoll.Next(1, 5);

            if (rollResult == 1)
            {
                enemyMovementY = enemyPositionY + 1;
                if (enemyMovementY >= maximumY)
                {
                    enemyMovementY = maximumY;
                }

                if (enemyMovementY == playerPositionY && enemyPositionX == playerPositionX)
                {
                    playerHealth -= 1;
                    if (playerHealth <= 0)
                    {
                        gameOver = true;
                    }

                    return;
                }

                if (layout[enemyMovementY, enemyPositionX] == '#')
                {
                    enemyMovementY = enemyPositionY;
                    enemyPositionY = enemyMovementY;
                    return;
                }

                else
                {
                    enemyPositionY = enemyMovementY;
                    if (enemyPositionY >= maximumY)
                    {
                        enemyPositionY = maximumY;
                    }
                }
            }

            if (rollResult == 2)
            {
                enemyMovementY = enemyPositionY - 1;
                if (enemyMovementY <= 0)
                {
                   enemyMovementY = 0;
                }

                if (enemyMovementY == playerPositionY && enemyPositionX == playerPositionX)
                {
                   playerHealth -= 1;
                   if (playerHealth <= 0)
                   {
                      gameOver = true;
                   }

                   return;
                }

                if (layout[enemyMovementY, enemyPositionX] == '#')
                {
                   enemyMovementY = enemyPositionY;
                   enemyPositionY = enemyMovementY;
                   return;
                }

                else
                {
                    enemyPositionY = enemyMovementY;
                if (enemyPositionY >= maximumY)
                {
                    enemyPositionY = maximumY;
                }
            }
        }



            if (rollResult == 3)
            {
                enemyMovementX = enemyPositionX - 1;
                if (enemyMovementX >= maximumX)
                {
                    enemyMovementX = maximumX;
                }

                if (enemyMovementX <= 0)
                {
                    enemyMovementX = 0;
                }

                if (enemyMovementX == playerPositionX && enemyPositionY == playerPositionY)
                {
                    playerHealth -= 1;
                    if (playerHealth <= 0)
                    {
                        gameOver = true;
                    }
                    return;
                }


                if (layout[enemyPositionY, enemyMovementX] == '#')
                {
                    enemyMovementX = enemyPositionX;
                    enemyPositionX = enemyMovementX;
                    return;
                }

                else
                {
                    enemyPositionX = enemyMovementX;
                    if (enemyPositionX <= 0)
                    {
                        enemyPositionX = 0;
                    }
                }
        }

        if (rollResult == 4)
        {
            enemyMovementX = enemyPositionX + 1;
            if (enemyMovementX == playerPositionY && enemyPositionX == playerPositionX)
            {
                playerHealth -= 1;
                if (playerHealth <= 0)
                {
                    gameOver = true;
                }
                return;
            }
            if (layout[enemyPositionY, enemyMovementX] == '#')
            {
                enemyMovementX = enemyPositionX;
                enemyPositionX = enemyMovementX;
                return;
            }
            else
            {
                enemyPositionY = enemyMovementX;
                
            if (enemyPositionX >= maximumX)
            {
                enemyPositionX = maximumX;
            }
        }
    }



                            

                
    }


        // Making sure enemy is still alive
        static void EnemyAlive()
        {
            enemyAlive = true;
        }


    }    
}


 
