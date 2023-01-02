using System.Text.RegularExpressions;


Day15.Run("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day15\\input15.txt");
public class Day15
{
    private static Regex lineRegEx = new(
        @"^Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)$",
        RegexOptions.Compiled
    );

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

    public static void Run(string filePath)
    {
        HashSet<(int x, int y)> sensors = new();
        HashSet<(int x, int y)> beacons = new();
        List<(int cx, int cy, int r)> circles = new();

        using (StreamReader reader = File.OpenText(filePath))
        {
            string? line;
            //find all the senor and beacon locations and constuct a taxi cab circle around all
            //sensors
            while ((line = reader.ReadLine()) != null)
            {
                var match = lineRegEx.Match(line);
                int sx = int.Parse(match.Groups[1].Value);
                int sy = int.Parse(match.Groups[2].Value);
                sensors.Add((sx, sy));

                int bx = int.Parse(match.Groups[3].Value);
                int by = int.Parse(match.Groups[4].Value);

                beacons.Add((bx, by));

                int r = Distance((bx, by), (sx, sy));

                circles.Add((sx, sy, r));
            }
        }

        int lowerBounds = 0;
        int upperBounds = 4000000;
        HashSet<(int x, int y)> toCheck = new();

        // Find each pair of sensor circles that have exatly one space between them and then add the
        // points between them to the set of points to check
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
                    int endy = Math.Min(circle.cy + circle.r, circle2.cy + circle2.r);
                    int starty = Math.Max(circle.cy - circle.r, circle2.cy - circle2.r);

                    int startx = Math.Max(circle.cx - circle.r, circle2.cx - circle2.r);
                    int endx = Math.Min(circle.cx + circle.r, circle2.cx + circle2.r);

                    for (int y = starty; y < endy; y++)
                    {
                        int x1 = circle.cx + (circle.r + 1 - Math.Abs(y - circle.cy));
                        int x2 = circle.cx - (circle.r + 1 - Math.Abs(y - circle.cy));

                        if (x1 >= lowerBounds && x1 <= upperBounds &&
                            x1 >= startx && x1 <= endx)
                        {
                            toCheck.Add((x1, y));
                        }
                        if (x2 >= lowerBounds && x2 <= upperBounds &&
                            x2 >= startx && x2 <= endx)
                        {
                            toCheck.Add((x2, y));
                        }
                    }
                }
            }


        }

        //// For each pointto check, check if it is inside a sensor range.  If
        //// not, print out it's tuning frequency and exit
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


}