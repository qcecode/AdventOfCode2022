using System.Text.RegularExpressions;

Console.WriteLine("Part 1: " + Part1());
Console.WriteLine("Part 2: " + Part2());

int Part1()
{
    var input = File.ReadAllText("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day22\\input22.txt");
    var parts = input.Split($"{Environment.NewLine}{Environment.NewLine}").ToArray();
    var matches = Regex.Matches(parts.Last(), @"(\d+)([RL]?)");
    var instructions = matches.Select(x => (Steps: int.Parse(x.Groups[1].Value), Rotate: x.Groups[2].Value));

    var lines = parts
        .First()
        .Split(Environment.NewLine)
        .ToArray();
    var position = (X: int.MinValue, Y: int.MinValue);
    var directions = new (int X, int Y)[]
    {
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1),
    };
    var directionIndex = 0;
    var map = new Dictionary<(int X, int Y), bool>();
    var minX = new Dictionary<int, int>();
    var maxX = new Dictionary<int, int>();
    var minY = new Dictionary<int, int>();
    var maxY = new Dictionary<int, int>();

    for (var y = 1; y <= lines.Length; y++)
    {
        var line = lines[y - 1];
        for (var x = 1; x <= line.Length; x++)
        {
            var character = line[x - 1];
            if (char.IsWhiteSpace(character))
            {
                continue;
            }
            if (!minX.ContainsKey(y))
            {
                minX[y] = x;
            }
            maxX[y] = x;
            if (!minY.ContainsKey(x))
            {
                minY[x] = y;
            }
            maxY[x] = y;

            var point = (X: x, Y: y);
            if (character == '#')
            {
                map[point] = false;
            }
            else if (character == '.')
            {
                map[point] = true;
                if (position == (int.MinValue, int.MinValue))
                {
                    position = (point);
                }
            }
        }
    }

    foreach (var instruction in instructions)
    {
        var (steps, rotate) = instruction;

        for (var i = 0; i < steps; i++)
        {
            var direction = directions[directionIndex];
            var next = (position.X + direction.X, position.Y + direction.Y);
            if (!map.TryGetValue(next, out var valid))
            {
                next = direction switch
                {
                    (1, 0) => (minX[position.Y], position.Y),
                    (0, 1) => (position.X, minY[position.X]),
                    (-1, 0) => (maxX[position.Y], position.Y),
                    (0, -1) => (position.X, maxY[position.X]),
                    _ => throw new Exception()
                };
                valid = map[next];
            }

            if (!valid)
            {
                break;
            }

            position = next;
        }

        if (rotate == "R")
        {
            directionIndex += 1;
            directionIndex %= 4;
        }
        else if (rotate == "L")
        {
            directionIndex += 3;
            directionIndex %= 4;
        }
    }

    return 1000 * position.Y + 4 * position.X + directionIndex;
}

int Part2(int length = 50)
{
    var input = File.ReadAllText("C:\\Users\\henri\\source\\repos\\qcecode\\AdventOfCode2022\\day22\\input22.txt");
    var parts = input.Split($"{Environment.NewLine}{Environment.NewLine}").ToArray();
    var matches = Regex.Matches(parts.Last(), @"(\d+)([RL]?)");
    var instructions = matches.Select(x => (Steps: int.Parse(x.Groups[1].Value), Rotate: x.Groups[2].Value));

    const string front = nameof(front);
    const string back = nameof(back);
    const string left = nameof(left);
    const string right = nameof(right);
    const string top = nameof(top);
    const string bottom = nameof(bottom);

    var lines = parts
        .First()
        .Split(Environment.NewLine)
        .ToArray();

    var faceNeighbours = new Dictionary<string, string[]>
    {
        {front, new [] { right, bottom, left, top}},
        {back, new [] { left, bottom, right, top}},
        {left, new [] { front, bottom, back, top}},
        {right, new [] { back, bottom, front, top}},
        {top, new [] { right, front, left, back}},
        {bottom, new [] { right, back, left, front}}
    };
    var faceOffset = new Dictionary<string, int>();
    var faceSegment = new Dictionary<string, (int X, int Y)>();
    var segments = new Dictionary<(int X, int Y), Dictionary<(int X, int Y), bool>>();

    var face = front;
    var position = (X: int.MinValue, Y: int.MinValue);
    var directions = new (int X, int Y)[]
    {
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1),
    };
    var directionIndex = 0;

    for (var j = 0; j < lines.Length / length; j++)
    {
        var jFactor = j * length;
        for (var i = 0; i < lines[jFactor].Length / length; i++)
        {
            var iFactor = i * length;
            var segment = (i, j);
            segments[segment] = new Dictionary<(int X, int Y), bool>();
            for (var y = 0; y < length; y++)
            {
                var line = lines[jFactor + y];
                for (var x = 0; x < length; x++)
                {
                    var character = line[iFactor + x];
                    if (char.IsWhiteSpace(character))
                    {
                        continue;
                    }
                    var point = (X: x, Y: y);
                    if (character == '#')
                    {
                        segments[segment][point] = false;
                    }
                    else if (character == '.')
                    {
                        segments[segment][point] = true;
                        if (position == (int.MinValue, int.MinValue))
                        {
                            position = (point);
                        }
                    }
                }
            }
        }
    }

    segments = segments.Where(s => s.Value.Any()).ToDictionary(s => s.Key, s => s.Value);

    var queue = new Queue<((int X, int Y) Segment, string Face, int FromDirection, string FromFace)>();
    var visited = new HashSet<(int X, int Y)>();
    visited.Add(segments.Keys.First());
    queue.Enqueue((visited.First(), front, 1, top));

    while (queue.Any())
    {
        var current = queue.Dequeue();
        faceSegment[current.Face] = current.Segment;
        var relativeFrom = current.FromDirection + 2 % 4;
        var offset = (4 + relativeFrom - Array.IndexOf(faceNeighbours[current.Face], current.FromFace)) % 4;
        faceOffset[current.Face] = offset;

        for (var i = 0; i < 4; i++)
        {
            var direction = directions[i];
            var segment = (current.Segment.X + direction.X, current.Segment.Y + direction.Y);
            if (segments.ContainsKey(segment) && !visited.Contains(segment))
            {
                visited.Add(segment);
                queue.Enqueue((segment, faceNeighbours[current.Face][(4 + i - offset) % 4], i, current.Face));
            }
        }
    }

    foreach (var instruction in instructions)
    {
        var (steps, rotate) = instruction;

        for (var i = 0; i < steps; i++)
        {
            var direction = directions[directionIndex];
            var newPosition = (X: position.X + direction.X, Y: position.Y + direction.Y);
            var newDirectionIndex = directionIndex;
            var newFace = face;
            if (!segments[faceSegment[face]].TryGetValue(newPosition, out var valid))
            {
                newFace = faceNeighbours[face][(4 + directionIndex - faceOffset[face]) % 4];
                newPosition = position;
                var relativeFrom = (directionIndex + 2) % 4;
                var positionOffset = (4 + Array.IndexOf(faceNeighbours[newFace], face) - relativeFrom) % 4;
                var offset = faceOffset[newFace];
                var rotations = (positionOffset + offset) % 4;

                for (var j = 0; j < rotations; j++)
                {
                    newDirectionIndex += 1;
                    newDirectionIndex %= 4;
                    newPosition = (length - 1 - newPosition.Y, newPosition.X);
                }

                newPosition = newDirectionIndex switch
                {
                    0 => (0, newPosition.Y),
                    1 => (newPosition.X, 0),
                    2 => (length - 1, newPosition.Y),
                    3 => (newPosition.X, length - 1),
                    _ => throw new Exception()
                };

                valid = segments[faceSegment[newFace]][newPosition];
            }

            if (!valid)
            {
                break;
            }

            position = newPosition;
            face = newFace;
            directionIndex = newDirectionIndex;
        }

        if (rotate == "R")
        {
            directionIndex += 1;
            directionIndex %= 4;
        }
        else if (rotate == "L")
        {
            directionIndex += 3;
            directionIndex %= 4;
        }
    }

    var (xSegment, ySegment) = faceSegment[face];
    var column = xSegment * length + position.X + 1;
    var row = ySegment * length + position.Y + 1;

    return 1000 * row + 4 * column + directionIndex;
}