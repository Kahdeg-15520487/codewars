using System;
using System.Text;

Assert.AreEqual("110", Kata.Add("91", "19"));
Assert.AreEqual("1111111111", Kata.Add("123456789", "987654322"));
Assert.AreEqual("1000000000", Kata.Add("999999999", "1"));
Assert.AreEqual("100", Kata.Add("99", "1"));

public class Kata
{
    public static string Add(string a, string b)
    {
        var length = Math.Max(a.Length, b.Length) + 2;
        a = a.PadLeft(length, '0');
        b = b.PadLeft(length, '0');
        var result = (new string('0', length)).ToCharArray();
        for (int i = length - 1; i > 0; i--)
        {
            int s = (result[i] - 48) + (a[i] - 48) + (b[i] - 48);
            if (s >= 10)
            {
                result[i] = (char)((s - 10) + 48);
                result[i - 1] = '1';
            }
            else
            {
                result[i] = (char)(s + 48);
            }
        }
        return new string(result).TrimStart('0'); // Fix this!
    }
}

public static class Assert
{
    public static void AreEqual(string expected, string actual)
    {
        if (expected != actual)
        {
            Console.WriteLine($"'{expected}' != '{actual}'");
            return;
        }
        Console.WriteLine("OK");
    }
}