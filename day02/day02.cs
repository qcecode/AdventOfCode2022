var inputFile = File.ReadAllLines("C:\\Users\\henri\\source\\repos\\AoC\\AoC\\input02.txt");
var input = new List<string>(inputFile);
//var input = File.ReadAllText("C:\\Users\\henri\\source\\repos\\AoC\\AoC\\Day02\\input02.txt");

void Part1()
{
    int score = 0;
    foreach (string line in input)
    {
        if (line is "A Y" or "B Z" or "C X")
            score += 6;
        if (line is "A X" or "B Y" or "C Z")
            score += 3;

        if (line.Contains('X'))
            score += 1;
        if (line.Contains('Y'))
            score += 2;
        if (line.Contains('Z'))
            score += 3;
    }
    Console.WriteLine(score.ToString());
}

void Part2()
{
    int score = 0;
    foreach (string line in input)
    {
        if (line.Contains('X'))
        {
            score += line[0] == 'A' ? 3 : line[0] == 'B' ? 1 : 2;
        }
        if (line.Contains('Y'))
        {
            score += line[0] == 'A' ? 4 : line[0] == 'B' ? 5 : 6;
        }
        if (line.Contains('Z'))
        {
            score += line[0] == 'A' ? 8 : line[0] == 'B' ? 9 : 7;
        }
    }
    Console.WriteLine(score.ToString());
}

Part2();
