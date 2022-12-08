var inputFile = File.ReadAllLines("C:\\Users\\henri\\source\\repos\\AoC\\day04\\input04.txt");
var input = new List<string>(inputFile);

void Part1()
{
    var enclosingPairs = 0;
    foreach (var line in input)
    {
        var assignments = line.Split(',');
        var firstStartRoom = int.Parse(assignments[0].Split("-")[0]);
        var firstEndRoom = int.Parse(assignments[0].Split("-")[1]);
        var secondStartRoom = int.Parse(assignments[1].Split("-")[0]);
        var secondEndRoom = int.Parse(assignments[1].Split("-")[1]);
        var firstTotalRooms = Enumerable.Range(firstStartRoom, firstEndRoom - firstStartRoom + 1);
        var secondTotalRooms = Enumerable.Range(secondStartRoom, secondEndRoom - secondStartRoom + 1);
        if (firstTotalRooms.All(x => secondTotalRooms.Contains(x)) || secondTotalRooms.All(x => firstTotalRooms.Contains(x)))
        {
            enclosingPairs++;
        }
    }
    Console.WriteLine(enclosingPairs.ToString());
}

void Part2()
{
    var enclosingPairs = 0;
    foreach (var line in input)
    {
        var assignments = line.Split(',');
        var firstStartRoom = int.Parse(assignments[0].Split("-")[0]);
        var firstEndRoom = int.Parse(assignments[0].Split("-")[1]);
        var secondStartRoom = int.Parse(assignments[1].Split("-")[0]);
        var secondEndRoom = int.Parse(assignments[1].Split("-")[1]);
        var firstTotalRooms = Enumerable.Range(firstStartRoom, firstEndRoom - firstStartRoom + 1);
        var secondTotalRooms = Enumerable.Range(secondStartRoom, secondEndRoom - secondStartRoom + 1);
        if (firstTotalRooms.Any(x => secondTotalRooms.Contains(x)) || secondTotalRooms.Any(x => firstTotalRooms.Contains(x)))
        {
            enclosingPairs++;
        }
    }
    Console.WriteLine(enclosingPairs.ToString());
}

Part2();