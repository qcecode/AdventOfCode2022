using System.Runtime.CompilerServices;

Day09.Part1();
Day09.Part2();

public static class Day09
{
    public static void Part1()
    {
        var input = File.ReadAllLines("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day09\\input09.txt");
        var visited = new HashSet<(int, int)>();

        (int X, int Y) headPos = (0, 0);
        (int X, int Y) tailPos = (0, 0);
        Add(visited, tailPos);

        for (int i = 0; i < input.Length; i++)
        {
            char dir;
            int dis;
            DecodeInput(input, i, out dir, out dis);

            for (int k = 0; k < dis; k++)
            {
                headPos.MoveHead(dir);
                tailPos.Follow(headPos);
                Add(visited, tailPos);
            }
        }
        Console.WriteLine($"Visited Positions: {visited.Count}");
    }

    public static void Part2()
    {
        var input = File.ReadAllLines("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day09\\input09.txt");
        var visited = new HashSet<(int, int)>()
        {
             (0, 0)
        };

        (int X, int Y) headPos = (0, 0);
        (int X, int Y)[] knots = {
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
            (0, 0),
        };

        for (int i = 0; i < input.Length; i++)
        {
            char dir;
            int dis;
            DecodeInput(input, i, out dir, out dis);

            for (int k = 0; k < dis; k++)
            {
                headPos.MoveHead(dir);
                knots[0].Follow(headPos);

                for (int m = 1; m < knots.Length; m++)
                {
                    knots[m].Follow(knots[m - 1]);
                }

                Add(visited, knots[^1]);
            }
        }
        Console.WriteLine($"Visited by tail of the rope: {visited.Count}");
    }

    private static void DecodeInput(string[] input, int i, out char dir, out int dis)
    {
        var instruction = input[i];
        dir = instruction[0];
        dis = int.Parse(instruction[2..]);
    }

    private static void Add(this HashSet<(int, int)> set, (int, int) item)
    {
        if (!set.Contains(item))
        {
            set.Add(item);
        }
    }

    public static void MoveHead(this ref (int X, int Y) pos, char dir)
    {
        switch (dir)
        {
            case 'U':
                pos.Y++;
                break;
            case 'D':
                pos.Y--;
                break;
            case 'L':
                pos.X--;
                break;
            case 'R':
                pos.X++;
                break;
        }
    }

    public static void Follow(this ref (int X, int Y) pos, (int X, int Y) target)
    {
        var stepX = target.X - pos.X;
        var stepY = target.Y - pos.Y;

        if (Math.Abs(stepX) <= 1 && Math.Abs(stepY) <= 1)
        {
            return;
        }

        pos.X += Math.Sign(stepX);
        pos.Y += Math.Sign(stepY);
    }
}