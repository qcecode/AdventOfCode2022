using System;
using System.Collections.Generic;
using System.Text;

Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day18\\example18.txt", true);
Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day18\\input18.txt", false );

static void Run(string inputPath, bool isTest)
{
    const int ExampleAnswer1 = 64;
    const int ExampleAnswer2 = 58;

    var input = File.ReadAllText(inputPath);
    var neighbors = new List<(int x, int y, int z)> { 
        (1, 0, 0), (-1, 0, 0), (0, 1, 0), 
        (0, -1, 0), (0, 0, 1), (0, 0, -1) 
    };
    var lines = input.Split("\n").Select(x => x.Split(",").Select(int.Parse).ToArray());
    var cubes = lines.Select(cube => (x: cube[0], y: cube[1], z: cube[2])).ToHashSet();

    var answer1 = 0;
    foreach (var (x, y, z) in cubes)
    {
        foreach (var (dx, dy, dz) in neighbors)
        {
            if (!cubes.Contains((x + dx, y + dy, z + dz)))
            {
                answer1++;
            }
        }
    }

    WriteAnswer(1, answer1, ExampleAnswer1, isTest);

    var answer2 = 0;
    var maxX = cubes.Max(x => x.x);
    var maxY = cubes.Max(y => y.y);
    var maxZ = cubes.Max(z => z.z);
    var minX = cubes.Min(x => x.x);
    var minY = cubes.Min(y => y.y);
    var minZ = cubes.Min(z => z.z);

    var xRange = Enumerable.Range(minX, maxX + 1).ToList();
    var yRange = Enumerable.Range(minY, maxY + 1).ToList();
    var zRange = Enumerable.Range(minZ, maxZ + 1).ToList();

    foreach (var (x, y, z) in cubes)
    {
        foreach (var (dx, dy, dz) in neighbors)
        {
            if (isOutside((x + dx, y + dy, z + dz)))
            {
                answer2++;
            }
        }
    }

    WriteAnswer(2, answer2, ExampleAnswer2, isTest);

    bool isOutside((int x, int y, int z) cube)
    {
        if (cubes.Contains(cube)) return false;

        // Use a queue to search for cubes that are outside the range of valid coordinates
        var checkedCubes = new Dictionary<(int x, int y, int z), bool>();
        var queue = new Queue<(int x, int y, int z)>();
        queue.Enqueue(cube);
        while (queue.Any())
        {
            var tempCube = queue.Dequeue();
            if (checkedCubes.TryGetValue(tempCube, out var visited) && visited) continue;
            checkedCubes[tempCube] = true;

            // Return true if the current cube is outside the range of valid coordinates
            if (!xRange.Contains(tempCube.x) || !yRange.Contains(tempCube.y) || !zRange.Contains(tempCube.z))
            {
                return true;
            }

            // Enqueue the neighbors of the current cube if they are not already occupied
            for (int i = 0; i < neighbors.Count; i++)
            {
                var (dx, dy, dz) = neighbors[i];
                var neighbor = (tempCube.x + dx, tempCube.y + dy, tempCube.z + dz);
                if (!cubes.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }
        return false;
    }
}



static void WriteAnswer<T>(int number, T val, T supposedval, bool isTest)
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
