using System.Collections.Generic;
using System.Diagnostics;

Day20.Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day20\\example20.txt", true);
Day20.Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day20\\input20.txt", false);

public static class Day20
{
    public static void Run(string inputPath, bool isTest)
    {

        const int ExampleAnswer1 = 3;
        const int ExampleAnswer2 = 1623178306;

        var input = File.ReadAllLines(inputPath).ToList();

        var answer1 = MixValues(input);
        var answer2 = MixValues(input, 811589153L, 10);

        WriteAnswer(1, answer1, ExampleAnswer1, isTest);
        WriteAnswer(2, answer2, ExampleAnswer2, isTest);

    }

    internal static Int64 MixValues(List<string> input, Int64 decryptionKey = 1, int mixCount = 1)
    {
        var parsedInput = input.Select(e => Int64.Parse(e) * decryptionKey).ToList();
        var encryptedValues = new List<(Int64 value, int index)>();

        for (int i = 0; i < parsedInput.Count; i++)
            encryptedValues.Add((parsedInput[i], i));

        var mixedValues = new List<(Int64 value, int index)>(encryptedValues);
        var count = encryptedValues.Count;

        for (int mc = 0; mc < mixCount; mc++)
        {
            for (int i = 0; i < count; i++)
            {
                var number = encryptedValues[i];
                var oldIndex = mixedValues.IndexOf(number);
                var newIndex = (oldIndex + number.value) % (count - 1);

                if (newIndex < 0)
                    newIndex = count + newIndex - 1;

                mixedValues.Remove(number);
                mixedValues.Insert((int)newIndex, number);
            }
        }

        var indexZero = mixedValues.FindIndex(e => e.value == 0);
        var index1000 = (1000 + indexZero) % count;
        var index2000 = (2000 + indexZero) % count;
        var index3000 = (3000 + indexZero) % count;

        var coordinatesSum = mixedValues[index1000].value + mixedValues[index2000].value + mixedValues[index3000].value;

        return coordinatesSum;
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
}