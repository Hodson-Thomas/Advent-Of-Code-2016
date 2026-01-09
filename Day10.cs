namespace Aoc16;

public class Day10
{
    public static string Part1(string filePath)
    {
        var data = _parseInput(filePath);
        var keys = data.Keys.ToArray();
        while (true)
        {
            foreach (var key in keys)
            {
                if (!data[key].CanGiveShips()) continue;
                if (data[key].IsComparingValues(61, 17)) return key;
                if (!data[data[key].ToMin!].CanReceiveValue() || !data[data[key].ToMax!].CanReceiveValue()) continue;
                data[data[key].ToMax!].GiveValue(data[key].GetMax());
                data[data[key].ToMin!].GiveValue(data[key].GetMin());
                data[key].Ship1 = null;
                data[key].Ship2 = null;
            }
        }
    }

    public static int Part2(string filePath)
    {
        var data = _parseInput(filePath);
        var keys = data.Keys.ToArray();
        while (true)
        {
            foreach (var key in keys)
            {
                if (!data[key].CanGiveShips()) continue;
                if (data["output 0"].HasValue() && data["output 1"].HasValue() && data["output 2"].HasValue())
                {
                    var output0 = data["output 0"].GetValue();
                    var output1 = data["output 1"].GetValue();
                    var output2 = data["output 2"].GetValue();
                    return output0 * output1 * output2;
                }
                if (!data[data[key].ToMin!].CanReceiveValue() || !data[data[key].ToMax!].CanReceiveValue()) continue;
                data[data[key].ToMax!].GiveValue(data[key].GetMax());
                data[data[key].ToMin!].GiveValue(data[key].GetMin());
                data[key].Ship1 = null;
                data[key].Ship2 = null;
            }
        }
        
        
    }
    
    private static Dictionary<string, Bot> _parseInput(string filePath)
    {
        Dictionary<string, Bot> data = new();
        using var reader = new StreamReader(filePath);
        while (reader.ReadLine() is { } line)
        {
            var split = line.Trim().Split(' ');
            if (split[0] == "value")
            {
                var bot = split[4] + " " + split[5];
                var value = int.Parse(split[1]);
                if (data.TryGetValue(bot, out var value1))
                    value1.GiveValue(value);
                else
                    data.Add(bot, new Bot(value, null, null, null));                    
            }
            else if (split[0] == "bot")
            {
                var bot = split[0] + " " + split[1];
                var destination1 = split[5] + " " + split[6];
                var destination2 = split[10] + " " + split[11];

                if (!data.ContainsKey(destination1)) data.Add(destination1, new Bot(null, null, null, null));
                if (!data.ContainsKey(destination2)) data.Add(destination2, new Bot(null, null, null, null));
                
                if (data.TryGetValue(bot, out var value1))
                {
                    value1.ToMin = destination1;
                    value1.ToMax = destination2;
                }
                else
                    data.Add(bot, new Bot(null, null, destination1, destination2));
            }
        }
        return data;
    }
}

public class Bot
{
    public int? Ship1;
    public int? Ship2;
    public string? ToMin;
    public string? ToMax;

    public Bot(int? ship1, int? ship2, string? toMin, string? toMax)
    {
        Ship1 = ship1;
        Ship2 = ship2;
        ToMin = toMin;
        ToMax = toMax;
    }
    
    public bool CanGiveShips() => Ship1 is not null && Ship2 is not null;

    public int GetMax() => Math.Max(Ship1!.Value, Ship2!.Value);
    public int GetMin() => Math.Min(Ship1!.Value, Ship2!.Value);

    public void GiveValue(int value)
    {
        if (Ship1 is null) Ship1 = value;
        else if (Ship2 is null) Ship2 = value;
        else throw new Exception("Could not give value to bot");
    }

    public bool IsComparingValues(int v1, int v2) =>
        (Ship1 is { } s1 && Ship2 is { } s2) && ((s1 == v1 && s2 == v2) || (s1 == v2 && s2 == v1));

    public bool CanReceiveValue() => Ship1 is null || Ship2 is null;

    public int GetValue() => Ship1 ?? Ship2!.Value;

    public bool HasValue() => Ship1 is not null || Ship2 is not null;
} 
