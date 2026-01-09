namespace Aoc16;

public class Day13
{
    private const int SecretNumber = 1352;
    private static readonly Dictionary<(int, int), int> Nodes = new();
    private static readonly PriorityQueue<(int, int), int> Queue = new();

    public static int Part1()
    {
        _solve(1, 1);
        return Nodes[(31, 39)];
    }
    
    public static int Part2()
    {
        _solve(1, 1);
        return Nodes.Count(x => x.Value <= 50);
    }

    private static void _solve(int startX, int startY)
    {
        Queue.Enqueue((startX, startY), 0);
        Nodes[(startX, startY)] = 0;
        
        while (Queue.Count > 0)
        {
            var node = Queue.Dequeue();
            var (x, y) = node;

            foreach (var neighbor in new List<(int, int)> {(x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)})
            {
                var (nx, ny) = neighbor;
                if (nx < 0 || ny < 0 || _isWall(nx, ny))
                    continue;
                var d = Nodes[node] + 1;
                if (Nodes.TryGetValue(neighbor, out var nd) && d >= nd) continue;
                Nodes[neighbor] = d;
                Queue.Enqueue(neighbor, d);
            }
        }
    }
    
    private static bool _isWall(int x, int y) =>
        Convert.ToString(x * x + 3 * x + 2 * x * y + y + y * y + SecretNumber, 2)
            .Count(c => c == '1') % 2 != 0;
}