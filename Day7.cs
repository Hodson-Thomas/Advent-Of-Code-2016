namespace Aoc16;

public class Day7
{
    public static int Part1(string filePath)
    {
        using StreamReader reader = new(filePath);
        var count = 0;
        while (reader.ReadLine() is { } line)
            if (_supportsTLS(line))
                count++;
        return count;
    }
    
    public static int Part2(string filePath)
    {
        using StreamReader reader = new(filePath);
        var count = 0;
        while (reader.ReadLine() is { } line)
            if (_supportsSSL(line))
                count++;
        return count;
    }
    
    private static bool _supportsTLS(string ipAddress)
    {
        var valid = false;
        
        for (var i = 0; i < ipAddress.Length - 3; i++)
        {
            if (ipAddress[i] == '[')
            {
                while (ipAddress[i] != ']' && i < ipAddress.Length - 3)
                {
                    if (ipAddress[i] == ipAddress[i + 3] && ipAddress[i + 1] == ipAddress[i + 2] &&
                        ipAddress[i] != ipAddress[i + 1]) return false;
                    i++;
                }
                continue;
            }

            if (ipAddress[i] == ipAddress[i + 3] && ipAddress[i + 1] == ipAddress[i + 2] &&
                ipAddress[i] != ipAddress[i + 1]) valid = true;
        }

        return valid;
    }

    private static bool _supportsSSL(string ipAddress)
    {
        List<string> aba = [];
        List<string> bab = [];
        
        for (var i = 0; i < ipAddress.Length - 2; i++)
        {
            if (ipAddress[i] == '[')
            {
                while (ipAddress[i] != ']' && i < ipAddress.Length - 2)
                {
                    if (ipAddress[i] == ipAddress[i + 2] && ipAddress[i] != ipAddress[i + 1]) 
                        bab.Add("" + ipAddress[i] + ipAddress[i + 1] + ipAddress[i + 2]);
                    i++;
                }
                continue;
            }

            if (ipAddress[i] == ipAddress[i + 2] && ipAddress[i] != ipAddress[i + 1]) 
                aba.Add("" + ipAddress[i] + ipAddress[i + 1] + ipAddress[i + 2]);
        }

        return aba.Any(sequence => bab.Any(seq => seq[0] == sequence[1] && seq[1] == sequence[0]));
    }
}