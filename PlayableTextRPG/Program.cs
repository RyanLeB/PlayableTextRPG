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

            currentSeeds = 1;
            
            path = RPGMap;
            floor = File.ReadAllLines(path);
            layout = new char[floor.Length, floor[0].Length];

            mapX = layout.GetLength(1);
            mapY = layout.GetLength(0);

            maximumX = mapX - 1;
            maximumY = mapY - 1;


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
                    if (layout[playerPositionY, playerPositionX] == '#')
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
    
        
        // Making sure enemy is still alive
        static void EnemyAlive()
        {
            enemyAlive = true;
        }
    
    
    }
}
 
