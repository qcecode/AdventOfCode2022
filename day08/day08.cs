using System.Data;
using System.Dynamic;



var inputFile = File.ReadLines("C:\\Users\\User\\source\\repos\\AdventOfCode2022\\day08\\input08.txt");
var input = new List<string>(inputFile);

int rows = input.Count, cols = input[0].Length;

int[,] PlantForest(List<String> inputList)
{
    var forest = new int[rows,cols];
    for (int i = 0; i < rows; i++)
    {
        for (int k = 0; k < cols; k++)
        {
            forest[i,k] = int.Parse(inputList[i][k].ToString());
        }
    }
    return forest;
}



PlantForest(input);
Direction Left = new Direction(0, -1);
Direction Right = new Direction(0, 1);
Direction Up = new Direction(-1, 0);
Direction Down = new Direction(1, 0);
record Direction(int dRow, int dCol);
