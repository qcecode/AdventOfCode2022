using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

const int START_X = 500;
const int START_Y = 0;
const int BUFFER = 1000;

var lines = File.ReadAllLines("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day14\\input14.txt")
    .Select(line => line.Split(" -> ")
        .Select(point => point.Split(',')
            .Select(int.Parse).ToArray())
    .Select(point => (X: point[0], Y: point[1])).ToArray()).ToArray();

var walls = new HashSet<(int X, int Y)>();
var sand = new HashSet<(int X, int Y)>();

foreach (var currentLine in lines)
{
    var wallBricks = GetWallBricks(currentLine[0], currentLine[1]);
    foreach (var wallBrick in wallBricks)
    {
        walls.Add(wallBrick);
    }
}

int minX = walls.Min(x => x.X);
int maxX = walls.Max(x => x.X);

int minY = walls.Min(y => y.Y);
int maxY = walls.Max(y => y.Y);

// add base line Part2        
for (int x = minX - BUFFER; x <= maxX + BUFFER; x++)
{
    var lUnit = (X: x, Y: maxY + 2);
    walls.Add(lUnit);
}

minX = walls.Min(x => x.X);
maxX = walls.Max(x => x.X);

minY = walls.Min(y => y.Y);
maxY = walls.Max(y => y.Y);

// draw picture
/*
for (int i = minY; i <= maxY; i++)
{
    for (int k = minX; k <= maxX; k++)
    {
        var lUnit = (X: k, Y: i);
        if (walls.Contains(lUnit))
        {
            Console.Write("#");
        }
        else
        {
            Console.Write(".");
        }
    }
    Console.WriteLine();
}
*/


var unit = (X: START_X, Y: START_Y);

while (!sand.Contains((START_X, START_Y)) && unit.Y < maxY)
{
    (int X, int Y) nextUnit;
    if (!walls.Contains((unit.X, unit.Y + 1)) && !sand.Contains((unit.X, unit.Y + 1)))
    {
        nextUnit = (unit.X, unit.Y + 1);
    }
    else if (!walls.Contains((unit.X - 1, unit.Y + 1)) && !sand.Contains((unit.X - 1, unit.Y + 1)))
    {
        nextUnit = (unit.X - 1, unit.Y + 1);
    }
    else if (!walls.Contains((unit.X + 1, unit.Y + 1)) && !sand.Contains((unit.X + 1, unit.Y + 1)))
    {
        nextUnit = (unit.X + 1, unit.Y + 1);
    }
    else
    {
        sand.Add(unit);
        unit = (X: START_X, Y: START_Y);
        continue;
    }

    unit = nextUnit;
}

Console.WriteLine(sand.Count);

IEnumerable<(int X, int Y)> GetWallBricks((int X, int Y) start, (int X, int Y) end)
{
    for (int x = Math.Min(start.X, end.X); x <= Math.Max(start.X, end.X); x++)
    {
        for (int y = Math.Min(start.Y, end.Y); y <= Math.Max(start.Y, end.Y); y++)
        {
            yield return (x, y);
        }
    }
}
