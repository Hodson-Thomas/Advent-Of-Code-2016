namespace Aoc16;

public class Day1
{
    public static int Part1(string filePath)
    {
        var orientation = Orientation.North;
        var position = (0, 0);
        foreach (var move in ParseInput(filePath))
        {
            orientation = Turn(orientation, move.Direction);
            position = Walk(position, orientation, move.Distance);
        }

        return Math.Abs(position.Item1) + Math.Abs(position.Item2);
    }

    public static int Part2(string filePath)
    {
        List<(int, int)> positions = [];
        var orientation = Orientation.North;
        var position = (0, 0);
        positions.Add(position);
        foreach (var move in ParseInput(filePath))
        {
            orientation = Turn(orientation, move.Direction);
            var steps = WalkStepByStep(position, orientation, move.Distance);

            foreach (var step in steps)
            {
                if (positions.Contains(step)) 
                    return Math.Abs(step.Item1) + Math.Abs(step.Item2);
                positions.Add(step);
            }

            position = steps.Last();
        }

        return -1;
    }
    
    private static List<Move> ParseInput(string filePath)
    {
        using StreamReader reader = new(filePath);
        List<Move> moves = [];
        moves.AddRange(from sequence in reader.ReadToEnd().Split(", ")
            where sequence.Length > 0
            let direction = sequence[0] == 'L' ? Direction.Left : Direction.Right
            select new Move(direction, int.Parse(sequence[1..])));
        return moves;
    }

    private static (int, int) Walk((int, int) position, Orientation orientation, int distance) => orientation switch
    {
        Orientation.North => (position.Item1, position.Item2 - distance),
        Orientation.South => (position.Item1, position.Item2 + distance),
        Orientation.East => (position.Item1 + distance, position.Item2),
        Orientation.West => (position.Item1 - distance, position.Item2),
        _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
    };

    private static List<(int, int)> WalkStepByStep((int, int) position, Orientation orientation, int distance)
    {
        var movement = orientation switch
        {
            Orientation.North => (0, -1),
            Orientation.South => (0, 1),
            Orientation.East => (1, 0),
            Orientation.West => (-1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
        };
        List<(int, int)> positions = [];
        for (var dist = 1; dist <= distance; dist++)
            positions.Add((position.Item1 + movement.Item1 * dist, position.Item2 + movement.Item2 * dist));
        return positions;
    }

    private static Orientation Turn(Orientation orientation, Direction direction) => orientation switch
    {
        Orientation.North => direction == Direction.Left ? Orientation.West : Orientation.East,
        Orientation.South => direction == Direction.Left ? Orientation.East : Orientation.West,
        Orientation.East => direction == Direction.Left ? Orientation.North : Orientation.South,
        Orientation.West => direction == Direction.Left ? Orientation.South : Orientation.North,
        _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
    };
    
    private enum Orientation
    {
        North,
        South,
        East,
        West
    }

    private enum Direction
    {
        Right = 0,
        Left = 1
    }

    private record Move(Direction Direction, int Distance);
}