namespace Aoc16;

public class Day19
{
    private const int Value = 3018458;
    
    public static int Part1()
    {
        var elves = new Queue<int>();
        for (var i = 1; i <= Value; i++) elves.Enqueue(i);
        while (elves.Count != 1)
        {
            var elf = elves.Dequeue();
            elves.Dequeue();
            elves.Enqueue(elf);
        }
        
        return elves.Dequeue();
    }

    public static int Part2()
    {
        var elves = new Queue<int>();
        for (var i = 1; i <= Value; i++) elves.Enqueue(i);
        
        while (elves.Count != 1)
        {
            elves = _removeAt(elves, elves.Count / 2);
            elves.Enqueue(elves.Dequeue());
        }
        
        return elves.Dequeue();
    }

    private static Queue<int> _removeAt(Queue<int> queue, int index)
    {
        if (index < 0 || index >= queue.Count) return queue;
        Queue<int> newQueue = new();
        var i = 0;
        while (queue.Count > 0)
        {
            if (i != index) newQueue.Enqueue(queue.Dequeue());
            else queue.Dequeue();
            i++;
        }

        return newQueue;
    }
}