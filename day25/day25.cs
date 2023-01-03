using System.Diagnostics;

Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day25\\example25.txt", true);
Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day25\\input25.txt", false);

void Run(string path, bool isTest)
{
    string ExampleAnswer1 = "2=-1=0";
    //string ExampleAnswer2 = "";

    var input = File.ReadAllLines(path).ToList();
    var numbers = new List<Int64>();

    Process(input, numbers);

    var result = numbers.Sum();
    var resultToPrint = result;
    var SNAFUResult = "";

    while (result > 0)
    {
        var number = result % 5;
        result /= 5;

        switch (number)
        {
            case 0:
                SNAFUResult = "0" + SNAFUResult;
                break;
            case 1:
                SNAFUResult = "1" + SNAFUResult;
                break;
            case 2:
                SNAFUResult = "2" + SNAFUResult;
                break;
            case 3:
                SNAFUResult = "=" + SNAFUResult;
                result++;
                break;
            case 4:
                SNAFUResult = "-" + SNAFUResult;
                result++;
                break;
            default:
                throw new Exception();
        }
    }
    Console.WriteLine($"Fuel requirements sum: {resultToPrint} | SNAFU sum: {SNAFUResult}");
    WriteAnswer(1, SNAFUResult, ExampleAnswer1, isTest);
    Console.WriteLine();
}

static void Process(List<string> input, List<long> numbers)
{
    for (int i = 0; i < input.Count; i++)
    {
        var numberString = input[i].Reverse();
        Int64 number = 0;

        for (int j = numberString.Count() - 1; j >= 0; j--)
        {
            var multiplier = (Int64)Math.Pow(5, j);

            if (numberString.ElementAt(j) == '=')
                number += multiplier * (-2);
            else if (numberString.ElementAt(j) == '-')
                number += multiplier * (-1);
            else
            {
                var value = (Int64)Char.GetNumericValue(numberString.ElementAt(j));
                number += multiplier * value;
            }
        }

        numbers.Add(number);
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
    Console.ForegroundColor = ConsoleColor.White;
}
