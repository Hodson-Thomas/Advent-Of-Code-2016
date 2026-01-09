using System.Text;

namespace Aoc16;

public class Day18
{
    public static int Part1(string filePath) => Solve(40, File.ReadAllText(filePath));

    public static int Part2(string filePath) => Solve(400000, File.ReadAllText(filePath));

    private static int Solve(int rowCount, string input)
    {
        var rows = new List<string> { input };
        var row = input;

        for (var i = 0; i < rowCount - 1; i++)
        {
            row = _getNextRow(row);
            rows.Add(row);
        }

        return rows.Sum(x => x.Count(c => c == '.'));
    }

    private static string _getNextRow(string input)
    {
        var builder = new StringBuilder();

        for (var i = 0; i < input.Length; i++)
        {
            var left = i - 1 >= 0 ? input[i - 1] : '.';
            var center = input[i];
            var right = i + 1 < input.Length ? input[i + 1] : '.';

            if ((left, center, right) is ('^', '^', '.') or ('.', '^', '^') or ('^', '.', '.') or ('.', '.', '^'))
            {
                builder.Append('^');
                continue;
            }

            builder.Append('.');
        }

        return builder.ToString();
    }
}