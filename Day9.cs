namespace Aoc16;

public class Day9
{
    public static int Part1(string filePath)
    {
        var content = _parseInput(filePath);
        var result = 0;
        
        for (var i = 0; i < content.Length; i++)
        {
            if (content[i] != '(')
            {
                result++;
                continue;
            }

            var buffer = "";

            do
            {
                buffer += content[i];
                i++;
            } while (content[i] != ')');

            i++;

            var split = buffer[1..].Split('x');
            if (split.Length != 2) throw new Exception("Invalid pattern");
            var sequence = int.Parse(split[0]);
            result += int.Parse(split[1]) * sequence;
            i += sequence - 1;
        }

        return result;
    }

    public static long Part2(string filePath) => Decompress(_parseInput(filePath));

    private static long Decompress(string content)
    {
        long size = 0;
        for (var i = 0; i < content.Length; i++)
        {
            if (content[i] != '(')
            {
                size++;
                continue;
            }
            
            var buffer = "";

            do
            {
                buffer += content[i];
                i++;
            } while (content[i] != ')');

            i++;
            
            var split = buffer[1..].Split('x');
            if (split.Length != 2) throw new Exception("Invalid pattern");
            var sequence = int.Parse(split[0]);
            size += long.Parse(split[1]) * Decompress(content.Substring(i, sequence));
            i += sequence - 1;
        }
        return size;
    }

    
    private static string _parseInput(string filePath)
    {
        using StreamReader reader = new(filePath);
        return reader.ReadToEnd().Where(c => !char.IsWhiteSpace(c)).Aggregate("", (current, c) => current + c);
    }
}