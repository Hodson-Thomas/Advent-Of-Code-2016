namespace Aoc16;

public class Day6
{
    public static string Part1(string filePath)
    {
        var columns = _parseInput(filePath);
        return columns.Aggregate("", (current, column) => current + column
            .OrderByDescending(ch => column.Count(c => c == ch))
            .ThenBy(c => c)
            .First());
    }
    
    public static string Part2(string filePath)
    {
        var columns = _parseInput(filePath);
        return columns.Aggregate("", (current, column) => current + column
            .OrderBy(ch => column.Count(c => c == ch))
            .ThenBy(c => c)
            .First());
    }

    private static List<List<char>> _parseInput(string filePath)
    {
        using StreamReader reader = new(filePath);
        List<List<char>> chars = [];
        while (reader.ReadLine() is { } line)
        {
            line = line.Trim();
            if (line.Length == 0) continue;
            while (chars.Count < line.Length) chars.Add(new());
            for (var i = 0; i < line.Length; i++)
                chars[i].Add(line[i]);
        }

        return chars;
    }
}