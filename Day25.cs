namespace Aoc16;

public class Day25
{
    public static int Part1(string filePath)
    {
        for (var i = 0; i < int.MaxValue; i++)
        {
            var output = new List<int>();
            var registers = new Dictionary<string, int> { { "a", i }, { "b", 0 }, { "c", 0 }, { "d", 0 } };
            var expected = new[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, };
            Run(registers, ref output, File.ReadAllLines(filePath));
            if (output.SequenceEqual(expected)) return i;
        }

        return 1;
    }


    private static void Run(Dictionary<string, int> registers, ref List<int> output, string[] input)
    {
        for (var i = 0; i < input.Length; i++)
        {
            var values = input[i].Split(' ');
            var instruction = values[0];

            switch (instruction)
            {
                case "cpy":
                {
                    var (x, y) = (values[1], values[2]);
                    registers[y] = int.TryParse(x, out var result) ? result : registers[x];
                    break;
                }
                case "inc":
                {
                    var x = values[1];
                    registers[x] += 1;
                    break;
                }
                case "dec":
                {
                    var x = values[1];
                    registers[x] -= 1;
                    break;
                }
                case "jnz":
                {
                    var (x, y) = (values[1], values[2]);
                    var xValue = int.TryParse(x, out var xResult) ? xResult : registers[x];
                    var yValue = int.TryParse(y, out var yResult) ? yResult : registers[y];
                    if (xValue != 0) i += yValue - 1;
                    break;
                }
                case "tgl":
                {
                    var x = values[1];
                    var xValue = int.TryParse(x, out var xResult) ? xResult : registers[x];
                    if (i + xValue < input.Length)
                        input[i + xValue] = Toggle(input[i + xValue]);
                    break;
                }
                case "out":
                {
                    var x = values[1];
                    var xValue = int.TryParse(x, out var xResult) ? xResult : registers[x];
                    output.Add(xValue);
                    if (output.Count >= 10) return;
                    break;
                }
                default:
                    throw new InvalidOperationException($"Unsupported Instruction: {instruction}");
            }
        }
    }

    private static string Toggle(string instruction) =>
        instruction[..3] switch
        {
            "inc" => instruction.Replace("inc", "dec"),
            "dec" => instruction.Replace("dec", "inc"),
            "tgl" => instruction.Replace("tgl", "inc"),
            "jnz" => instruction.Replace("jnz", "cpy"),
            "cpy" => instruction.Replace("cpy", "jnz"),
            _ => throw new InvalidOperationException()
        };
}
