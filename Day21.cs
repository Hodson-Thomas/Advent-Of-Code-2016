using System.Text;

namespace Aoc16;

public class Day21
{
    public static string Part1(string filePath)
        => Scramble("abcdefgh", File.ReadAllLines(filePath));
    
    public static string Part2(string filePath)
        => Unscramble("fbgdceah", File.ReadAllLines(filePath));

    private static string Scramble(string password, string[] instructions)
    {
        var builder = new StringBuilder(password);

        foreach (var line in instructions)
        {
            var values = line.Split(' ');

            switch (values[0])
            {
                case "swap" when values[1] == "position":
                    SwapByPosition(values, builder);
                    break;
                case "swap" when values[1] == "letter":
                    SwapByLetter(values, builder);
                    break;
                case "rotate" when values[1] == "left":
                    RotateLeft(values, builder);
                    break;
                case "rotate" when values[1] == "right":
                    RotateRight(values, builder);
                    break;
                case "rotate" when values[1] == "based":
                    RotateByLetter(builder, values);
                    break;
                case "reverse":
                    Reverse(values, builder);
                    break;
                case "move":
                    Move(values, builder);
                    break;
            }
        }

        return builder.ToString();
    }

    private static string Unscramble(string password, string[] instructions)
    {
        var permutations = _getPermutations(password, password.Length).Select(x => new string(x.ToArray()));
        foreach (var permutation in permutations)
            if (password == Scramble(permutation, instructions))
                return permutation;
        return "";
    }

    private static void SwapByPosition(string[] values, StringBuilder builder)
    {
        var (x, y) = (int.Parse(values[2]), int.Parse(values[5]));
        (builder[x], builder[y]) = (builder[y], builder[x]);
    }

    private static void SwapByLetter(string[] values, StringBuilder builder)
    {
        var (x, y) = (values[2], values[5]);
        builder.Replace(x, "_");
        builder.Replace(y, x);
        builder.Replace("_", y);
    }

    private static void RotateLeft(string[] values, StringBuilder builder)
    {
        var x = int.Parse(values[2]);
        var buffer = new char[builder.Length];

        for (var i = 0; i < builder.Length; i++)
        {
            var index = (i + x) % builder.Length;
            buffer[i] = builder[index];
        }

        builder.Clear();
        builder.Append(buffer);
    }

    private static void RotateRight(string[] values, StringBuilder builder)
    {
        var x = int.Parse(values[2]);
        var buffer = new char[builder.Length];

        for (var i = 0; i < builder.Length; i++)
        {
            var index = (i - x) % builder.Length;
            if (index < 0)
                index += builder.Length;
            buffer[i] = builder[index];
        }

        builder.Clear();
        builder.Append(buffer);
    }

    private static void RotateByLetter(StringBuilder builder, string[] values)
    {
        var letter = values[6];
        var indexOfLetter = builder.ToString().IndexOf(letter, StringComparison.Ordinal);
        var x = 1 + indexOfLetter + (indexOfLetter >= 4 ? 1 : 0);
        var buffer = new char[builder.Length];

        for (var i = 0; i < builder.Length; i++)
        {
            var index = (i - x) % builder.Length;
            if (index < 0)
                index += builder.Length;
            buffer[i] = builder[index];
        }

        builder.Clear();
        builder.Append(buffer);
    }

    private static void Reverse(string[] values, StringBuilder builder)
    {
        var (x, y) = (int.Parse(values[2]), int.Parse(values[4]));
        while (y > x)
        {
            (builder[x], builder[y]) = (builder[y], builder[x]);
            x++;
            y--;
        }
    }

    private static void Move(string[] values, StringBuilder builder)
    {
        var (x, y) = (int.Parse(values[2]), int.Parse(values[5]));
        var c = builder[x];
        builder.Remove(x, 1);
        builder.Insert(y, c);
    }
    
    private static IEnumerable<IEnumerable<T>> _getPermutations<T>(IEnumerable<T> enumerable, int length) => length == 1 
        ? enumerable.Select(x => new List<T> { x }) 
        :  _getPermutations(enumerable, length - 1)
            .SelectMany(x => enumerable.Where(y => !x.Contains(y)), (a, b) => a.Concat(new[] { b }));
}
