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
        Parent = null;
    }
}

public static class Day12
{
    static string[] input = File.ReadAllLines("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day12\\input12.txt");
    static Node[,] map;
    static int MAP_WIDTH = input[0].Length;
    static int MAP_HEIGHT = input.Length;
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
            Node currentNode = nodesToVisit.Dequeue();
            int[] loc = new int[] { currentNode.XPos, currentNode.YPos };

            // Up
            if (loc[1] - 1 >= 0)
            {
                Node upNode = map[loc[0], loc[1] - 1];
                if (!nodesToVisit.Contains(upNode) && !visited.Contains(upNode) && (upNode.Elevation - currentNode.Elevation) <= 1)
                {
                    upNode.Parent = currentNode;
                    nodesToVisit.Enqueue(upNode);
                }
            }
            // Down
            if (loc[1] + 1 < MAP_HEIGHT)
            {
                Node downNode = map[loc[0], loc[1] + 1];
                if (!nodesToVisit.Contains(downNode) && !visited.Contains(downNode) && (downNode.Elevation - currentNode.Elevation) <= 1)
                {
                    downNode.Parent = currentNode;
                    nodesToVisit.Enqueue(downNode);
                }
            }
            // Left
            if (loc[0] - 1 >= 0)
            {
                Node leftNode = map[loc[0] - 1, loc[1]];
                if (!nodesToVisit.Contains(leftNode) && !visited.Contains(leftNode) && (leftNode.Elevation - currentNode.Elevation) <= 1)
                {
                    leftNode.Parent = currentNode;
                    nodesToVisit.Enqueue(leftNode);
                }
            }
            // Right
            if (loc[0] + 1 < MAP_WIDTH)
            {
                Node rightNode = map[loc[0] + 1, loc[1]];
                if (!nodesToVisit.Contains(rightNode) && !visited.Contains(rightNode) && (rightNode.Elevation - currentNode.Elevation) <= 1)
                {
                    rightNode.Parent = currentNode;
                    nodesToVisit.Enqueue(rightNode);
                }
            }
            visited.Enqueue(currentNode);
        }
    }

    private static void ParseInput()
    {
        map = new Node[MAP_WIDTH, MAP_HEIGHT];
        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                map[x, y] = new Node(x, y, input[y][x]);
                if (input[y][x] == 'E')
                {
                    destination = new int[] { x, y };
                    map[x, y].Elevation = 'z';
                }
                else if (input[y][x] == 'S')
                {
                    startingLocation = new int[] { x, y };
                    map[x, y].Elevation = 'a';
                }
            }
        }

        Node startingNode = map[startingLocation[0], startingLocation[1]];
        nodesToVisit.Enqueue(startingNode);
        startingNode.Parent = null;
    }
}
