namespace Aoc16;

public class Day8
{
    public static int Solve(string filePath)
    {
        var instructions = _parseInput(filePath);
        var screen = new bool[6][];
        for (var i = 0; i < screen.Length; i++)
            screen[i] = new bool[50];
        screen = instructions.Aggregate(screen, (current, instruction) => instruction.UpdateScreen(current));
        _printScreen(screen);
        return screen.Sum(row => row.Count(pixel => pixel));
    }

    
    private static List<Instruction> _parseInput(string filePath)
    {
        List<Instruction> instructions = [];
        using StreamReader reader = new(filePath);
        while (reader.ReadLine() is { } line)
        {
            line = line.Trim();
            if (line.Length == 0) continue;
            if (line.StartsWith("rect "))
            {
                var split = line[5..].Split('x');
                instructions.Add(new Rect(int.Parse(split[0]), int.Parse(split[1])));
            }
            else if (line.StartsWith("rotate row y="))
            {
                var split = line[13..].Split(' ');
                instructions.Add(new RotateRow(int.Parse(split[0]), int.Parse(split[2])));
            }
            else if (line.StartsWith("rotate column x="))
            {
                var split = line[16..].Split(' ');
                instructions.Add(new RotateColumn(int.Parse(split[0]), int.Parse(split[2])));
            }
        }
        return instructions;
    }

    private static void _printScreen(bool[][] screen)
    {
        Console.WriteLine("");
        foreach (var row in screen)
        {
            foreach (var pixel in row)
                Console.Write(pixel ? '#' : ' ');
            Console.WriteLine("");
        }
        Console.WriteLine("");
    }

    private static bool[][] _clone(bool[][] screen)
    {
        var newScreen = new bool[6][];
        for (var row = 0; row < newScreen.Length; row++)
        {
            newScreen[row] = new bool[50];
            for (var col = 0; col < 50; col++)
                newScreen[row][col] = screen[row][col];
        }

        return newScreen;
    }
    
    private abstract class Instruction(int A, int B)
    {
        public abstract bool[][] UpdateScreen(bool[][] screen);
    }
    
    private class Rect(int A, int B) : Instruction(A, B)
    {
        public override bool[][] UpdateScreen(bool[][] screen)
        {
            var newScreen = _clone(screen);
            for (var row = 0; row < B; row++)
            for (var col = 0; col < A; col++)
                newScreen[row][col] = true;
            return newScreen;
        }

        public override string ToString()
        {
            return "Rect " + A + " " + B;
        }
    }

    private class RotateRow(int A, int B) : Instruction(A, B)
    {
        public override bool[][] UpdateScreen(bool[][] screen)
        {
            var newScreen = _clone(screen);
            for (var col = 0; col < screen[A].Length; col++)
                newScreen[A][(col + B) % 50] = screen[A][col];
            return newScreen;
        }

        public override string ToString()
        {
            return "Rotate row " + A + " " + B;
        }
    }

    private class RotateColumn(int A, int B) : Instruction(A, B)
    {
        public override bool[][] UpdateScreen(bool[][] screen)
        {
            var newScreen = _clone(screen);
            for (var row = 0; row < screen.Length; row++)
                newScreen[(row + B) % screen.Length][A] = screen[row][A];
            return newScreen;
        }

        public override string ToString()
        {
            return "Rotate column " + A + " " + B;
        }
    }
}