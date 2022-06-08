using System.Text;

using utility;

var test = new SolutionTest();
test.FixedTests();

public static class Kata
{
    static readonly Dictionary<string, int> cardValueLookup = new Dictionary<string, int>
    {
        {"A", 14}, {"K", 13}, {"Q", 12}, {"J", 11},
        {"1", 10}, {"9", 9}, {"8", 8}, {"7", 7}, {"6", 6},
        {"5", 5}, {"4", 4}, {"3", 3}, {"2", 2},
        {"♥", 4}, {"♦", 3}, {"♣", 2}, {"♠", 1},
    };
    public static (string type, string[] ranks) Hand(string[] holeCards, string[] communityCards)
    {
        var sorted = holeCards.Concat(communityCards).OrderByDescending(c => (cardValueLookup[c[0] + ""]) * 10 + cardValueLookup[c.Last() + ""]);

        var flush = sorted.GroupBy(c => c.Last()).Where(g => g.Count() >= 5);
        var straight_flush = flush.SelectMany(g => g.ToList()).Take(5).Reverse().Select((c, i) => cardValueLookup[c[0] + ""] - i).Distinct();
        if (straight_flush.Count() == 1)
        {
            return ("straight-flush", flush.SelectMany(g => g.ToList().Select(c => c[0] == '1' ? "10" : c[0] + "")).Take(5).ToArray());
        }

        var byrank = sorted.GroupBy(c => c.First());
        var bysuit = sorted.GroupBy(c => c.Last());

        var four_of_a_kind = byrank.Where(g => g.Count() == 4);
        if (four_of_a_kind.Any())
        {
            var lastCard = sorted.Except(four_of_a_kind.Take(1).SelectMany(g => g.ToList())).Take(1);
            return ("four-of-a-kind", four_of_a_kind.SelectMany(g => g.ToList()).Concat(lastCard).Select(c => c[0] == '1' ? "10" : c[0] + "").Distinct().ToArray());
        }

        var three_of_a_kind = byrank.Where(g => g.Count() == 3);
        var pair = byrank.Where(g => g.Count() == 2);
        if (three_of_a_kind.Any() && pair.Any())
        {
            return ("full house", three_of_a_kind.Take(1).Concat(pair.Take(1)).SelectMany(g => g.ToList().Select(c => c[0] == '1' ? "10" : c[0] + "")).Distinct().ToArray());
        }

        if (flush.Any())
        {
            return ("flush", flush.SelectMany(g => g.ToList().Take(5).Select(c => c[0] == '1' ? "10" : c[0] + "")).ToArray());
        }
        var ttt = sorted.Distinct().Take(5);
        var straight = sorted.Select(c => cardValueLookup[c[0] + ""]).Distinct().Take(5).Reverse().Select((c, i) => c - i).Distinct();
        if (straight.Count() == 1)
        {
            return ("straight", sorted.Select(c => c[0]).Distinct().Take(5).Select(c => c == '1' ? "10" : c + "").ToArray());
        }

        if (three_of_a_kind.Any())
        {
            var lastCard = sorted.Except(three_of_a_kind.Take(1).SelectMany(g => g.ToList())).Take(2);
            return ("three-of-a-kind", three_of_a_kind.Take(1).SelectMany(g => g.ToList()).Concat(lastCard).Select(c => c[0] == '1' ? "10" : c[0] + "").Distinct().ToArray());
        }

        if (pair.Count() == 2)
        {
            var lastCard = sorted.Except(pair.Take(2).SelectMany(g => g.ToList())).Take(1);
            return ("two pair", pair.Take(2).SelectMany(g => g.ToList()).Concat(lastCard).Select(c => c[0] == '1' ? "10" : c[0] + "").Distinct().ToArray());
        }

        if (pair.Any())
        {
            var lastCard = sorted.Except(pair.Take(1).SelectMany(g => g.ToList())).Take(3);
            return ("pair", pair.Take(1).SelectMany(g => g.ToList()).Concat(lastCard).Select(c => c[0] == '1' ? "10" : c[0] + "").Distinct().ToArray());
        }

        return ("nothing", sorted.Take(5).Select(c => c[0] == '1' ? "10" : c[0] + "").ToArray());
    }
}

public class SolutionTest
{
    #region Sample Tests

    public void FixedTests()
    {
        // expected: hand name         cards                       input -> hole cards           community cards
        //SampleTest(("nothing", new[] { "A", "K", "Q", "J", "9" }), new[] { "K♠", "A♦" }, new[] { "J♣", "Q♥", "9♥", "2♥", "3♦" });
        //SampleTest(("pair", new[] { "Q", "K", "J", "9" }), new[] { "K♠", "Q♦" }, new[] { "J♣", "Q♥", "9♥", "2♥", "3♦" });
        //SampleTest(("two pair", new[] { "K", "J", "9" }), new[] { "K♠", "J♦" }, new[] { "J♣", "K♥", "9♥", "2♥", "3♦" });
        //SampleTest(("two pair", new[] { "K", "J", "9" }), new[] { "K♠", "J♦" }, new[] { "J♣", "K♥", "9♥", "2♥", "3♦" });
        //SampleTest(("three-of-a-kind", new[] { "Q", "J", "9" }), new[] { "4♠", "9♦" }, new[] { "J♣", "Q♥", "Q♠", "2♥", "Q♦" });
        //SampleTest(("straight", new[] { "K", "Q", "J", "10", "9" }), new[] { "Q♠", "2♦" }, new[] { "J♣", "10♥", "9♥", "K♥", "3♦" });
        //SampleTest(("straight", new[] { "Q", "J", "10", "9", "8" }), new[] { "Q♠", "J♥" }, new[] { "7♦", "8♥", "Q♦", "10♦", "9♣" });
        SampleTest(("straight", new[] { "10", "9", "8", "7", "6" }), new[] { "10♣", "9♥" }, new[] { "6♠", "7♥", "8♠", "K♥", "Q♥" });
        //SampleTest(("flush", new[] { "Q", "J", "10", "5", "3" }), new[] { "A♠", "K♦" }, new[] { "J♥", "5♥", "10♥", "Q♥", "3♥" });
        //SampleTest(("full house", new[] { "A", "K" }), new[] { "A♠", "A♦" }, new[] { "K♣", "K♥", "A♥", "Q♥", "3♦" });
        //SampleTest(("four-of-a-kind", new[] { "2", "3" }), new[] { "2♠", "3♦" }, new[] { "2♣", "2♥", "3♠", "3♥", "2♦" });
        //SampleTest(("straight-flush", new[] { "J", "10", "9", "8", "7" }), new[] { "8♠", "6♠" }, new[] { "7♠", "5♠", "9♠", "J♠", "10♠" });
    }

    private static void SampleTest((string type, string[] ranks) expected, string[] holeCards, string[] communityCards)
    {
        Console.WriteLine("??: {0} - {1}", expected.type, string.Join(", ", expected.ranks));
        var actual = Act(holeCards, communityCards);
        Verify(expected, actual, holeCards, communityCards);
        Console.WriteLine("OK: {0} - {1}", actual.type, string.Join(", ", actual.ranks));
    }

    #endregion

    private static readonly StringBuilder template = new StringBuilder();
    private static readonly StringBuilder buffer = new StringBuilder();
    private static readonly string[] ranks = new string[] { "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2" };
    private static readonly string[] types = new string[] { "straight-flush", "four-of-a-kind", "full house", "flush", "straight", "three-of-a-kind", "two pair", "pair", "nothing" };
    private static readonly Dictionary<string, int> ranksLookup = ranks.ToDictionary(x => x, x => Array.FindIndex(ranks, y => y == x));
    private static string Show(string str) => $@"""{str}""";
    private static string ShowSeq(IEnumerable<string> seq) => $"[{string.Join(", ", seq.Select(Show))}]";
    private static (string type, string[] ranks) Act(string[] holeCards, string[] communityCards) => Kata.Hand(holeCards, communityCards);

    private static string Error(string message)
    {
        buffer.Clear();
        buffer.Append(template.ToString());
        buffer.AppendLine($"Error: {message}");
        return buffer.ToString();
    }

    private static void Verify(
        (string type, string[] ranks) expected, (string type, string[] ranks) actual, string[] holeCards, string[] communityCards)
    {
        template.Clear();
        template.AppendLine($"\tHole cards: {ShowSeq(holeCards)}");
        template.AppendLine($"\tCommunity cards: {ShowSeq(communityCards)}");
        template.AppendLine($"Expected: (type: {Show(expected.type)}, ranks: {ShowSeq(expected.ranks)})");
        Assert.IsNotNull(actual.type, Error("Type must not be null"));
        Assert.IsNotNull(actual.ranks, Error("Ranks must not be null"));
        template.AppendLine($"Actual: (type: {Show(actual.type)}, ranks: {ShowSeq(actual.ranks)})");
        Assert.IsTrue(types.Any(x => string.Equals(x, actual.type)),
            Error($"{Show(actual.type)} is not valid, valid options are: {ShowSeq(types)}"));
        Assert.AreEqual(expected.type, actual.type, Error("Type is incorrect"));
        Assert.AreEqual(expected.ranks.Length, actual.ranks.Length, Error("Number of ranks is incorrect"));
        for (var i = 0; i < expected.ranks.Length; i++)
            Assert.IsTrue(ranks.Any(x => string.Equals(x, actual.ranks[i])),
                Error($"{Show(actual.ranks[i])} is not valid, valid options are: {ShowSeq(ranks)}"));
        for (var i = 0; i < expected.ranks.Length; i++)
            Assert.AreEqual(expected.ranks[i], actual.ranks[i], Error($"Rank at position {i} is incorrect"));
    }
}