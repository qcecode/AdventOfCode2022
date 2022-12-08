var inputFile = File.ReadAllLines("C:\\Users\\henri\\source\\repos\\AoC\\day03\\input03.txt");
var input = new List<string>(inputFile);

void Part1()
{
    var properties = 0;
    foreach (var bag in input)
    {
        var firstComparment = bag[..(bag.Length / 2)].ToCharArray();
        var secondComparment = bag[(bag.Length / 2)..].ToCharArray();
        var match = firstComparment.Intersect(secondComparment).ToArray()[0];
        properties += char.IsUpper(match) ? match - 'A' + 27 : match - 'a' + 1;
    }
    Console.WriteLine(properties.ToString());
}

void Part2()
{
    var properties = 0;
    for (var x = 0; x < input.Count; x += 3)
    {
        var firstBag = input[x].ToCharArray();
        var seconBag = input[x + 1].ToCharArray();
        var thirdBag = input[x + 2].ToCharArray();
        var match = firstBag.Intersect(seconBag.Intersect(thirdBag)).ToArray()[0];
        properties += char.IsUpper(match) ? match - 'A' + 27 : match - 'a' + 1;
    }
    Console.WriteLine(properties.ToString());
}

Part2();
    