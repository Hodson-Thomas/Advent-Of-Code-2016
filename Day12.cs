namespace Aoc16;

class Day12
{
    public static int Part1(string filePath) =>
        _execute(new Dictionary<string, int>
        {
            { "a", 0 },
            { "b", 0 },
            { "c", 0 },
            { "d", 0 }
        }, _parseInput(filePath));
    
    public static int Part2(string filePath) =>
        _execute(new Dictionary<string, int>
        {
            { "a", 0 },
            { "b", 0 },
            { "c", 1 },
            { "d", 0 }
        }, _parseInput(filePath));

    private static int _execute(Dictionary<string, int> registers, List<IInstruction> instructions)
    {
        var cursor = 0;
        while (cursor >= 0 && cursor < instructions.Count)
            cursor += instructions[cursor].Execute(registers);
        
        return registers["a"];
    }

    private static List<IInstruction> _parseInput(string filePath)
    {
        using var reader = new StreamReader(filePath);
        List<IInstruction> instructions = [];
        while (reader.ReadLine() is { } line)
        {
            var content = line.Trim().Split(' ');
            switch (content[0])
            {
                case "cpy": 
                    instructions.Add(new Copy(
                        int.TryParse(content[1], out var val) ? new Literal(val) : new Register(content[1]), content[2]
                    ));
                    break;
                case "inc": 
                    instructions.Add(new Increment(content[1]));
                    break;
                case "dec": 
                    instructions.Add(new Decrement(content[1]));
                    break;
                case "jnz":
                    instructions.Add(new Jump(
                        int.TryParse(content[1], out var val2) ? new Literal(val2) : new Register(content[1]),
                        int.TryParse(content[2], out var val3) ? new Literal(val3) : new Register(content[2])
                    ));
                    break;
                default: continue;
            }
        }

        return instructions;
    }
}

public partial interface IValue
{
    int GetInt(Dictionary<string, int> registers);
}

public partial interface IInstruction
{
    int Execute(Dictionary<string, int> registers);
}

record Register(string Name) : IValue
{
    public int GetInt(Dictionary<string, int> registers) => registers[Name];
}

record Literal(int Value) : IValue
{
    public int GetInt(Dictionary<string, int> registers) => Value;
}

record Copy(IValue Value, string Register) : IInstruction
{
    public int Execute(Dictionary<string, int> registers)
    {
        registers[Register] = Value.GetInt(registers);
        return 1;
    }
}

record Increment(string Register) : IInstruction
{
    public int Execute(Dictionary<string, int> registers)
    {
        registers[Register]++;
        return 1;
    }
}

record Decrement(string Register) : IInstruction
{
    public int Execute(Dictionary<string, int> registers)
    {
        registers[Register]--;
        return 1;
    }
}

record Jump(IValue Value, IValue Shift) : IInstruction
{
    public int Execute(Dictionary<string, int> registers) =>
        Value.GetInt(registers) == 0 ? 1 : Shift.GetInt(registers);
}