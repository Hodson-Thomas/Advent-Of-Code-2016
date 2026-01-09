namespace Aoc16;

public class Day20
{
    public static long Part1(string filePath)
    {
        var ranges = _parseInput(filePath);
        long i = 0;
        while (ranges.Any(range => range.Item1 <= i && i <= range.Item2)) i++;
        return i;
    }

    public static long Part2(string filePath) => _joinRanges(_parseInput(filePath))
        .Aggregate<(long, long), long>(4294967295, (current, range) => current - (range.Item2 - range.Item1 + 1)) + 1;
    
    private static List<(long, long)> _parseInput(string filePath)
    {
        List<(long, long)> values = [];
        using var reader = new StreamReader(filePath);
        while (reader.ReadLine() is { } line)
        {
            var content = line.Trim().Split('-');
            if (content.Length != 2) continue;
            if (!long.TryParse(content[0], out var start) || !long.TryParse(content[1], out var end))
                continue;
            values.Add((Math.Min(start, end), Math.Max(start, end)));
        }

        return values;
    }

    private static List<(long, long)> _joinRanges(List<(long, long)> ranges)
    {
        var joinRanges = ranges;
        var changes = true;
        while (changes)
        {
            changes = false;
            for (var i = 0; i < joinRanges.Count; i++)
            {
                if (changes) break;
                for (var j = 0; j < joinRanges.Count; j++)
                {
                    if (i == j || !_rangesOverlaps(joinRanges[i], joinRanges[j])) continue;
                    var first = Math.Min(i, j);
                    var last = Math.Max(i, j);
                    var combined = _combine(joinRanges[i], joinRanges[j]);
                    joinRanges.RemoveAt(last);
                    joinRanges.RemoveAt(first);
                    joinRanges.Add(combined);
                    changes = true;
                    break;
                }
            }
        }

        return joinRanges;
    }

    private static bool _rangesOverlaps((long, long) range1, (long, long) range2) =>
        (range1.Item1 < range2.Item1 && range1.Item2 >= range2.Item1) ||
        (range2.Item1 < range1.Item1 && range2.Item2 >= range1.Item1);

    private static (long, long) _combine((long, long) range1, (long, long) range2) =>
        (Math.Min(range1.Item1, range2.Item1), Math.Max(range1.Item2, range2.Item2));
}