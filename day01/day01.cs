var inputFile = File.ReadLines("C:\\Users\\User\\source\\repos\\AdventOfCode2022\\day01\\input01.txt");
var input = new List<string>(inputFile);


void Part1()
{
    int maxCal = 0;
    int currentCal = 0;

    foreach (var line in input)
    {
        if (line == "")
        {
            if (currentCal > maxCal)
            {
                maxCal = currentCal;
            }
            currentCal = 0;
        }
        else
        {
            currentCal += Int32.Parse(line);
        }
    }
    Console.WriteLine(maxCal);
}

void Part2()
{
    int[] maxCalArr = new int[3];
    int currentCal = 0;

    foreach (var line in input)
    {
        if (line == "")
        {
            for (int i = 2; i >= 0; i--)
            {
                if (currentCal > maxCalArr[i])
                {
                    if (i != 2)
                    {
                        maxCalArr[i + 1] = maxCalArr[i];
                    }
                    maxCalArr[i] = currentCal;
                }
            }
            currentCal = 0;
        }
        else
        {
            currentCal += Int32.Parse(line);
        }
    }

    int totalCal = 0;

    foreach (int num in maxCalArr)
    {
        totalCal += num;
    }
    Console.WriteLine(totalCal.ToString());
}

Part2();