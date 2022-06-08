
ExampleTests.TestUp();
//ExampleTests.TestDown();
//ExampleTests.TestDownAndDown();
//ExampleTests.TestUpAndUp();

public class Dinglemouse
{
    public static int[] TheLift(int[][] queues, int capacity)
    {
        var stops = new List<int>();
        var floorCount = queues.Length;

        return stops.ToArray();
    }

}
class Building
{
    public Floor[] floors;
}
class Floor
{
    public int[] queue;
}
class Elevator
{
    public int currentFloor;
    public bool direction;
    public readonly int capacity;
    public int[] queue;
    public List<int> stops;

    public Building building;

    public IEnumerable<int> Move(bool direction)
    {
        do
        {
            /*
             * //unload passenger
             * if queue.Count > 0
             *   if currentFloor.queue.Remove(p=>p==currentFloor) > 0
             *     stops.Add(currentFloor);
             *  
             * if queue.Count < capacity
             *   newPassenger = currentFloor.queue.Where(p=>direction?p>currentFloor:p<currentFloor).Take(capacity - queues.Count);
             *   currentFloor.Remove(newPassenger);
             *   queue.Add(newPassenger);
             * 
             * 
             * goto next floor by direction
             */
        } while (currentFloor < building.floors.Length && currentFloor < 0);
    }
}

public class Assert
{
    public static void AreEqual(int[] expected, int[] actual)
    {
        if (expected.SequenceEqual(actual))
        {
            Console.WriteLine("OK");
            return;
        }
        Console.WriteLine($"[{string.Join(", ", expected)}] != [{string.Join(", ", actual)}]");
    }
}
public class ExampleTests
{
    public static void TestUp()
    {
        int[][] queues =
        {
            new int[0], // G
            new int[0], // 1
            new int[]{5,5,5}, // 2
            new int[0], // 3
            new int[0], // 4
            new int[0], // 5
            new int[0], // 6
        };
        var result = Dinglemouse.TheLift(queues, 5);
        Assert.AreEqual(new[] { 0, 2, 5, 0 }, result);
    }

    public static void TestDown()
    {
        int[][] queues =
        {
            new int[0], // G
            new int[0], // 1
            new int[]{1,1}, // 2
            new int[0], // 3
            new int[0], // 4
            new int[0], // 5
            new int[0], // 6
        };
        var result = Dinglemouse.TheLift(queues, 5);
        Assert.AreEqual(new[] { 0, 2, 1, 0 }, result);
    }

    public static void TestUpAndUp()
    {
        int[][] queues =
        {
            new int[0], // G
            new int[]{3}, // 1
            new int[]{4}, // 2
            new int[0], // 3
            new int[]{5}, // 4
            new int[0], // 5
            new int[0], // 6
        };
        var result = Dinglemouse.TheLift(queues, 5);
        Assert.AreEqual(new[] { 0, 1, 2, 3, 4, 5, 0 }, result);
    }

    public static void TestDownAndDown()
    {
        int[][] queues =
        {
            new int[0], // G
            new int[]{0}, // 1
            new int[0], // 2
            new int[0], // 3
            new int[]{2}, // 4
            new int[]{3}, // 5
            new int[0], // 6
        };
        var result = Dinglemouse.TheLift(queues, 5);
        Assert.AreEqual(new[] { 0, 5, 4, 3, 2, 1, 0 }, result);
    }
}