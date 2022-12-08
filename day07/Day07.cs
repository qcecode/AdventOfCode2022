using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

Day07 day07 = new Day07();
day07.Solve();

public class Directory
{
    public string DirName { get; set; }
    public List<Directory> SubDir { get; set; }
    public Dictionary<string, int> Files { get; set; }
    public Directory ParentDir { get; set; }
    public int DirSize { get; set; }

    public Directory(string dirName, Directory parent)
    {
        DirName = dirName;
        ParentDir = parent;
        SubDir = new List<Directory>();
        Files = new Dictionary<string, int>();
        DirSize = 0;
    }
}

public class Day07
{
    Directory FileSystem = new Directory("/", null);
    List<int> _dirSizes = new List<int>();
    int _part1 = 0;

    public void Solve()
    {
        IEnumerable<String> inputFile = File.ReadLines("C:\\Users\\User\\source\\repos\\AdventOfCode2022\\day07\\input07.txt");
        List<string> input = new List<string>(inputFile);

        Directory currentDir = null;

        for (int i = 0; i < input.Count; i++)
        {
            if (input[i].Contains("$ cd"))
            {
                string dirToChangeTo = input[i].Substring(5);
                switch (dirToChangeTo)
                {
                    case "/":
                        currentDir = FileSystem;
                        break;
                    case "..":
                        currentDir = currentDir.ParentDir;
                        break;
                    default:
                        currentDir = currentDir.SubDir.Find(x => x.DirName == dirToChangeTo);
                        break;
                }
            }
            else if (input[i] == "$ ls")
            {
                int posIdx = i + 1;
                if ((posIdx <= input.Count - 1))
                {
                    while (!input[posIdx].Contains("$"))
                    {
                        if (input[posIdx].Contains("dir "))
                        {
                            string dirName = input[posIdx].Substring(4);
                            if (!currentDir.SubDir.Contains(currentDir.SubDir.Find(x => x.DirName == dirName)))
                            {
                                currentDir.SubDir.Add(new Directory(dirName, currentDir));
                            }
                        }
                        else
                        {
                            string[] file = input[posIdx].Split();
                            currentDir.Files.Add(file[1], int.Parse(file[0]));
                        }
                        posIdx++;
                        if (posIdx == input.Count)
                        {
                            break;
                        }
                    }
                }
                i = posIdx - 1;
            }
        }
        CheckDir(FileSystem);
        Console.WriteLine("Part1: " + _part1);

        int totalDiskSpace = 70000000;
        int updateSize = 30000000;
        int availableDiskSpace = totalDiskSpace - FileSystem.DirSize;
        int neededSpace = updateSize - availableDiskSpace;

        int smallestDirToDelete = int.MaxValue;

        foreach (int i in _dirSizes)
        {
            if (i >= neededSpace && i <= smallestDirToDelete)
            {
                smallestDirToDelete = i;
            }
        }
        Console.WriteLine("Part2: " + smallestDirToDelete);
    }

    public int CheckDir(Directory dir)
    {
        if (dir.Files.Count > 0)
        {
            foreach (KeyValuePair<string, int> kvp in dir.Files)
            {
                dir.DirSize += kvp.Value;
            }
        }

        if (dir.SubDir.Count > 0)
        {
            foreach (Directory subD in dir.SubDir)
            {
                dir.DirSize += CheckDir(subD);
            }
        }

        if (dir.DirSize <= 100000)
        {
            _part1 += dir.DirSize;
        }

        _dirSizes.Add(dir.DirSize);

        return dir.DirSize;
    }
}


