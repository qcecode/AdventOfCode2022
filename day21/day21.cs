using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static System.Console;

Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day21\\example21.txt", true);
Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day21\\input21.txt", false);
static void Run(string inputString, bool isTest)
{
    // For testing
    long supposedanswer1 = 152;
    long supposedanswer2 = 301;


    // Parse input
    var numbers = new Dictionary<string, long>();
    var waiting = new List<(string name, string a, string operation, string b)>();

    var input = File.ReadAllLines(inputString).ToList();

    foreach (var line in input)
    {
        string monkey = line.Substring(0, 4);
        string rest = line.Substring(6);

        if (char.IsDigit(rest[0]))
        {
            numbers.Add(monkey, long.Parse(rest));
        }
        else
        {
            string[] split = rest.Split(' ');
            waiting.Add((monkey, split[0], split[1], split[2]));
        }
    }

    // Compute and print result for Part 1
    var numbersBase = new Dictionary<string, long>(numbers);
    numbersBase.Remove("humn");
    var waitingBase = new List<(string name, string a, string operation, string b)>(waiting);
    var root = waitingBase.First(w => w.name == "root");
    waitingBase.Remove(root);

    while (waiting.Count > 0)
    {
        var item = waiting.First(w => numbers.ContainsKey(w.a) && numbers.ContainsKey(w.b));
        waiting.Remove(item);

        numbers[item.name] = Compute(item.operation, numbers[item.a], numbers[item.b]);
    }

    WriteAnswer(1, numbers["root"], supposedanswer1, isTest);

    // Compute and print result for Part 2
    long crack = 0;
    while (true)
    {
        numbers = new Dictionary<string, long>(numbersBase);
        waiting = new List<(string name, string a, string operation, string b)>(waitingBase);
        numbers["humn"] = crack;

        while (waiting.Count > 0)
        {
            var item = waiting.First(w => numbers.ContainsKey(w.a) && numbers.ContainsKey(w.b));
            waiting.Remove(item);

            numbers[item.name] = Compute(item.operation, numbers[item.a], numbers[item.b]);
        }

        if (numbers[root.a] == numbers[root.b])
        {
            break;
        }

        long diff = numbers[root.a] - numbers[root.b];
        if (diff < 100)
        {
            crack++;
        }
        else
        {
            crack += diff / 100;
        }
    }

    WriteAnswer(2, crack, supposedanswer2, isTest);
}

static long Compute(string operation, long a, long b)
{
    switch (operation)
    {
        case "+":
            return a + b;
        case "-":
            return a - b;
        case "*":
            return a * b;
        case "/":
            return a / b;
        default:
            throw new Exception();
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


