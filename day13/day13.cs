using System.Text.Json;
using System.IO;
using System.Linq;
using System;

// Read the input
var input = await File.ReadAllTextAsync("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day13\\input13.txt");

// Split the input into pairs
var pairs = input.Split("\r\n\r\n");
var pairIndex = 0;
var correctPairs = 0;

// Iterate over each pair
foreach (var pair in pairs)
{
    pairIndex++;

    // Split the pair into left and right
    var splitPair = pair.Split("\r\n");
    var left = splitPair[0];
    var right = splitPair[1];

    // Parse the left and right strings as JSON
    var jsonLeft = JsonDocument.Parse(left).RootElement;
    var jsonRight = JsonDocument.Parse(right).RootElement;

    // Compare the left and right JSON
    var isCorrect = Compare(jsonLeft, jsonRight);

    // If the pair is correct, add the pair index to the correct pairs count
    if (isCorrect == true) correctPairs += pairIndex;
}

// Output the correct pairs count
Console.WriteLine($"Part 1: {correctPairs}");

// Parse the input strings as JSON
var allPackets = input.Split("\r\n").Where(l => !string.IsNullOrEmpty(l)).Select(l => JsonDocument.Parse(l).RootElement).ToList();

// Parse the x and y strings as JSON
var x = JsonDocument.Parse("[[2]]").RootElement;
var y = JsonDocument.Parse("[[6]]").RootElement;

// Add x and y to the list of packets
allPackets.Add(x);
allPackets.Add(y);

// Sort the packets using the Compare method
allPackets.Sort((left, right) => Compare(left, right) == true ? -1 : 1);

// Output the result
Console.WriteLine($"Part 2: {(allPackets.IndexOf(x) + 1) * (allPackets.IndexOf(y) + 1)}");

// Compare method
bool? Compare(JsonElement left, JsonElement right)
{
    // If both elements are numbers, compare them
    if (left.ValueKind == JsonValueKind.Number && right.ValueKind == JsonValueKind.Number)
    {
        return left.GetInt32() == right.GetInt32() ? null : left.GetInt32() < right.GetInt32();
    }

    // If either element is an array, compare the elements in the arrays
    if (left.ValueKind == JsonValueKind.Array || right.ValueKind == JsonValueKind.Array)
    {
        var leftArray = left.ValueKind == JsonValueKind.Array ? left.EnumerateArray().ToList() : new List<JsonElement> { left };
        var rightArray = right.ValueKind == JsonValueKind.Array ? right.EnumerateArray().ToList() : new List<JsonElement> { right };

        for (var i = 0; i < Math.Min(leftArray.Count, rightArray.Count); i++)
        {
            var res = Compare(leftArray[i], rightArray[i]);
            if (res.HasValue) { return res.Value; }
        }

        return leftArray.Count < rightArray.Count;
    }

    // If neither element is a number or an array, return false
    return false;
}

