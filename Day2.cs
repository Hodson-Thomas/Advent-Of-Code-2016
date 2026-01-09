namespace Aoc16;

public class Day2
{
    private static readonly int[][] Keypad1 =
    [
        [1, 2, 3],
        [4, 5, 6],
        [7, 8, 9]
    ];

    private static readonly char[][] Keypad2 =
    [
        ['0', '0', '1', '0', '0'],
        ['0', '2', '3', '4', '0'],
        ['5', '6', '7', '8', '9'],
        ['0', 'A', 'B', 'C', '0'],
        ['0', '0', 'D', '0', '0']
    ];

    public static string Part1(string filePath)
    {
        var instructions = _parseInput(filePath);
        var position = (1, 1);
        var code = "";
        foreach (var line in instructions)
        {
            position = line.Aggregate(position, _move1);
            code += Keypad1[position.Item2][position.Item1];
        }

        return code;
    }

    public static string Part2(string filePath)
    {
        var instructions = _parseInput(filePath);
        var position = (0, 2);
        var code = "";
        foreach (var line in instructions)
        {
            position = line.Aggregate(position, _move2);
            code += Keypad2[position.Item2][position.Item1];
        }

        return code;
    }

    private static (int, int) _move1((int, int) position, Instruction instruction) => instruction switch
    {
        Instruction.Left => (_inRange(0, 2, position.Item1 - 1), position.Item2),
        Instruction.Up => (position.Item1, _inRange(0, 2, position.Item2 - 1)),
        Instruction.Down => (position.Item1, _inRange(0, 2, position.Item2 + 1)),
        Instruction.Right => (_inRange(0, 2, position.Item1 + 1), position.Item2),
        _ => throw new ArgumentOutOfRangeException(nameof(instruction), instruction, null)
    };

    private static (int, int) _move2((int, int) position, Instruction instruction) => instruction switch
    {
        Instruction.Left => _attemptMove(position, (_inRange(0, 4, position.Item1 - 1), position.Item2)),
        Instruction.Up => _attemptMove(position, (position.Item1, _inRange(0, 4, position.Item2 - 1))),
        Instruction.Down => _attemptMove(position, (position.Item1, _inRange(0, 4, position.Item2 + 1))),
        Instruction.Right => _attemptMove(position, (_inRange(0, 4, position.Item1 + 1), position.Item2)),
        _ => throw new ArgumentOutOfRangeException(nameof(instruction), instruction, null)
    };

    private static (int, int) _attemptMove((int, int) position, (int, int) move) =>
        Keypad2[move.Item2][move.Item1] == '0' ? position : move;

    private static int _inRange(int min, int max, int value) => value <= min ? min : value >= max ? max : value;

    private static List<List<Instruction>> _parseInput(string filePath)
    {
        using StreamReader reader = new(filePath);
        List<List<Instruction>> instructions = [];
        while (reader.ReadLine() is { } line)
        {
            line = line.Trim();
            if (line.Length == 0) continue;
            instructions.Add(line.Select(c => c switch
            {
                'D' => Instruction.Down,
                'L' => Instruction.Left,
                'R' => Instruction.Right,
                'U' => Instruction.Up,
                _ => throw new ArgumentOutOfRangeException()
            }).ToList());
        }

        return instructions;
    }


    private enum Instruction
    {
        Left,
        Up,
        Down,
        Right
    }
}