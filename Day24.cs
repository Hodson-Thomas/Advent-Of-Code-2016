namespace Aoc16;

public class Day24
{
    public static int Part1(string filePath) => new Maze(filePath).GetShortestPath();
}

class Maze
{
    private readonly List<List<char>> _maze;
    private readonly string _poi;
    private readonly (int, int) _start;

    public Maze(string filePath)
    {
        using var reader = new StreamReader(filePath);
        List<List<char>> maze = [];
        var poi = "";
        var start = (0, 0);
        var y = 0;
        while (reader.ReadLine() is { } line)
        {
            var l = line.Trim();
            if (l.Length == 0) continue;
            maze.Add(l.ToCharArray().ToList());
            var x = 0;
            foreach (var c in l)
            {
                if (!char.IsDigit(c))
                {
                    x++;
                    continue;
                }

                if (c == '0') start = (x, y);
                poi += c;
                x++;
            }

            y++;
        }

        _poi = string.Concat(poi.OrderBy(c => c));
        _maze = maze;
        _start = start;
    }

    public int GetShortestPath() => _getShortestPath(_start, 0, int.MaxValue, "");
    
    private int _getShortestPath((int, int) position, int path, int min, string poi)
    {
        if (poi == _poi) return Math.Min(min, path);
        if (path > min) return min;
        var (x, y) = position;
        if (y < 0 || y >= _maze.Count || x < 0 || x >= _maze[y].Count || _maze[y][x] == '#') return min;
        var p = poi;
        if (char.IsDigit(_maze[y][x]) && !poi.Contains(_maze[y][x]))
            p = string.Concat((poi + _maze[y][x]).OrderBy(c => c));

        var m = min;
        m = Math.Min(m, _getShortestPath((x - 1, y), path + 1, m, p));
        m = Math.Min(m, _getShortestPath((x + 1, y), path + 1, m, p));
        m = Math.Min(m, _getShortestPath((x, y - 1), path + 1, m, p));
        return Math.Min(m, _getShortestPath((x, y + 1), path + 1, m, p));
    }
    
}
