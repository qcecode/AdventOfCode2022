using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

Day12.Part2();


public class Node
{
    public int XPos { get; set; }
    public int YPos { get; set; }
    public char Elevation { get; set; }
    public Node Parent { get; set; }

    public Node(int x, int y, char elev)
    {
        XPos = x;
        YPos = y;
        Elevation = elev;
    }

    public void SetParent(Node parent)
    {
        Parent = parent;
    }

}

public static class Day12
{
    static string[] input = File.ReadAllLines("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day12\\input12.txt");
    static Node[,] map;
    static int xSize = input[0].Length;
    static int ySize = input.Length;
    static int[,] mapCoords;

    static int[] startingLocation = new int[2];
    static int[] destination = new int[2];

    static Queue<Node> visited = new Queue<Node>();
    static Queue<Node> nodesToVisit = new Queue<Node>();

    static List<Node> Path = new List<Node>();

    public static void Part1()
    {
        Console.WriteLine("Part 1");

        ParseInput();

        DFS();

        bool PathFound = false;
        Node check = map[destination[0], destination[1]];
        Path.Add(check);
        while (!PathFound)
        {
            if (check.Parent == null)
            {
                PathFound = true;
            }
            else
            {
                Path.Add(check.Parent);
                check = check.Parent;
            }
        }
        Console.WriteLine(Path.Count - 1);
        Path.Clear();
    }

    public static void Part2()
    {
        Console.WriteLine("Part 2");

        ParseInput();

        DFS();

        bool PathFound = false;
        Node check = map[destination[0], destination[1]];
        Path.Add(check);
        PathFound = false;
        while (!PathFound)
        {
            if (check.Elevation == 'a')
            {
                PathFound = true;
            }
            else
            {
                Path.Add(check.Parent);
                check = check.Parent;
            }
        }
        Console.WriteLine(Path.Count + 1);
        Path.Clear();
    }

    private static void DFS()
    {
        while (nodesToVisit.Count > 0 || !visited.Contains(map[destination[0], destination[1]]))
        {
            // Check adjacent nodes
            int[] loc = new int[] { nodesToVisit.First().XPos, nodesToVisit.First().YPos };
            // Up
            if (loc[1] - 1 >= 0)
            {
                if (!nodesToVisit.Contains(map[loc[0], loc[1] - 1]) && !visited.Contains(map[loc[0], loc[1] - 1]) && (map[loc[0], loc[1] - 1].Elevation - nodesToVisit.First().Elevation) <= 1)
                {
                    map[loc[0], loc[1] - 1].Parent = nodesToVisit.First();
                    nodesToVisit.Enqueue(map[loc[0], loc[1] - 1]);
                }
            }
            // Down
            if (loc[1] + 1 < ySize)
            {
                if (!nodesToVisit.Contains(map[loc[0], loc[1] + 1]) && !visited.Contains(map[loc[0], loc[1] + 1]) && (map[loc[0], loc[1] + 1].Elevation - nodesToVisit.First().Elevation) <= 1)
                {
                    map[loc[0], loc[1] + 1].Parent = nodesToVisit.First();
                    nodesToVisit.Enqueue(map[loc[0], loc[1] + 1]);
                }
            }
            // Left
            if (loc[0] - 1 >= 0)
            {
                if (!nodesToVisit.Contains(map[loc[0] - 1, loc[1]]) && !visited.Contains(map[loc[0] - 1, loc[1]]) && (map[loc[0] - 1, loc[1]].Elevation - nodesToVisit.First().Elevation) <= 1)
                {
                    map[loc[0] - 1, loc[1]].Parent = nodesToVisit.First();
                    nodesToVisit.Enqueue(map[loc[0] - 1, loc[1]]);
                }
            }
            // Right
            if (loc[0] + 1 < xSize)
            {
                if (!nodesToVisit.Contains(map[loc[0] + 1, loc[1]]) && !visited.Contains(map[loc[0] + 1, loc[1]]) && (map[loc[0] + 1, loc[1]].Elevation - nodesToVisit.First().Elevation) <= 1)
                {
                    map[loc[0] + 1, loc[1]].Parent = nodesToVisit.First();
                    nodesToVisit.Enqueue(map[loc[0] + 1, loc[1]]);
                }
            }
            visited.Enqueue(nodesToVisit.Dequeue());
        }
    }

    private static void ParseInput()
    {
        map = new Node[xSize, ySize];
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                map[x, y] = new Node(x, y, input[y][x]);
                if (input[y][x] == 'E')
                {
                    destination = new int[] { x, y };
                    map[x, y].Elevation = 'z';
                }
                if (input[y][x] == 'S')
                {
                    startingLocation = new int[] { x, y };
                    map[x, y].Elevation = 'a';
                }
            }
        }

        nodesToVisit.Enqueue(map[startingLocation[0], startingLocation[1]]);
        nodesToVisit.First().Parent = null;
    }
}
