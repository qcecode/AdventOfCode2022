var inputFile = File.ReadLines("C:\\Users\\User\\source\\repos\\AdventOfCode2022\\day05\\input05.txt");
var input = new List<string>(inputFile);


void Solve()
{
    Dictionary<int, List<char>> cargoStacks = new Dictionary<int, List<char>>();
    Dictionary<int, List<char>> cargoStacks2 = new Dictionary<int, List<char>>();
    foreach (var s in input)
    {
        if (s.Contains('['))
        {
            int col = 1;
            for (int i = 0; i < s.Length; i += 4)
            {
                if (!cargoStacks.ContainsKey(col))
                {
                    if (s[i + 1] != ' ')
                    {
                        cargoStacks.Add(col, new List<char> { s[i + 1] });
                        cargoStacks2.Add(col, new List<char> { s[i + 1] });
                    }
                }
                else
                {
                    if (s[i + 1] != ' ')
                    {
                        cargoStacks[col].Add(s[i + 1]);
                        cargoStacks2[col].Add(s[i + 1]);
                    }
                }
                col++;
            }
        }
        else
        {
            if (s.Contains("move"))
            {
                // 0 = numToMove
                // 1 = colToMoveFrom
                // 2 = colToMoveTo

                string[] moves = s.Split(new string[] { "move ", " from ", " to " }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < int.Parse(moves[0]); i++)
                {
                    cargoStacks[int.Parse(moves[2])].Insert(0, cargoStacks[int.Parse(moves[1])][i]);
                }

                cargoStacks[int.Parse(moves[1])].RemoveRange(0, int.Parse(moves[0]));


                for (int i = int.Parse(moves[0]) - 1; i >= 0; i--)
                {
                    cargoStacks2[int.Parse(moves[2])].Insert(0, cargoStacks2[int.Parse(moves[1])][i]);
                }

                cargoStacks2[int.Parse(moves[1])].RemoveRange(0, int.Parse(moves[0]));

            }
        }
    }

    char[] part1 = new char[9];
    char[] part2 = new char[9];


    foreach (KeyValuePair<int, List<char>> kvp in cargoStacks)
    {
        part1[kvp.Key - 1] = kvp.Value[0];
    }

    foreach (KeyValuePair<int, List<char>> kvp in cargoStacks2)
    {
        part2[kvp.Key - 1] = kvp.Value[0];
    }
    Console.Write("Part1: ");
    foreach (char c in part1)
    {
        Console.Write(c);
    }
    Console.WriteLine();
    Console.Write("Part2: ");
    foreach (char c in part2)
    {
        Console.Write(c);
    }
}

Solve();

