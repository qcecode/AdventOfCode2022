using System.Diagnostics;

Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day25\\example25.txt", true);
Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day25\\input25.txt", false);

void Run(string path, bool isTest)
{
    // For Testing
    string exampleAnswer1 = "2=-1=0";

    var input = File.ReadAllLines(path).ToList();
    var numbers = new List<long>();

    Process(input, numbers);

    var result = numbers.Sum();
    var resultToPrint = result;
    var snafuResult = "";

    for (int i = 0; result > 0; i++)
    {
        var number = result % 5;
        result /= 5;

        switch (number)
        {
            case 0:
                snafuResult = "0" + snafuResult;
                break;
            case 1:
                snafuResult = "1" + snafuResult;
                break;
            case 2:
                snafuResult = "2" + snafuResult;
                break;
            case 3:
                snafuResult = "=" + snafuResult;
                result++;
                break;
            case 4:
                snafuResult = "-" + snafuResult;
                result++;
                break;
            default:
                throw new Exception("Invalid number.");
        }
    }
    WriteAnswer(1, snafuResult, exampleAnswer1, isTest);
}

// Extract the logic for calculating the multiplier and the logic for updating the number variable into separate helper functions
static void Process(List<string> input, List<long> numbers)
{
    for (int i = 0; i < input.Count; i++)
    {
        long number = 0;
        for (int j = 0; j < input[i].Length; j++)
        {
            var multiplier = CalculateMultiplier(input[i].Length, j);
            number = UpdateNumber(input[i][j], multiplier, number);
        }
        numbers.Add(number);
    }
}

// Helper function to calculate the multiplier
static long CalculateMultiplier(int length, int j)
{
    return (long)Math.Pow(5, length - 1 - j);
}

// Helper function to update the number variable
static long UpdateNumber(char c, long multiplier, long number)
{
    switch (c)
    {
        case '=':
            number += multiplier * (-2);
            break;
        case '-':
            number += multiplier * (-1);
            break;
        default:
            var value = (long)Char.GetNumericValue(c);
            number += multiplier * value;
            break;
    }
    return number;
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
    Console.ForegroundColor = ConsoleColor.White;
}
