namespace Aoc16;

public class Day3
{
    public static int Part1(string filePath) =>
        ParseInput1(filePath).Count(t => t.IsTriangle());

    public static int Part2(string filePath) =>
        ParseInput2(filePath).Count(t => t.IsTriangle());

    private static List<Triangle> ParseInput1(string filePath)
    {
        using StreamReader reader = new(filePath);
        List<Triangle> triangles = [];
        while (reader.ReadLine() is { } line)
        {
            line = _cleanLine(line.Trim());
            var split = line.Split(" ");
            if (split.Length != 3) continue;
            triangles.Add(new Triangle(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])));
        }

        return triangles;
    }

    private static List<Triangle> ParseInput2(string filePath)
    {
        using StreamReader reader = new(filePath);
        List<int> col1 = [], col2 = [], col3 = [];
        while (reader.ReadLine() is { } line)
        {
            line = _cleanLine(line.Trim());
            var split = line.Split(" ");
            if (split.Length != 3) continue;
            col1.Add(int.Parse(split[0]));
            col2.Add(int.Parse(split[1]));
            col3.Add(int.Parse(split[2]));
        }

        List<Triangle> triangles = [];
        for (var i = 0; i < col1.Count - 2; i += 3)
            triangles.Add(new Triangle(col1[i], col1[i + 1], col1[i + 2]));
        for (var i = 0; i < col2.Count - 2; i += 3)
            triangles.Add(new Triangle(col2[i], col2[i + 1], col2[i + 2]));
        for (var i = 0; i < col3.Count - 2; i += 3)
            triangles.Add(new Triangle(col3[i], col3[i + 1], col3[i + 2]));
        
        return triangles;
    }
    

    private static string _cleanLine(string line)
    {
        var result = "" + line[0];
        for (var i = 1; i < line.Length; i++)
        {
            if (result.Last() == ' ' && line[i] == ' ')
                continue;
            result += line[i];
        }

        return result;
    }
    
    private record Triangle(int A, int B, int C)
    {
        public bool IsTriangle() => A + B > C && A + C > B && B + C > A; 
    }
}