using utility;

Assert.AreEqual("2000 103 123 4444 99", WeightSort.orderWeight("103 123 4444 99 2000"));
Assert.AreEqual("11 11 2000 10003 22 123 1234000 44444444 9999", WeightSort.orderWeight("2000 10003 1234000 44444444 9999 11 11 22 123"));

class WeightComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        var sx = x.Sum(c => c - 48);
        var sy = y.Sum(c => c - 48);
        if (sx == sy)
        {
            return x.CompareTo(y);
        }
        return sx.CompareTo(sy);
    }
}

public class WeightSort
{
    public static string orderWeight(string strng)
    {
        return string.Join(" ", strng.Split().OrderBy(s => s, new WeightComparer()));
    }
}