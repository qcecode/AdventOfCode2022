var inputFile = File.ReadAllLines("C:\\Users\\User\\source\\repos\\AdventOfCode2022\\day06\\input06.txt");
//var input = new List<string>(inputFile);

int part1()
{
    char[] allChars = inputFile[0].ToCharArray();

    for (int i = 0; i < allChars.Length; i++)
    {
        char[] charWindow = allChars[i..(i + 4)];
        HashSet<char> hashSet = new HashSet<char>(charWindow);
        if (hashSet.Count() == 4)
        {
            return i + 4;
        }
    }
    return 0;
}

int part2()
{
    char[] allChars = inputFile[0].ToCharArray();

    for (int i = 0; i < allChars.Length; i++)
    {
        // only diff +14 rather than +4
        char[] charWindow = allChars[i..(i + 14)];
        HashSet<char> hashSet = new HashSet<char>(charWindow);
        if (hashSet.Count() == 14)
        {
            return i + 14;
        }
    }
    return 0;
}


Console.Write("Part 1: ");
Console.WriteLine(part1().ToString());
Console.Write("Part 2: ");
Console.WriteLine(part2().ToString());