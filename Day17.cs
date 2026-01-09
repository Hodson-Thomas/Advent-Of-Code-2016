using System.Security.Cryptography;
using System.Text;

namespace Aoc16;

public class Day17
{
    private const string Base = "awrkjxxr";
    private const string Open = "bcdef";
    private static readonly Dictionary<State, int> Nodes = new();
    private static readonly Queue<State> Queue = new();

    private record State((int X, int Y) Position, string Path)
    {
        public bool IsValid() => Position.X is >= 0 and <= 3 && Position.Y is >= 0 and <= 3;
    }

    public static string Part1() => _getShortestPath(Base);

    public static int Part2() => _getLongestPathLength(Base);

    private static string _getShortestPath(string input)
    {
        _solve(input);
        return Nodes.First(x => x.Key.Position == (3, 3)).Key.Path.Replace(input, "");
    }

    private static int _getLongestPathLength(string input)
    {
        _solve(input);
        return Nodes.Last(x => x.Key.Position == (3, 3)).Key.Path.Replace(input, "").Length;
    }

    private static void _solve(string input)
    {
        var initialState = new State((0, 0), input);
        Nodes[initialState] = 0;
        Queue.Enqueue(initialState);

        while (Queue.Count > 0)
        {
            var node = Queue.Dequeue();
            var neighbors = _getNeighbors(node);
            _processNeighbors(neighbors, Nodes[node] + 1);
        }
    }

    private static IEnumerable<State> _getNeighbors(State node)
    {
        var (x, y) = node.Position;
        var doors = _getDoorStates(node.Path);
        var n1 = Open.Contains(doors[0]) ? new State((x, y - 1), node.Path + 'U') : null;
        var n2 = Open.Contains(doors[1]) ? new State((x, y + 1), node.Path + 'D') : null;
        var n3 = Open.Contains(doors[2]) ? new State((x - 1, y), node.Path + 'L') : null;
        var n4 = Open.Contains(doors[3]) ? new State((x + 1, y), node.Path + 'R') : null;
        var neighbors = new[] { n1, n2, n3, n4 };

        return neighbors.Where(neighbor => neighbor is not null && neighbor.IsValid())!;
    }

    private static string _getDoorStates(string input) => _md5(input)[..4];

    private static void _processNeighbors(IEnumerable<State> neighbors, int steps)
    {
        foreach (var neighbor in neighbors)
        {
            if (neighbor.Position == (3, 3))
            {
                Nodes[neighbor] = steps;
                continue;
            }

            Nodes[neighbor] = steps;
            Queue.Enqueue(neighbor);
        }
    }
    
    private static string _md5(string input) =>
        Convert.ToHexString(MD5.HashData(Encoding.ASCII.GetBytes(input))).ToLower();
}