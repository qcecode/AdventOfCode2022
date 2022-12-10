using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;

Day10.Part1();
Day10.Part2();

public static class Day10
{
    static IEnumerable<String> inputFile = File.ReadLines("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day10\\input10.txt");
    static List<string> input = new List<string>(inputFile);

    static Dictionary<int,int> dic = new Dictionary<int,int>();

    public static void Part1()
    {
        int signalSum = 0;

        GenDic();

        signalSum += (dic.GetValueOrDefault(20) * 20);
        signalSum += (dic.GetValueOrDefault(60) * 60);
        signalSum += (dic.GetValueOrDefault(100) * 100);
        signalSum += (dic.GetValueOrDefault(140) * 140);
        signalSum += (dic.GetValueOrDefault(180) * 180);
        signalSum += (dic.GetValueOrDefault(220) * 220);
        Console.WriteLine($"Sum of these six signal strengths: {signalSum}");
    }

    public static void Part2()
    {
        Console.WriteLine();
        Console.WriteLine("Part 2 ");
        Console.WriteLine();

        int pixelKey = 0;
        for (int i = 0; i < 6; i++)
        {
            for (int k = 0; k < 40; k++)
            {
                pixelKey = i * 40 + k + 1;
                if(dic.GetValueOrDefault(pixelKey) == k-1   ||
                   dic.GetValueOrDefault(pixelKey) == k     ||
                   dic.GetValueOrDefault(pixelKey) == k +1  )
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }       
            }
            Console.WriteLine();
        }
    }

    private static void PrintDic()
    {
        foreach (KeyValuePair<int, int> kvp in dic)
        {
            Console.WriteLine(string.Format("cycle = {0}, x = {1}", kvp.Key, kvp.Value));
        }
    }

    private static void GenDic()
    {
        int cycle = 1; int x = 1;

        foreach (string line in input)
        {
            if (line.Contains("addx"))
            {
                dic.Add(cycle, x);
                dic.Add(cycle + 1, x);
                x += int.Parse(line[5..]);
                cycle += 2;
            }
            else
            {
                dic.Add(cycle, x);
                cycle++;
            }
        }
    }
}