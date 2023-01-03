using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;

Day16 day16 = new Day16();
day16.Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day16\\example16.txt", true);
day16.Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day16\\input16.txt", false);

public class Day16
{
    // For testing
    long supposedAnswer1 = 1651;
    long supposedAnswer2 = 1707;

    public void Run(string inputPath, bool isTest)
    {
        var sol1 = Solve(true, 30, inputPath);
        WriteAnswer(1, sol1, supposedAnswer1, isTest);

        var sol2 = Solve(false, 26, inputPath);
        WriteAnswer(2, sol2, supposedAnswer2, isTest);
    }

    private int Solve(bool singlePlayer, int time, string inputPath)
    {
        var map = Parse(inputPath);

        var valveMap = new Dictionary<string, Valve>();
        for (int i = 0; i < map.valves.Length; i++)
        {
            valveMap[map.valves[i].name] = map.valves[i];
        }

        var start = valveMap["AA"];

        var valvesToOpen = new BitArray(map.valves.Length);
        for (int i = 0; i < map.valves.Length; i++)
        {
            if (map.valves[i].flowRate > 0)
            {
                valvesToOpen[i] = true;
            }
        }

        const int MAX_FLOW = 1000;
        if (singlePlayer)
        {
            return MaxFlow(map, 0, 0, new Player(start, 0), new Player(start, MAX_FLOW), valvesToOpen, time);
        }
        else
        {
            return MaxFlow(map, 0, 0, new Player(start, 0), new Player(start, 0), valvesToOpen, time);
        }
    }

    record Map(int[,] distances, Valve[] valves);
    record Valve(int id, string name, int flowRate, string[] tunnels);
    record Player(Valve valve, int distance);

    int MaxFlow(Map map, int maxFlow, int currentFlow, Player player0, Player player1, BitArray valvesToOpen, int remainingTime)
    {
        if (player0.distance != 0 && player1.distance != 0)
        {
            throw new ArgumentException("Both players cannot have a non-zero distance.");
        }

        // Next states for both players
        var nextStatesByPlayer = new Player[2][];

        // Iterate over both players
        for (var iplayer = 0; iplayer < 2; iplayer++)
        {
            var player = iplayer == 0 ? player0 : player1;

            // If the player has a distance, decrement it
            if (player.distance > 0)
            {
                nextStatesByPlayer[iplayer] = new[] { player with { distance = player.distance - 1 } };
            }
            // If the player is at a valve and the valve is open, increase the current flow
            // and mark the valve as closed
            else if (valvesToOpen[player.valve.id])
            {
                currentFlow += player.valve.flowRate * (remainingTime - 1);

                if (currentFlow > maxFlow)
                {
                    maxFlow = currentFlow;
                }

                valvesToOpen = new BitArray(valvesToOpen);
                valvesToOpen[player.valve.id] = false;

                nextStatesByPlayer[iplayer] = new[] { player };
            }
            // If the player is at a valve and the valve is closed, generate next states
            // by finding all open valves and computing the distance to them
            else
            {
                var nextStates = new List<Player>();

                for (var i = 0; i < valvesToOpen.Length; i++)
                {
                    if (valvesToOpen[i])
                    {
                        var nextValve = map.valves[i];
                        var distance = map.distances[player.valve.id, nextValve.id];
                        nextStates.Add(new Player(nextValve, distance - 1));
                    }
                }

                nextStatesByPlayer[iplayer] = nextStates.ToArray();
            }
        }

        remainingTime--;
        if (remainingTime < 1)
        {
            return maxFlow;
        }

        // If the current flow plus the residue is less than or equal to the max flow, return the max flow
        if (currentFlow + Residue(valvesToOpen, map, remainingTime) <= maxFlow)
        {
            return maxFlow;
        }

        // Iterate over all next states for both players
        for (var i0 = 0; i0 < nextStatesByPlayer[0].Length; i0++)
        {
            for (var i1 = 0; i1 < nextStatesByPlayer[1].Length; i1++)
            {
                player0 = nextStatesByPlayer[0][i0];
                player1 = nextStatesByPlayer[1][i1];

                // If both players are at the same valve, skip this iteration
                if ((nextStatesByPlayer[0].Length > 1 || nextStatesByPlayer[1].Length > 1) && player0.valve == player1.valve)
                {
                    continue;
                }

                // Calculate the time advancement
                var advance = 0;
                if (player0.distance > 0 && player1.distance > 0)
                {
                    advance = Math.Min(player0.distance, player1.distance);
                    player0 = player0 with { distance = player0.distance - advance };
                    player1 = player1 with { distance = player1.distance - advance };
                }

                // Recursively call MaxFlow with updated values
                maxFlow = MaxFlow(
                    map,
                    maxFlow,
                    currentFlow,
                    player0,
                    player1,
                    valvesToOpen,
                    remainingTime - advance
                );
            }
        }

        return maxFlow;
    }
  
    private int Residue(BitArray valvesToOpen, Map map, int remainingTime)
    {
        int flow = 0;
        for (int i = 0; i < valvesToOpen.Length; i++)
        {
            if (valvesToOpen[i])
            {
                int flowRate = map.valves[i].flowRate;
                flow += flowRate * (remainingTime - 1);
                if ((i & 1) == 0)
                {
                    remainingTime--;
                }
            }
            if (remainingTime <= 0)
            {
                break;
            }
        }
        return flow;
    }

    private Map Parse(string inputPath)
    {
        string input = File.ReadAllText(inputPath);
        Valve[] valves = ParseValves(input);
        return new Map(ComputeDistances(valves), valves);
    }

    private Valve[] ParseValves(string input)
    {
        List<Valve> valveList = new List<Valve>();
        foreach (string line in input.Split("\n"))
        {
            string nLine = line.Trim();
            string name = nLine.Split(" ")[1];
            int flow = int.Parse(Regex.Match(nLine, @"\d+").Groups[0].Value);
            string[] tunnels = Regex.Match(nLine, "to valves? (.*)").Groups[1].Value.Split(", ");
            valveList.Add(new Valve(0, name, flow, tunnels));
        }
        return valveList
            .OrderByDescending(valve => valve.flowRate)
            .Select((v, i) => v with { id = i })
            .ToArray();
    }


    private int[,] ComputeDistances(Valve[] valves)
    {
        int[,] distances = InitializeDistances(valves);

        var valveMap = new Dictionary<string, Valve>();
        for (int i = 0; i < valves.Length; i++)
        {
            valveMap[valves[i].name] = valves[i];
        }

        foreach (Valve valve in valves)
        {
            foreach (string target in valve.tunnels)
            {
                Valve targetNode = valveMap[target];
                distances[valve.id, targetNode.id] = 1;
                distances[targetNode.id, valve.id] = 1;
            }
        }

        bool done = false;
        while (!done)
        {
            done = true;
            for (int source = 0; source < distances.GetLength(0); source++)
            {
                for (int target = 0; target < distances.GetLength(0); target++)
                {
                    if (source != target)
                    {
                        for (int through = 0; through < distances.GetLength(0); through++)
                        {
                            if (distances[source, through] == int.MaxValue || distances[through, target] == int.MaxValue)
                            {
                                continue;
                            }

                            int cost = distances[source, through] + distances[through, target];
                            if (cost < distances[source, target])
                            {
                                done = false;
                                distances[source, target] = cost;
                                distances[target, source] = cost;
                            }
                        }
                    }
                }
            }
        }
        return distances;
    }

    private int[,] InitializeDistances(Valve[] valves)
    {
        int[,] distances = new int[valves.Length, valves.Length];
        for (int i = 0; i < valves.Length; i++)
        {
            for (int j = 0; j < valves.Length; j++)
            {
                distances[i, j] = int.MaxValue;
            }
        }
        return distances;
    }

    private void WriteAnswer<T>(int number, T val, T supposedval, bool isTest)
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
