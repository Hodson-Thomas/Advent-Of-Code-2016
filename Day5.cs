namespace Aoc16;

public class Day5
{
    private const string Input = "uqwqemis";

    public static string Part1()
    {
        var code = "";

        var i = 1;
        while (code.Length != 8)
        {
            var hash = _hash(Input + i);
            if (hash.StartsWith("00000"))
                code += hash[5];
            i++;
        }

        return code;
    }

    public static string Part2()
    {
        char[] code = [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '];
        var i = 1;

        while (code.Any(c => c == ' '))
        {
            var hash = _hash(Input + i);
            if (hash.StartsWith("00000") && _charToInt(hash[5]) is { } index and >= 0 and < 8 && code[index] == ' ')
                code[index] = hash[6];
            i++;
        }

        return string.Join("", code);
    }

    private static int? _charToInt(char c) => c switch
    {
        '0' => 0,
        '1' => 1,
        '2' => 2,
        '3' => 3,
        '4' => 4,
        '5' => 5,
        '6' => 6,
        '7' => 7,
        '8' => 8,
        '9' => 9,
        _ => null
    };

    private static string _hash(string str)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
        var hashBytes = md5.ComputeHash(inputBytes);
        return Convert.ToHexString(hashBytes);
    }
}