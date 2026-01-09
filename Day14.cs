using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc16;

public class Day14
{
    private const string Salt = "ihaygndm";
    private static readonly Dictionary<string, string> Memoization = new();
    
    public static int Part1() => Solve(1);
    
    public static int Part2() => Solve(2017);

    private static int Solve(int timesToHash)
    {
        var count = 0;
        for (var index = 0; index < int.MaxValue; index++)
        {
            count += IsValidKey(Salt, index, timesToHash) ? 1 : 0;
            if (count == 64) return index;
        }
        return 0;
    }

    private static bool IsValidKey(string salt, int index, int timesToHash)
    {
        var hash = GetHash(salt + index, timesToHash);
        var match = Regex.Match(hash, @"([a-zA-Z0-9])\1{2}");
        return match.Success && HasSecondaryMatch(salt, index, timesToHash, match.Value[0]);
    }

    private static bool HasSecondaryMatch(string salt, int index, int timesToHash, char character)
    {
        for (var i = 0; i < 1000; i++)
        {
            var hash = GetHash(salt + (index + i + 1), timesToHash);
            var match = Regex.Match(hash, $"({character})\\1{{4}}");

            if (match.Success) return true;
        }

        return false;
    }

    private static string GetHash(string input, int timesToHash)
    {
        if (Memoization.TryGetValue(input, out var result))
            return result;
    
        var hash = input;
        
        for (var i = 0; i < timesToHash; i++)
            hash = _md5(hash);
        
        Memoization[input] = hash;
        return hash;
    }
    
    private static string _md5(string input) =>
        Convert.ToHexString(MD5.HashData(Encoding.ASCII.GetBytes(input))).ToLower();
}