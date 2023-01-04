using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day19\\example19.txt", true);
Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day19\\input19.txt", false);
 
void Run(string path, bool isTest)
{
    // for Testing
    var exampleAnswer1 = 33;
    var exampleAnswer2 = 3472;

    var answer1 = 0L;
    var answer2 = 1L;

    var blueprints = parse(path);

    foreach (var blueprint in blueprints)
    {
        var blueprintResult = GetMostGeodes(blueprint, 24);
        answer1 += blueprint.id * blueprintResult;
    }

    WriteAnswer(1, answer1, exampleAnswer1, isTest);


    foreach (var blueprint in blueprints.Take(3))
    {
        var blueprintResult = GetMostGeodes(blueprint, 32);
        answer2 *= blueprintResult;
    }

    WriteAnswer(2, answer2, exampleAnswer2, isTest);
}

static List<Blueprint> parse(string path)
{
    var blueprints = new List<Blueprint>();

    foreach (var line in File.ReadAllLines(path))
    {
        if (string.IsNullOrWhiteSpace(line))
            continue;
        var nline = line.Replace("Blueprint ", "").Replace(": Each ore robot costs ", ",").Replace(" ore. Each clay robot costs ", ",").Replace(" ore. Each obsidian robot costs ", ",").Replace(" ore and ", ",").Replace(" clay. Each geode robot costs ", ",").Replace(" obsidian.", ",").Split(',');
        blueprints.Add(new Blueprint(
            int.Parse(nline[0]),                                    
            int.Parse(nline[1]),                                    
            int.Parse(nline[2]),                                    
            int.Parse(nline[3]), int.Parse(nline[4]),  
            int.Parse(nline[5]), int.Parse(nline[6])   
        ));
    }
    return blueprints;
}

int GetMostGeodes(Blueprint blueprint, int time)
{
    var alreadyRun = new HashSet<Status>();
    var toRun = new Stack<Status>();
    toRun.Push(new Status(0, 1, 0, 0, 0, 0, 0, 0, time));

    var best = 0;

    while (toRun.TryPop(out var status))
    {
        if (status.timeLeft == 0)
        {
            best = Math.Max(best, status.geodes);
            continue;
        }

        status.oreMachines = Math.Min(status.oreMachines, blueprint.MaxOreCost);
        status.ore = Math.Min(status.ore, status.timeLeft * blueprint.MaxOreCost - status.oreMachines * (status.timeLeft - 1));
        status.clayMachines = Math.Min(status.clayMachines, blueprint.obsidianCostClay);
        status.clay = Math.Min(status.clay, status.timeLeft * blueprint.obsidianCostClay - status.clayMachines * (status.timeLeft - 1));
        status.obsidianMachines = Math.Min(status.obsidianMachines, blueprint.geodeCostObsidian);
        status.obsidian = Math.Min(status.obsidian, status.timeLeft * blueprint.geodeCostObsidian - status.obsidianMachines * (status.timeLeft - 1));

        if (alreadyRun.Contains(status))
            continue;

        alreadyRun.Add(status);

        // geode machine
        if (status.ore >= blueprint.geodeCostOre && status.obsidian >= blueprint.geodeCostObsidian)
            toRun.Push(new Status(
                status.ore + status.oreMachines - blueprint.geodeCostOre, status.oreMachines,
                status.clay + status.clayMachines, status.clayMachines,
                status.obsidian + status.obsidianMachines - blueprint.geodeCostObsidian, status.obsidianMachines,
                status.geodes + status.geodeMachines, status.geodeMachines + 1,
                status.timeLeft - 1));

        // obsidian machine
        if (status.ore >= blueprint.obsidianCostOre && status.clay >= blueprint.obsidianCostClay)
            toRun.Push(new Status(
                status.ore + status.oreMachines - blueprint.obsidianCostOre, status.oreMachines,
                status.clay + status.clayMachines - blueprint.obsidianCostClay, status.clayMachines,
                status.obsidian + status.obsidianMachines, status.obsidianMachines + 1,
                status.geodes + status.geodeMachines, status.geodeMachines,
                status.timeLeft - 1));

        // clay machine
        if (status.ore >= blueprint.clayCostOre)
            toRun.Push(new Status(
                status.ore + status.oreMachines - blueprint.clayCostOre, status.oreMachines,
                status.clay + status.clayMachines, status.clayMachines + 1,
                status.obsidian + status.obsidianMachines, status.obsidianMachines,
                status.geodes + status.geodeMachines, status.geodeMachines,
                status.timeLeft - 1));

        // ore machine
        if (status.ore >= blueprint.oreCostOre)
            toRun.Push(new Status(
                status.ore + status.oreMachines - blueprint.oreCostOre, status.oreMachines + 1,
                status.clay + status.clayMachines, status.clayMachines,
                status.obsidian + status.obsidianMachines, status.obsidianMachines,
                status.geodes + status.geodeMachines, status.geodeMachines,
                status.timeLeft - 1));

        // do nothing
        toRun.Push(new Status(
            status.ore + status.oreMachines, status.oreMachines,
            status.clay + status.clayMachines, status.clayMachines,
            status.obsidian + status.obsidianMachines, status.obsidianMachines,
            status.geodes + status.geodeMachines, status.geodeMachines,
            status.timeLeft - 1));
    }

    return best;
}

void WriteAnswer<T>(int number, T val, T supposedval, bool isTest)
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


class Status
{
    public int ore;
    public int oreMachines;
    public int clay;
    public int clayMachines;
    public int obsidian;
    public int obsidianMachines;
    public int geodes;
    public int geodeMachines;
    public int timeLeft;

    public Status(int ore, int oreMachines, int clay, int clayMachines, int obsidian, int obsidianMachines, int geodes, int geodeMachines, int timeLeft)
    {
        this.ore = ore;
        this.oreMachines = oreMachines;
        this.clay = clay;
        this.clayMachines = clayMachines;
        this.obsidian = obsidian;
        this.obsidianMachines = obsidianMachines;
        this.geodes = geodes;
        this.geodeMachines = geodeMachines;
        this.timeLeft = timeLeft;
    }

    public override int GetHashCode() =>
        HashCode.Combine(ore, oreMachines, clay, clayMachines, obsidian, obsidianMachines, geodes, geodeMachines);

    public override bool Equals(object? obj)
    {
        if (obj is not Status other)
            return false;

        return
            ore == other.ore &&
            oreMachines == other.oreMachines &&
            clay == other.clay &&
            clayMachines == other.clayMachines &&
            obsidian == other.obsidian &&
            obsidianMachines == other.obsidianMachines &&
            geodes == other.geodes &&
            geodeMachines == other.geodeMachines &&
            timeLeft == other.timeLeft;
    }
}

record class Blueprint(int id, int oreCostOre, int clayCostOre, int obsidianCostOre, int obsidianCostClay, int geodeCostOre, int geodeCostObsidian)
{
    Lazy<int> maxOreCost = new(() => new[] { oreCostOre, clayCostOre, obsidianCostOre, geodeCostOre }.Max());

    public int MaxOreCost => maxOreCost.Value;
}

