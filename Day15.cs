namespace Aoc16;

public class Day15
{
    public static int Part1(string filePath)
    {
        var time = 0;
        var discs = _parseInput(filePath);

        while (discs.Any(d => d.GetPositionAtTime(time) != 0))
            time++;
        
        return time;
    }
    
    public static int Part2(string filePath)
    {
        var time = 0;
        var discs = _parseInput(filePath);
        discs.Add(new Disc(discs.Count + 1, 11, 0));

        while (discs.Any(d => d.GetPositionAtTime(time) != 0))
            time++;
        
        return time;
    }
    
    private static List<Disc> _parseInput(string filePath)
    {
        using var reader = new StreamReader(filePath);
        List<Disc> discs = [];
        while (reader.ReadLine() is { } line)
        {
            var content = line.Trim().Split(' ');
            if (content.Length != 12) continue;
            
            discs.Add(new Disc(int.Parse(content[1][1..]), int.Parse(content[3]), int.Parse(content[11][..^1])));
        }

        return discs;
    }
}

record Disc(int Index, int Positions, int Start)
{
    public int GetPositionAtTime(int time) => (Start + time + Index) % Positions;
}