using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;

Day10.Part1();
Day10.Part2();

public class Monkey
{
    public Monkey(List<long> items, string operation, int divBy, int ifTrue, int ifFalse)
    {
        Items = new Queue<long>(items);
        Operation = operation;
        DivBy = divBy;
        IfTrueTarget = ifTrue;
        IfFalseTarget = ifFalse;
    }

    public Queue<long> Items { get; set; }
    public string Operation { get; set; }
    public int DivBy { get; set; }
    public int IfTrueTarget { get; set; }
    public int IfFalseTarget { get; set; }
    public long InspectCnt { get; set; } = 0;
}
public static class Day10
{
    static List<Monkey> monkeys = new List<Monkey>();

    public static void Part1()
    {
        Console.WriteLine("Part 1");

        Parse();
        for (int i = 0; i < 20; i++)
        {
            ProcessRound1();
        }
        var top = monkeys.OrderByDescending(n => n.InspectCnt).ToList();
        Console.WriteLine("Ans: " + (top[0].InspectCnt * top[1].InspectCnt));
        Console.WriteLine();
    }

    public static void Part2()
    {
        Console.WriteLine("Part 2");

        Parse();
        int lcm = 1;
        foreach (Monkey monkey in monkeys)
        {
            lcm *= monkey.DivBy;
        }
        Console.WriteLine("lcm : " + lcm);

        for (int i = 0; i < 10000; i++)
        {
            ProcessRound2(lcm);
        }

        var top = monkeys.OrderByDescending(n => n.InspectCnt).ToList();
        Console.WriteLine("Ans: " + (top[0].InspectCnt * top[1].InspectCnt ));
        Console.WriteLine();
    }

    public static void ProcessRound1()
    {
        foreach (Monkey monkey in monkeys)
        {
            while (monkey.Items.Count > 0)
            {
                monkey.InspectCnt++;
                long newItem = monkey.Items.Dequeue();
                if (monkey.Operation.Contains("+"))
                {
                    newItem += long.Parse(monkey.Operation[1..]);
                }
                else if (monkey.Operation.Contains("old"))
                {
                    newItem *= newItem;
                }
                else
                {
                    newItem *= long.Parse(monkey.Operation[1..]);
                }

                newItem = newItem / 3;

                if (newItem % monkey.DivBy == 0)
                {
                    monkeys[monkey.IfTrueTarget].Items.Enqueue(newItem);
                }
                else
                {
                    monkeys[monkey.IfFalseTarget].Items.Enqueue(newItem);
                }
            }
        }
    }

    public static void ProcessRound2(int lcm)
    {
        foreach (Monkey monkey in monkeys)
        {
            while (monkey.Items.Count > 0)
            {
                monkey.InspectCnt++;
                long newItem = monkey.Items.Dequeue();
                if (monkey.Operation.Contains("+"))
                {
                    newItem += long.Parse(monkey.Operation[1..]);
                    newItem %= lcm;
                }
                else if (monkey.Operation.Contains("old"))
                {
                    newItem *= newItem;
                    newItem %= lcm;
                }
                else
                {
                    newItem *= long.Parse(monkey.Operation[1..]);
                    newItem %= lcm;
                }

                //newItem = newItem / 3;

                if (newItem % monkey.DivBy == 0)
                {
                    monkeys[monkey.IfTrueTarget].Items.Enqueue(newItem);
                }
                else
                {
                    monkeys[monkey.IfFalseTarget].Items.Enqueue(newItem);
                }
            }
        }
    }

    private static void Parse()
    {
        monkeys.Clear();
        File.ReadAllLines("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day11\\input11.txt")
            .Chunk(7).ToList()
            .ForEach(line =>
            {
                var items = line[1].Trim().Replace("Starting items: ", "").Split(", ").Select(long.Parse)
                    .ToList();
                var operation = line[2].Trim().Replace("Operation: new = old ", "");
                var divBy = int.Parse(line[3].Trim().Replace("Test: divisible by ", ""));
                var ifTrueTarget = int.Parse(line[4].Trim().Replace("If true: throw to monkey ", ""));
                var ifFalseTarget = int.Parse(line[5].Trim().Replace("If false: throw to monkey ", ""));

                var monkey = new Monkey(items, operation, divBy, ifTrueTarget, ifFalseTarget);
                monkeys.Add(monkey);
            });
    }
}