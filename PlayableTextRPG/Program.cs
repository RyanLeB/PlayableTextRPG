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
            Console.WriteLine("Welcome to my playable text RPG");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("Your goal is to collect seeds around a dungeon map whilst avoiding or defeating the enemies around you!");
            Console.WriteLine("\n");
            Console.WriteLine("You can attack by either running into the enemy or pressing the spacebar when an enemy is close!");
            Console.WriteLine("It's dangerous to go alone... good luck!");
            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);
            Console.Clear();


            while (gameOver != true)
            {

                DrawMap();
                DisplayHUD();
                DisplayLegend();
                PlayerInput();
                EnemyMovement();

            }
            Console.Clear();
            if (youWin == true)
            {
                Console.WriteLine("You win!");
                Console.WriteLine("\n");
                Console.WriteLine($"You collected : {currentSeeds} Seeds!");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("You died...");
                Console.ReadKey(true);
            }
        }


        static void DisplayHUD()
        {
            Console.SetCursorPosition(0, mapY + 1);
            Console.WriteLine($"Player Health: {playerHealth}/{maxPlayerHealth} | Collected Seeds: {currentSeeds} | Enemy Health: {enemyHealth}/{maxEnemyHealth}");
        }


        static void DisplayLegend()
        {
            Console.SetCursorPosition(0, mapY + 2);
            Console.WriteLine("Player = !" + "\n" + "Enemy = E" + "\n" + "Walls = #" + "\n" + "Floor = -" + "\n" + "Seeds = &" + "\n" + "SpikeTrap = ^  Door: %");
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

            Console.Clear();

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

                    if (tile == 'E' && levelComplete == false)
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("!");
            Console.ResetColor();
        }

        static void PlayerInput()
        {
            bool moved = false;

            int movementX;
            int movementY;

            int newPlayerPositionX = playerPositionX;
            int newPlayerPositionY = playerPositionY;

            moved = false;

            playerController = Console.ReadKey(true);

            if (moved == false)

                if (playerController.Key == ConsoleKey.Spacebar)
                {
                    // Add code for the attack action here
                    AttackEnemy();
                    moved = true; // Mark as moved to prevent additional movement
                    return;
                }

            



            // Up

            if (playerController.Key == ConsoleKey.UpArrow || playerController.Key == ConsoleKey.W)
            {
                movementY = Math.Max(playerPositionY - 1, 0);

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


                if (layout[movementY, playerPositionX] == '^')
                {
                    playerHealth -= 1;
                    if (playerHealth <= 0)
                    {
                        gameOver = true;
                    }
                }

                if (layout[movementY, playerPositionX] == '#')
                {
                    movementY = playerPositionY;
                    playerPositionY = movementY;
                    return;
                }
                if (layout[movementY, playerPositionX] == 'E')
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
                movementY = Math.Min(playerPositionY + 1, maximumY);

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
                if (layout[movementY, playerPositionX] == '^')
                {
                    playerHealth -= 1;
                    if (playerHealth <= 0)
                    {
                        gameOver = true;
                    }
                }

                if (layout[movementY, playerPositionX] == '#')
                {
                    movementY = playerPositionY;
                    playerPositionY = movementY;
                    return;
                }

                if (layout[movementY, playerPositionX] == 'E')
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
                movementX = Math.Max(playerPositionX - 1, 0);
                    
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

                if (layout[playerPositionY, movementX] == '^')
                {
                    playerHealth -= 1;
                    if (playerHealth <= 0)
                    {
                        gameOver = true;
                    }
                }
                if (layout[playerPositionY, movementX] == '#')
                {
                    movementX = playerPositionX;
                    playerPositionX = movementX;
                    
                    return;
                }
                if (layout[playerPositionY, movementX] == 'E')
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
                movementX = Math.Min(playerPositionX + 1, maximumX);

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

                if (layout[playerPositionY, movementX] == '^')
                {
                    playerHealth -= 1;
                    if (playerHealth <= 0)
                    {
                        gameOver = true;
                    }

                }


                if (layout[playerPositionY, movementX] == '#')
                {
                    movementX = playerPositionX;
                    playerPositionX = movementX;
                    return;
                }
                if (layout[playerPositionY, movementX] == 'E')
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

        static void EnemyMovement()
        {
            int enemyMovementX = enemyPositionX;
            int enemyMovementY = enemyPositionY;
            int newEnemyPositionX = enemyPositionX;
            int newEnemyPositionY = enemyPositionY;

            // random roll to move
            Random randomRoll = new Random();

            // enemy will have 1 of 4 options to move
            int rollResult = randomRoll.Next(1, 5);

            while ((enemyMovementX == playerPositionX && enemyMovementY == playerPositionY) ||
           (enemyMovementX == newEnemyPositionX && enemyMovementY == newEnemyPositionY))
            {
                rollResult = randomRoll.Next(1, 5); // Retry if the position is the same as the player



                if (rollResult == 1)
                {
                    enemyMovementY = enemyPositionY + 1;
                    if (enemyMovementY >= maximumY)
                    {
                        enemyMovementY = maximumY;
                    }
                }
                else if (rollResult == 2)
                {
                    enemyMovementY = enemyPositionY - 1;
                    if (enemyMovementY <= 0)
                    {
                        enemyMovementY = 0;
                    }
                }
                else if (rollResult == 3)
                {
                    enemyMovementX = enemyPositionX - 1;
                    if (enemyMovementX <= 0)
                    {
                        enemyMovementX = 0;
                    }
                }
                else // rollResult == 4
                {
                    enemyMovementX = enemyPositionX + 1;
                    if (enemyMovementX >= maximumX)
                    {
                        enemyMovementX = maximumX;
                    }
                }
            }

            if (layout[enemyMovementY, enemyMovementY] == '!')
            {
                return;
                 
            }

            // Check for collisions and update the enemy position
            if (layout[enemyMovementY, enemyMovementX] != '#')
            {

                layout[enemyPositionY, enemyPositionX] = '-';
                // Update enemy position if no collision
                enemyPositionX = enemyMovementX;
                enemyPositionY = enemyMovementY;
            }


            // Check for collision with player
            if (enemyPositionX == playerPositionX && enemyPositionY == playerPositionY)
            {
                playerHealth -= 1;
                if (playerHealth <= 0)
                {
                    gameOver = true;
                }
                return;
            }
            if (enemyMovementX == playerPositionX && enemyMovementY == playerPositionY)
            {
                // Do not move to the player's position
                return;
            }
        
        }




        static void AttackEnemy()
        {

            if (Math.Abs(playerPositionX - enemyPositionX) <= 1 && Math.Abs(playerPositionY - enemyPositionY) <= 1)
            {
                enemyHealth -= 1;
                if (enemyHealth <= 0)
                {
                    enemyPositionX = 0;
                    enemyPositionY = 0;
                    enemyAlive = false;
                }
            }
        }



        static void EnemyPosition()
        {
            Console.SetCursorPosition(enemyPositionX, enemyPositionY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("E");
            Console.ResetColor();
        }

        // Making sure enemy is still alive
        static void EnemyAlive()
        {
            enemyAlive = true;
        }
    }



}



















            







        














 
