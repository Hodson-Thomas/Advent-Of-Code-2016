namespace Aoc16;

public class Day23
{
    public static int Part1(string filePath)
    {
        var cpu = new Cpu(Parse(File.ReadAllText(filePath)));
        cpu.SetRegisters(7);
        cpu.Execute();
        return cpu.GetValue("a");
    }

    public static int Part2(string filePath)
    {
        var cpu = new Cpu(Parse(File.ReadAllText(filePath)));
        cpu.SetRegisters(12);
        cpu.Execute();
        return cpu.GetValue("a"); 
    }
    
    static string[][] Parse(string input) =>
        input.Split('\n').Select(line => line.Split(' ')).ToArray();
}

class Cpu(string[][] program)
{
    private readonly string[][] _program = program;
    private readonly Dictionary<string, int> _registers = new();

    private static readonly Dictionary<string, string> _toggleMap = new Dictionary<string, string>()
    {
        {"inc", "dec"}, 
        {"dec", "inc"}, 
        {"tgl", "inc"}, 
        {"jnz", "cpy"}, 
        {"cpy", "jnz"}
    };

    public void SetRegisters(int a)
    {
        _registers["a"] = a;
        _registers["b"] = 0;
        _registers["c"] = 0;
        _registers["d"] = 0;
    }
    
    public int GetValue(string text) =>
        int.TryParse(text, out var val) ? val : _registers.GetValueOrDefault(text, 0);

    public void Execute()
    {
        var cursor = 0;
        while (cursor < _program.Length)
        {
            if (cursor == 5)
            {
                _registers["a"] += _registers["c"] * _registers["d"];
                _registers["c"] = 0;
                _registers["d"] = 0;
                cursor = 10;
            }

            var instruction = _program[cursor];
            switch (instruction[0])
            {
                case "cpy" when int.TryParse(instruction[2], out var _):
                    cursor += 1;
                    continue;
                case "cpy":
                    _registers[instruction[2]] = GetValue(instruction[1]);
                    break;
                case "inc":
                    _registers[instruction[1]]++;
                    break;
                case "dec":
                    _registers[instruction[1]]--;
                    break;
                case "jnz":
                {
                    if (GetValue(instruction[1]) != 0)
                        cursor += GetValue(instruction[2]) - 1;
                    break;
                }
                case "tgl":
                {
                    var change = cursor + GetValue(instruction[1]);
                    if (change < _program.Length)
                        _program[change][0] = _toggleMap[_program[change][0]];
                    break;
                }
            }

            cursor++;
        }
    }
}

 