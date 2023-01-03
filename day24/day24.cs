﻿using System.Diagnostics;

Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day24\\example24.txt", true);
Console.Clear();
Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day24\\input24.txt", false);

void Run(string inputfile, bool isTest)
{
    int ExampleAnswer1 = 18;
    int ExampleAnswer2 = 54;

    var input = File.ReadAllLines(inputfile).ToList();
    int answer1 = 0;
    int answer2 = 0;
    int xCnt = input[0].Length - 2;
    int yCnt = input.Count - 2;

    var lcm = yCnt * xCnt;
    for (int x = 1; x <= xCnt; x++)
    {
        for (int y = 1; y <= yCnt; y++)
        {
            if (x * yCnt == y * xCnt && y * xCnt < lcm) lcm = y * xCnt;
        }
    }

    var isFree = new bool[yCnt, xCnt, lcm];
    var fields = new byte[yCnt, xCnt];

    int i = 1;
    while (i < input.Count - 1)
    {
        var s = input[i];
        for (int j = 1; j < s.Length - 1; j++)
        {
            byte val = 0;
            switch (s[j])
            {
                case '^': val = 1; break;
                case '>': val = 2; break;
                case 'v': val = 4; break;
                case '<': val = 8; break;
            }
            fields[i - 1, j - 1] = val;
        }

        i++;
    }

    for (int j = 0; j < lcm; j++)
    {
        var newfields = new byte[yCnt, xCnt];

        for (int y = 0; y < yCnt; y++)
        {
            for (int x = 0; x < xCnt; x++)
            {

                isFree[y, x, j] = fields[y, x] == 0;

                if ((fields[y, x] & 1) == 1)
                {
                    int newY = y - 1;
                    if (newY < 0) newY = yCnt - 1;
                    newfields[newY, x] |= 1;
                }
                if ((fields[y, x] & 2) == 2)
                {
                    int newX = x + 1;
                    if (newX == xCnt) newX = 0;
                    newfields[y, newX] |= 2;
                }
                if ((fields[y, x] & 4) == 4)
                {
                    int newY = y + 1;
                    if (newY == yCnt) newY = 0;
                    newfields[newY, x] |= 4;
                }
                if ((fields[y, x] & 8) == 8)
                {
                    int newX = x - 1;
                    if (newX < 0) newX = xCnt - 1;
                    newfields[y, newX] |= 8;
                }
            }
        }
        fields = newfields;
    }

    int startX = input[0].IndexOf('.') - 1;
    int endX = input[^1].IndexOf('.') - 1;
    int startY = -1;
    int endY = yCnt;

    var work = new HashSet<(int y, int x, int state, (int y, int x)[] path)>();

    var visited = new bool[yCnt, xCnt, lcm];
    int[,,] shortest = EmptyLookup(xCnt, yCnt, lcm);

    int firstpos = 1;
    var l = new List<(int y, int x)>();
    (int y, int x)[] path = FindPath(ref answer1, xCnt, yCnt, lcm, isFree, startX, endX, startY, endY, shortest, work, visited, ref firstpos, ref l);
    fields = PrintPath(input, answer1, xCnt, yCnt, fields, i, path);

    visited = new bool[yCnt, xCnt, lcm];
    shortest = EmptyLookup(xCnt, yCnt, lcm);

    firstpos = answer1 + 1;
    l = new List<(int y, int x)>(path);
    path = FindPath(ref answer2, xCnt, yCnt, lcm, isFree, endX, startX, endY, startY, shortest, work, visited, ref firstpos, ref l);
    fields = PrintPath(input, answer2, xCnt, yCnt, fields, i, path);

    visited = new bool[yCnt, xCnt, lcm];
    shortest = EmptyLookup(xCnt, yCnt, lcm);

    firstpos = answer2 + 1;
    l = new List<(int y, int x)>(path);
    path = FindPath(ref answer2, xCnt, yCnt, lcm, isFree, startX, endX, startY, endY, shortest, work, visited, ref firstpos, ref l);
    fields = PrintPath(input, answer2, xCnt, yCnt, fields, i, path);

    Console.SetCursorPosition(0, 1 + yCnt + 2);

    WriteAnswer(1, answer1, ExampleAnswer1, isTest);
    WriteAnswer(2, answer2, ExampleAnswer2, isTest);

    Console.WriteLine("Press any key");
    Console.ReadKey();

    static void CheckCell(bool[,,] isFree, int[,,] shortest, bool[,,] visited, HashSet<(int y, int x, int state, (int y, int x)[] path)> work, int nextCost, (int y, int x, int state, (int y, int x)[] path) cNew)
    {
        if (!visited[cNew.y, cNew.x, cNew.state] &&
            isFree[cNew.y, cNew.x, cNew.state] &&
            shortest[cNew.y, cNew.x, cNew.state] > nextCost)
        {
            work.Add(cNew);
            shortest[cNew.y, cNew.x, cNew.state] = nextCost;
        }
    }

    static (int y, int x)[] FindPath(ref int answer1, int xCnt, int yCnt, int numstates, bool[,,] isFree, int startX, int endX, int startY, int endY, int[,,] shortest, HashSet<(int y, int x, int state, (int y, int x)[] path)> work, bool[,,] visited, ref int firstpos, ref List<(int y, int x)> l)
    {
        int stopY = endY == -1 ? 0 : endY - 1;

        var path = l.ToArray();
        var endPos = firstpos + numstates;
        while (firstpos <= endPos)
        {
            var pos = firstpos % numstates;
            l.Add((startY, startX));
            int y = startY == -1 ? 0 : startY - 1;

            if (isFree[y, startX, pos])
            {
                work.Add((y, startX, pos, l.ToArray()));
                shortest[y, startX, pos] = (int)firstpos;
            }
            firstpos++;
        }

        while (work.Count > 0)
        {
            var cw = work.OrderBy(c => shortest[c.y, c.x, c.state]).First();
            work.Remove(cw);
            visited[cw.y, cw.x, cw.state] = true;

            int nextstate = (cw.state + 1) % numstates;
            int nextCost = shortest[cw.y, cw.x, cw.state] + 1;

            l = new List<(int y, int x)>(cw.path);
            l.Add((cw.y, cw.x));

            if (cw.y == stopY && cw.x == endX)
            {
                answer1 = nextCost;
                path = l.ToArray();
                break;
            }

            (int y, int x, int state, (int y, int x)[] path) cNew = (cw.y, cw.x, nextstate, l.ToArray());
            CheckCell(isFree, shortest, visited, work, nextCost, cNew);
            if (cw.y > 0)
            {
                cNew = (cw.y - 1, cw.x, nextstate, l.ToArray());
                CheckCell(isFree, shortest, visited, work, nextCost, cNew);
            }
            if (cw.x < xCnt - 1)
            {
                cNew = (cw.y, cw.x + 1, nextstate, l.ToArray());
                CheckCell(isFree, shortest, visited, work, nextCost, cNew);
            }
            if (cw.y < yCnt - 1)
            {
                cNew = (cw.y + 1, cw.x, nextstate, l.ToArray());
                CheckCell(isFree, shortest, visited, work, nextCost, cNew);
            }
            if (cw.x > 0)
            {
                cNew = (cw.y, cw.x - 1, nextstate, l.ToArray());
                CheckCell(isFree, shortest, visited, work, nextCost, cNew);
            }
        }
        return path;
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
}

static byte[,] PrintPath(List<string> S, int answer1, int xCnt, int yCnt, byte[,] fields, int i, (int y, int x)[] path)
{
    int k = 1;
    while (k < S.Count - 1)
    {
        var s = S[k];
        for (int j = 1; j < s.Length - 1; j++)
        {
            byte val = 0;
            switch (s[j])
            {
                case '^': val = 1; break;
                case '>': val = 2; break;
                case 'v': val = 4; break;
                case '<': val = 8; break;
            }
            fields[k - 1, j - 1] = val;
        }

        k++;
    }

    for (int j = 0; j < answer1; j++)
    {
        var newfields = new byte[yCnt, xCnt];

        for (int y = 0; y < yCnt; y++)
        {
            for (int x = 0; x < xCnt; x++)
            {
                if ((fields[y, x] & 1) == 1)
                {
                    int newY = y - 1;
                    if (newY < 0) newY = yCnt - 1;
                    newfields[newY, x] |= 1;
                }
                if ((fields[y, x] & 2) == 2)
                {
                    int newX = x + 1;
                    if (newX == xCnt) newX = 0;
                    newfields[y, newX] |= 2;
                }
                if ((fields[y, x] & 4) == 4)
                {
                    int newY = y + 1;
                    if (newY == yCnt) newY = 0;
                    newfields[newY, x] |= 4;
                }
                if ((fields[y, x] & 8) == 8)
                {
                    int newX = x - 1;
                    if (newX < 0) newX = xCnt - 1;
                    newfields[y, newX] |= 8;
                }
            }
        }

        Console.SetCursorPosition(0, 0);
        Console.Write(S[0]);
        Console.WriteLine($" Minute: {j}");
        for (int y1 = 0; y1 < yCnt; y1++)
        {
            Console.Write('#');
            for (int x = 0; x < xCnt; x++)
            {
                if (path[j].y == y1 && path[j].x == x)
                {
                    var oldc = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('E');
                    Console.ForegroundColor = oldc;
                    Debug.Assert(fields[y1, x] == 0);
                }
                else
                {
                    switch (fields[y1, x])
                    {
                        case 0: Console.Write('.'); break;
                        case 1: Console.Write('^'); break;
                        case 2: Console.Write('>'); break;
                        case 3: Console.Write('2'); break;
                        case 4: Console.Write('v'); break;
                        case 5: Console.Write('2'); break;
                        case 6: Console.Write('2'); break;
                        case 7: Console.Write('3'); break;
                        case 8: Console.Write('<'); break;
                        case 9: Console.Write('2'); break;
                        case 10: Console.Write('2'); break;
                        case 11: Console.Write('3'); break;
                        case 12: Console.Write('2'); break;
                        case 13: Console.Write('3'); break;
                        case 14: Console.Write('3'); break;
                        case 15: Console.Write('2'); break;
                    }
                }
            }
            Console.WriteLine('#');
        }
        Console.Write(S[^1]);
        fields = newfields;
        Thread.Sleep(20);
    }
    return fields;
}

static int[,,] EmptyLookup(int xCnt, int yCnt, int numstates)
{
    var shortest = new int[yCnt, xCnt, numstates];
    for (int a = 0; a < yCnt; a++)
        for (int b = 0; b < xCnt; b++)
            for (int c = 0; c < numstates; c++)
                shortest[a, b, c] = int.MaxValue;
    return shortest;
}