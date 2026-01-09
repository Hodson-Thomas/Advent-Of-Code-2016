namespace Aoc16;

public class Day4
{
    private const string Map = "abcdefghijklmnopqrstuvwxyz";
    private const string Target = "northpole object storage";

    public static int Part1(string filePath)
        => ParseInput(filePath).Where(room => room.IsValid()).Select(room => room.Id).Sum();

    public static int Part2(string filePath) =>
        ParseInput(filePath).Where(room => room.IsValid())
            .Where(room => _decrypt(room.Name, room.Id) == Target)
            .Select(room => room.Id)
            .FirstOrDefault(-1);

    private static List<Room> ParseInput(string filePath)
    {
        using StreamReader reader = new(filePath);
        List<Room> rooms = [];
        while (reader.ReadLine() is { } line)
        {
            line = line.Trim();
            if (line.Length == 0) continue;
            var split = line.Split("-");
            var roomName = string.Join("-", split.Take(new Range(0, split.Length - 1)));
            var id = int.Parse(split.Last()[..3]);
            var checkSum = split.Last().Substring(4, 5);
            rooms.Add(new Room(roomName, id, checkSum));
        }

        return rooms;
    }

    private static string _decrypt(string str, int id)
    {
        var result = "";
        foreach (var c in str)
        {
            if (c == '-') result += ' ';
            else result += Map[(Map.IndexOf(c) + id) % Map.Length];
        }
        return result;
    } 

    private record Room(string Name, int Id, string Checksum)
    {
        public bool IsValid()
        {
            Dictionary<char, int> frequencies = [];
            foreach (var c in Name.Where(c => c != '-' && !frequencies.TryAdd(c, 1)))
                frequencies[c]++;

            var str = frequencies.OrderByDescending(x => x.Value)
                .ThenBy(x => x.Key)
                .Take(5)
                .Select(c => c.Key.ToString())
                .Aggregate((a, b) => a + b);

            return Checksum == str;
        }
    }
}