namespace Aoc16;

public class Day16
{
    public static string Part1() => _solve("10111011111001111", 272);
    
    public static string Part2() => _solve("10111011111001111", 35651584);
    
    private static string _solve(string initialState, int discSize)
    {
        var text = initialState;
        while (text.Length < discSize)
        {
            text = _dragonWave(text);
        }

        var checkSum = _checkSum(text.Substring(0, discSize));
        while (checkSum.Length % 2 == 0)
            checkSum = _checkSum(checkSum);

        return checkSum;
    }
    
    private static string _dragonWave(string text)
    {
        var copy = "";
        for (var i = text.Length - 1; i >= 0; i--)
            copy += text[i] == '1' ? "0" : "1";
        return text + "0" + copy;
    }
    
    private static string _checkSum(string text)
    {
        if (text.Length % 2 != 0) throw new ArgumentException("Text must have even size");
        var result = "";
        for (var i = 0; i < text.Length; i += 2)
            result += text[i] == text[i + 1] ? "1" : "0";
        return result;
    }
}