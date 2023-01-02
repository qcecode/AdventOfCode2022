using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


var input = File.ReadAllLines("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day14\\input14.txt")
    .Select(line => line.Split(" -> ")
        .Select(point => point.Split(',')
            .Select(int.Parse).ToArray())
    .Select(point => (X: point[0], Y: point[1])).ToArray()).ToArray();

var walls = new HashSet<(int X,int Y)> ();
var sand = new HashSet<(int X,int Y)> ();

foreach (var line in input)
{
    for(int i = 1; i < line.Length; i++)
    {
        var wall =  from X in Enumerable.Range(
                        Math.Min(line[i].X, line[i - 1].X),
                        Math.Abs(line[i].X - line[i - 1].X) + 1)
                    from Y in Enumerable.Range(
                        Math.Min(line[i].Y, line[i - 1].Y),
                        Math.Abs(line[i].Y - line[i - 1].Y) + 1)
                    select  (X, Y);
        foreach (var brick in wall)
        {
            walls.Add(brick);
        }
    }
}

int minX = walls.Min(x => x.X);
int maxX = walls.Max(x => x.X);

int minY = walls.Min(y => y.Y);
int maxY = walls.Max(y => y.Y);

// add base line Part2        
for (int k = minX-1000; k <= maxX+1000; k++)
{
    var lUnit = (X: k, Y: maxY+2);
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


var unit = (X: 500, Y: 0);

while (!sand.Contains((500, 0)) && unit.Y < maxY)
{
    if(!walls.Contains((unit.X, unit.Y + 1 )) && !sand.Contains((unit.X, unit.Y + 1)))
    {
        unit = (unit.X, unit.Y + 1);
    }

    else if (!walls.Contains((unit.X - 1, unit.Y + 1)) && !sand.Contains((unit.X - 1, unit.Y + 1)))
    {
        unit = (unit.X - 1, unit.Y + 1);
    }

    else if (!walls.Contains((unit.X + 1, unit.Y + 1)) && !sand.Contains((unit.X + 1, unit.Y + 1)))
    {
        unit = (unit.X + 1, unit.Y + 1);
    }
    else
    {
        sand.Add(unit);
        unit = (X: 500, Y: 0);
    }

}

Console.WriteLine(sand.Count);