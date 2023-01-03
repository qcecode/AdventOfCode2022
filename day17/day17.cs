using System;
using System.Collections.Generic;
using System.Linq;

Day17 day = new Day17();

day.Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day17\\example17.txt", true);
day.Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day17\\input17.txt", false);

class Day17
{
    // For Testing
    long ExampleAnswer1 = 3068;
    long ExampleAnswer2 = 1514285714288;

    public void Run(string inputPath, bool isTest)
    {
        string input = File.ReadAllText(inputPath);

        var ans1 = new Tunnel(input, 100).AddRocks(2022).Height;
        var ans2 = new Tunnel(input, 100).AddRocks(1000000000000).Height;

        WriteAnswer(1, ans1, ExampleAnswer1, isTest);
        WriteAnswer(2, ans2, ExampleAnswer2, isTest);
    }

    private void WriteAnswer<T>(int number, T val, T supposedval, bool isTest)
    {
        // Convert the values to strings for output
        string v = val?.ToString() ?? "(null)";
        string sv = supposedval?.ToString() ?? "(null)";

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Answer Part " + number + ": ");
        if (isTest)
        {
            // If this is a test, compare the actual and supposed values
            if (v == sv)
            {
                // If they match, output in green
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(v);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" ... supposed (example) answer: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(sv);
            }
            else
            {
                // If they don't match, output in white
                Console.Write(v);
                Console.Write(" ... supposed (example) answer: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(sv);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        else
        {
            // If this is not a test, just output the answer
            Console.WriteLine(v);
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    class Tunnel
    {
        int linesStored;

        List<string> screenLines = new List<string>();
        long linesNotStored;

        string[][] rockShapes;
        string jetMovement;
        customCnt rockCounter;
        customCnt jetCounter;

        public Tunnel(string jetMovement, int linesStored)
        {
            // Initialize member variables
            this.linesStored = linesStored;
            rockShapes = new string[][]{
                new []{"####"},
                new []{" # ", "###", " # "},
                new []{"  #", "  #", "###"},
                new []{"#", "#", "#", "#"},
                new []{"##", "##"}
            };
            rockCounter = new customCnt(0, rockShapes.Length);
            this.jetMovement = jetMovement;
            jetCounter = new customCnt(0, jetMovement.Length);
        }

        // Declare and implement the Height property
        public long Height => screenLines.Count + linesNotStored;

        public Tunnel AddRocks(long numRocksToAdd)
        {
            // Create a dictionary to store previously seen tunnel configurations
            Dictionary<string, (long numRocksToAdd, long height)> seenConfigurations = new Dictionary<string, (long, long)>();

            // Loop until all the rocks have been added
            while (numRocksToAdd > 0)
            {
                // Generate a hash of the current tunnel configuration
                string configurationHash = string.Join("\n", screenLines);

                // Check if the current configuration has been seen before
                if (seenConfigurations.TryGetValue(configurationHash, out var cache))
                {
                    // Calculate the height of one period of the repeating pattern
                    long periodHeight = this.Height - cache.height;

                    // Calculate the length of one period of the repeating pattern
                    long periodLength = cache.numRocksToAdd - numRocksToAdd;

                    // Calculate the number of periods that can fit into the remaining number of rocks
                    long numPeriods = numRocksToAdd / periodLength;

                    // Add the height of the periods to the total height of the tunnel
                    linesNotStored += numPeriods * periodHeight;

                    // Update the number of rocks remaining to add
                    numRocksToAdd = numRocksToAdd % periodLength;

                    // Exit the loop
                    break;
                }
                else
                {
                    // Add the current configuration to the dictionary
                    seenConfigurations[configurationHash] = (numRocksToAdd, this.Height);

                    // Add a new rock to the tunnel
                    this.AddRock();

                    // Decrement the number of rocks remaining to add
                    numRocksToAdd--;
                }
            }

            // Add any remaining rocks to the tunnel
            while (numRocksToAdd > 0)
            {
                this.AddRock();
                numRocksToAdd--;
            }

            return this;
        }

        public Tunnel AddRock()
        {
            // Get the next rock in the rock array
            string[] rockShape = rockShapes[(int)rockCounter++];

            // Add blank lines to the top of the screen to make room for the new rock
            for (int i = 0; i < rockShape.Length + 3; i++)
            {
                screenLines.Insert(0, "|       |");
            }

            // Declare and initialize variables
            int rockXPosition = 3;
            int rockYPosition = 0;
            int jetIndex = 0;

            // Loop until the rock reaches the bottom of the screen or a collision occurs
            while (true)
            {
                // Get the next jet character
                char jetDirection = this.jetMovement[(int)jetCounter++];

                // Move the rock in the specified direction if there is no collision
                if (jetDirection == '>' && !Hit(rockShape, rockXPosition + 1, rockYPosition))
                {
                    rockXPosition++;
                }
                else if (jetDirection == '<' && !Hit(rockShape, rockXPosition - 1, rockYPosition))
                {
                    rockXPosition--;
                }

                // Check if the rock has reached the bottom of the screen or collided with another object
                if (Hit(rockShape, rockXPosition, rockYPosition + 1))
                {
                    break;
                }

                // Increment the rock's vertical position
                rockYPosition++;
            }

            // Draw the rock on the screen
            Draw(rockShape, rockXPosition, rockYPosition);

            return this;
        }

        bool Hit(string[] rockArray, int rockXPosition, int rockYPosition)
        {
            // Check if the rock will go off the bottom of the screen
            if (rockArray.Length + rockYPosition > screenLines.Count)
            {
                return true;
            }

            // Declare and initialize variables
            int numRowsInRock = rockArray.Length;
            int numColsInRock = rockArray[0].Length;
            int rockStartCol = rockXPosition;
            int rockStartRow = rockYPosition;

            // Iterate over the rows and columns of the rock array
            foreach (var rockRow in rockArray)
            {
                foreach (var rockElement in rockRow)
                {
                    // Check if the current element is a '#' character
                    if (rockElement == '#')
                    {
                        // Check if there is an object at the current position on the screen
                        if (screenLines[rockStartRow][rockStartCol] != ' ')
                        {
                            return true;
                        }
                    }

                    // Increment the column position for the next iteration
                    rockStartCol++;
                }

                // Reset the column position and increment the row position for the next iteration
                rockStartCol = rockXPosition;
                rockStartRow++;
            }

            return false;
        }

        void Draw(string[] rockArray, int rockXPosition, int rockYPosition)
        {
            // Declare and initialize variables
            int numRowsInRock = rockArray.Length;
            int numColsInRock = rockArray[0].Length;
            int rockStartCol = rockXPosition;
            int rockStartRow = rockYPosition;

            // Iterate over the rows and columns of the rock array
            foreach (var rockRow in rockArray)
            {
                foreach (var rockElement in rockRow)
                {
                    // Check if the current element is a '#' character
                    if (rockElement == '#')
                    {
                        // Get the current line of text from the screen
                        char[] screenChars = screenLines[rockStartRow].ToArray();

                        // Check if there is already an object at the current position on the screen
                        if (screenChars[rockStartCol] != ' ')
                        {
                            // Throw an exception if there is an object in the way
                            throw new Exception("Collision detected!");
                        }

                        // Add the rock element to the screen at the current position
                        screenChars[rockStartCol] = rockElement;

                        // Convert the modified screen line back to a string and update the screen lines list
                        screenLines[rockStartRow] = string.Join("", screenChars);
                    }

                    // Increment the column position for the next iteration
                    rockStartCol++;
                }

                // Reset the column position and increment the row position for the next iteration
                rockStartCol = rockXPosition;
                rockStartRow++;
            }

            // Remove any lines at the top of the screen that do not contain a '#' character
            while (!screenLines[0].Contains('#'))
            {
                screenLines.RemoveAt(0);
            }

            // Check if the number of lines in the screen exceeds the maximum number of lines stored
            if (screenLines.Count > linesStored)
            {
                // Remove any excess lines from the bottom of the screen
                int numLinesToRemove = screenLines.Count - linesStored;
                screenLines.RemoveRange(linesStored, numLinesToRemove);
                linesNotStored += numLinesToRemove;
            }
        }
    }

    public record struct customCnt
    {
        public int Index { get; private set; }
        public int Mod { get; }

        public customCnt(int index, int mod)
        {
            Index = index;
            Mod = mod;
        }

        public static explicit operator int(customCnt c) => c.Index;
        public static customCnt operator ++(customCnt c) =>
            c with { Index = c.Index == c.Mod - 1 ? 0 : c.Index + 1 };
    }
}