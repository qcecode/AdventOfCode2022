using System.Text.RegularExpressions;


Day15.Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day15\\input15.txt");
Day15.Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day15\\example15.txt");
public class Day15
{
    public static void Run(string filePath)
    {
        HashSet<(int x, int y)> sensors = new();
        HashSet<(int x, int y)> beacons = new();
        List<(int cx, int cy, int r)> circles = new();

        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            var nline = line.Replace("Sensor at x=", "").Replace(" y=", "").Replace(": closest beacon is at x=", ",").Trim();
            string[] values = nline.Split(',');
            int sx = int.Parse(values[0]);
            int sy = int.Parse(values[1]);
            sensors.Add((sx, sy));
            int bx = int.Parse(values[2]);
            int by = int.Parse(values[3]);
            beacons.Add((bx, by));
            int r = Distance((bx, by), (sx, sy));
            circles.Add((sx, sy, r));
        }

        int min = 0;
        int max = 4000000;
        HashSet<(int x, int y)> toCheck = new();

        for (int i = 0; i < circles.Count; i++)
        {
            var circle = circles[i];

            for (int j = 0; j < circles.Count; j++)
            {
                var circle2 = circles[j];
                if (i == j)
                {
                    continue;
                }

                if (Distance((circle.cx, circle.cy), (circle2.cx, circle2.cy)) == circle.r + circle2.r + 2)
                {
                    int yEnd = Math.Min(circle.cy + circle.r, circle2.cy + circle2.r);
                    int yStart = Math.Max(circle.cy - circle.r, circle2.cy - circle2.r);

                    int xStart = Math.Max(circle.cx - circle.r, circle2.cx - circle2.r);
                    int xEnd = Math.Min(circle.cx + circle.r, circle2.cx + circle2.r);

                    for (int y = yStart; y < yEnd; y++)
                    {
                        int x1 = circle.cx + (circle.r + 1 - Math.Abs(y - circle.cy));
                        int x2 = circle.cx - (circle.r + 1 - Math.Abs(y - circle.cy));

                        if (x1 >= min && x1 <= max &&
                            x1 >= xStart && x1 <= xEnd)
                        {
                            toCheck.Add((x1, y));
                        }
                        if (x2 >= min && x2 <= max &&
                            x2 >= xStart && x2 <= xEnd)
                        {
                            toCheck.Add((x2, y));
                        }
                    }
                }
            }
        }

        foreach (var point in toCheck)
        {
            if (sensors.Contains(point) || beacons.Contains(point))
            {
                continue;
            }
            bool found = true;
            foreach (var circle in circles)
            {
                if (IsPointInCircle(point, circle))
                {
                    found = false;
                    break;
                }
            }
            if (found)
            {

                Console.WriteLine($"{(long)point.x * 4000000L + (long)point.y}");
                return;
            }
        }
    }
        private static int Distance((int x, int y) p1, (int x, int y) p2)
    {
        return Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
    }

    private static bool IsPointInCircle((int x, int y) point, (int cx, int cy, int r) circle)
    {
        if (Distance(point, (circle.cx, circle.cy)) <= circle.r)
        {
            return true;
        }
        return false;
    }
}