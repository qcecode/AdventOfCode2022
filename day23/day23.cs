using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;

Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day23\\example23.txt", true);
Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day23\\input23.txt", false);

void Run(string inputfile, bool isTest)
{
    long supposedanswer1 = 110;
    long supposedanswer2 = 20;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var elves = new HashSet<(long x, long y)>();

    int i = 0;
    while (i < S.Count)
    {
        var s = S[i];
        for (int j = 0; j < s.Length; j++)
        {
            if (s[j] == '#') elves.Add((j, i));
        }
        i++;
    }

    bool AddProposed(Dictionary<(long x, long y), List<(long x, long y)>> proposed, (long x, long y) elf, (long x, long y) target, (long x, long y) check1, (long x, long y) check2, (long x, long y) check3)
    {
        if (!elves.Contains(check1) && !elves.Contains(check2) && !elves.Contains(check3))
        {
            List<(long x, long y)> l;
            if (!proposed.TryGetValue(target, out l))
            {
                l = new List<(long x, long y)>();
            }
            l.Add(elf);
            proposed[target] = l;
            return true;
        }
        return false;
    }

    var proposed = new Dictionary<(long x, long y), List<(long x, long y)>>();
    int start = 0;
    bool moves = true;
    int k = 0;
    while (moves)
    {
        if (k == 10)
        {
            long minX = elves.Select(a => a.x).Min();
            long maxX = elves.Select(a => a.x).Max();
            long minY = elves.Select(a => a.y).Min();
            long maxY = elves.Select(a => a.y).Max();

            answer1 = (maxX - minX + 1) * (maxY - minY + 1) - elves.Count();
        }

        foreach (var elf in elves)
        {

            bool done = NoElvesArround(elves, elf);
            for (int p = start; p < start + 4; p++)
            {
                if (!done && p % 4 == 0) done = AddProposed(proposed, elf, (elf.x, elf.y - 1), (elf.x - 1, elf.y - 1), (elf.x, elf.y - 1), (elf.x + 1, elf.y - 1));
                if (!done && p % 4 == 1) done = AddProposed(proposed, elf, (elf.x, elf.y + 1), (elf.x - 1, elf.y + 1), (elf.x, elf.y + 1), (elf.x + 1, elf.y + 1));
                if (!done && p % 4 == 2) done = AddProposed(proposed, elf, (elf.x - 1, elf.y), (elf.x - 1, elf.y + 1), (elf.x - 1, elf.y), (elf.x - 1, elf.y - 1));
                if (!done && p % 4 == 3) done = AddProposed(proposed, elf, (elf.x + 1, elf.y), (elf.x + 1, elf.y + 1), (elf.x + 1, elf.y), (elf.x + 1, elf.y - 1));
            }
        }

        moves = false;
        foreach (var target in proposed.Keys)
        {
            var l = proposed[target];
            if (l.Count == 1)
            {
                moves = true;
                var elf = l[0];
                elves.Remove(elf);
                elves.Add(target);
            }
        }
        proposed = new Dictionary<(long x, long y), List<(long x, long y)>>();
        start++; if (start > 3) start = 0;
        k++;
    }
    answer2 = k;

    WriteAnswer(1, answer1, supposedanswer1, isTest);
    WriteAnswer(2, answer2, supposedanswer2, isTest);
}

bool NoElvesArround(HashSet<(long x, long y)> elves, (long x, long y) elf)
{
    // Check all 8 surrounding cells
    for (int x = -1; x <= 1; x++)
    {
        for (int y = -1; y <= 1; y++)
        {
            // Skip the current cell
            if (x == 0 && y == 0) continue;

            if (elves.Contains((elf.x + x, elf.y + y)))
            {
                return false;
            }
        }
    }

    return true;
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
}