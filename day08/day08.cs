using System.Data;
using System.Diagnostics;
using System.Dynamic;

Day08 day08 = new Day08();
day08.Part01and02();

public class Day08
{
    public void Part01and02()
    {
        var input = File.ReadAllLines("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day08\\input08.txt");

        int visibleTrees = 0;
        int highestScenicScore = 0;
        for (int row = 0; row < input.Count(); row++)
        {
            string treeLine = input.ElementAt(row);
            for (int column = 0; column < treeLine.Count(); column++)
            {
                Tuple<bool, int> tuple = GetVisibilityAndScenicScore(input, row, column);
                bool isVisible = tuple.Item1;
                if (isVisible)
                {
                    visibleTrees++;
                }
                highestScenicScore = Math.Max(highestScenicScore, tuple.Item2);
            }
        }
        Console.WriteLine($"Visible trees: {visibleTrees} | Highest scenic score: {highestScenicScore}");
    }

    internal Tuple<bool, int> GetVisibilityAndScenicScore(string[] treePatch, int x, int y)
    {
        int patchWidth = treePatch.ElementAt(0).Count();
        int patchLength = treePatch.Count();

        bool isVisible = false;
        bool isLineClear = true;
        int score = 1;
        int clearLineLength = 0;

        //corners
        if (x == 0 || y == 0 || (x == patchLength - 1) || (y == patchWidth - 1))
        {
            isVisible = true;
            return new Tuple<bool, int>(isVisible, score);
        }

        //upwards
        for (int row = x - 1; row >= 0; row--)
        {
            clearLineLength++;
            if (treePatch.ElementAt(row).ElementAt(y) >= treePatch.ElementAt(x).ElementAt(y))
            {
                isLineClear = false;
                break;
            }
        }

        isVisible = isVisible || isLineClear;
        isLineClear = true;
        score *= clearLineLength;
        clearLineLength = 0;

        //downwards
        for (int row = x + 1; row < patchLength; row++)
        {
            clearLineLength++;
            if (treePatch.ElementAt(row).ElementAt(y) >= treePatch.ElementAt(x).ElementAt(y))
            {
                isLineClear = false;
                break;
            }
        }

        isVisible = isVisible || isLineClear;
        isLineClear = true;
        score *= clearLineLength;
        clearLineLength = 0;

        //right
        for (int column = y + 1; column < patchWidth; column++)
        {
            clearLineLength++;
            if (treePatch.ElementAt(x).ElementAt(column) >= treePatch.ElementAt(x).ElementAt(y))
            {
                isLineClear = false;
                break;
            }
        }

        isVisible = isVisible || isLineClear;
        isLineClear = true;
        score *= clearLineLength;
        clearLineLength = 0;

        //left
        for (int column = y - 1; column >= 0; column--)
        {
            clearLineLength++;
            if (treePatch.ElementAt(x).ElementAt(column) >= treePatch.ElementAt(x).ElementAt(y))
            {
                isLineClear = false;
                break;
            }
        }

        isVisible = isVisible || isLineClear;
        score *= clearLineLength;

        return new Tuple<bool, int>(isVisible, score);
    }
}

