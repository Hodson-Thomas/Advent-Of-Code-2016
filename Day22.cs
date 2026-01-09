using System.Text;
using System.Text.RegularExpressions;

namespace Aoc16;



public class Day22
{
    public static int Part1(string filePath)
    {
        var data = _parseInput(filePath);
        var keys = data.Keys.ToList();
        var count = 0;

        for (var i = 0; i < keys.Count; i++)
        {
            if (data[keys[i]].Item1 == 0) continue;
            for (var j = 0; j < keys.Count; j++)
            {
                if (i == j) continue;
                if (data[keys[i]].Item1 <= data[keys[j]].Item2) count++;
            }
        }
        
        return count;
    }
    
    private static Dictionary<(int, int), (int, int)> _parseInput(string filePath)
    {
        using var reader = new StreamReader(filePath);
        Dictionary<(int, int), (int, int)> data = new();

        while (reader.ReadLine() is { } line)
        {
            if (!line.StartsWith('/')) continue;
            var split = _cleanLine(line.Trim()).Split(' ');
            var name = split[0].Split('-');
            if (!int.TryParse(name[1][1..], out var x) || !int.TryParse(name[2][1..], out var y) ||
                !int.TryParse(split[2][..^1], out var used) ||
                !int.TryParse(split[3][..^1], out var available)) continue;
            if (data.ContainsKey((x, y))) continue;
            data.Add((x, y), (used, available));
        }

        return data;
    }

    private static string _cleanLine(string line)
    {
        var res = "";
        var previous = '\0';
        foreach (var c in line)
        {
            if (c == ' ' && previous == ' ') continue;
            res += c;
            previous = c;
        }

        return res;
    }
}