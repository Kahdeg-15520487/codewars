using System;
using System.Collections.Generic;
using System.Text;

using utility;

var test = new Solution.SolutionTest();
test.TestSingle();
test.TestExpression();

class Simplexer : Iterator<Token>
{
    private readonly string source;
    private int pos;

    public Simplexer(string buffer)
    {
        source = buffer;
        pos = 0;
    }

    public override bool MoveNext()
    {
        if (pos == source.Length)
        {
            return false;
        }

        var c = source[pos];
        var s = new StringBuilder();

        if (IsWhiteSpace(c))
        {
            while (pos < source.Length && IsWhiteSpace(source[pos]))
            {
                s.Append(source[pos]);
                pos++;
            }
            Current = new Token(s.ToString(), "whitespace");
            return true;
        }

        if (c == '+'
         || c == '-'
         || c == '*'
         || c == '/'
         || c == '%'
         || c == '('
         || c == ')'
         || c == '=')
        {
            Current = new Token(c + "", "operator");
            pos++;
            return true;
        }

        if (IsDigit(c))
        {
            while (pos < source.Length && IsDigit(source[pos]))
            {
                s.Append(source[pos]);
                pos++;
            }
            Current = new Token(s.ToString(), "integer");
            return true;
        }

        if (c == '"')
        {
            s.Append('"');
            pos++;
            while (pos < source.Length && source[pos] != '"')
            {
                s.Append(source[pos]);
                pos++;
            }
            s.Append('"');
            Current = new Token(s.ToString(), "string");
            return true;
        }

        while (pos < source.Length && !IsWhiteSpace(source[pos]))
        {
            s.Append(source[pos]);
            pos++;
        }

        if (IsKeyword(s.ToString()))
        {
            Current = new Token(s.ToString(), "keyword");
            return true;
        }

        if (bool.TryParse(s.ToString(), out bool _))
        {
            Current = new Token(s.ToString(), "boolean");
            return true;
        }

        Current = new Token(s.ToString(), "identifier");
        return true;

        static bool IsWhiteSpace(char c)
        {
            return c == ' '
                     || c == '\t'
                     || c == '\r'
                     || c == '\n';
        }

        static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        static bool IsKeyword(string s)
        {
            return s == "if"
                || s == "else"
                || s == "for"
                || s == "while"
                || s == "return"
                || s == "func"
                || s == "break";
        }
    }

    public override Token Current
    {
        get; protected set;
    }
}
class Token : IEquatable<Token>
{
    public Token(string value = null, string type = null)
    {
        Value = value;
        Type = type;
    }

    public string Value { get; private set; }
    public string Type { get; private set; }

    public bool Equals(Token? other)
    {
        return other != null && other.Value == Value && other.Type == Type;
    }
    public override string ToString()
    {
        return $"token({Value},{Type})";
    }
}
class Iterator<T>
{
    public virtual bool MoveNext() { return false; }
    public virtual T Current { get; protected set; }
}

namespace Solution
{
    public class SolutionTest
    {
        public void TestEmpty()
        {
            Simplexer lexer = new Simplexer("");
            Assert.AreEqual(false, lexer.MoveNext());
        }

        public void TestSingle()
        {
            // Identifier
            Simplexer lexer = new Simplexer("x");
            Assert.AreEqual(true, lexer.MoveNext());
            Assert.AreEqual(new Token("x", "identifier"), lexer.Current, $"Should return token(x,identifier), instead get {lexer.Current}");

            // Boolean
            lexer = new Simplexer("true");
            Assert.AreEqual(true, lexer.MoveNext());
            Assert.AreEqual(new Token("true", "boolean"), lexer.Current, $"Should return token(true,boolean), instead get {lexer.Current}");

            // Integer
            lexer = new Simplexer("12345");
            Assert.AreEqual(true, lexer.MoveNext());
            Assert.AreEqual(new Token("12345", "integer"), lexer.Current, $"Should return token(12345,integer), instead get {lexer.Current}");

            // String
            lexer = new Simplexer("\"Hello\"");
            Assert.AreEqual(true, lexer.MoveNext());
            Assert.AreEqual(new Token("\"Hello\"", "string"), lexer.Current, $"Should return token(\"Hello\",string), instead get {lexer.Current}");

            // Keyword
            lexer = new Simplexer("break");
            Assert.AreEqual(true, lexer.MoveNext());
            Assert.AreEqual(new Token("break", "keyword"), lexer.Current, $"Should return token(break,keyword), instead get {lexer.Current}");
        }

        public void TestExpression()
        {
            // Simple Expression
            Simplexer lexer = new Simplexer("(1 + 2) - 5");
            Assert.AreEqual(new Token[] {
                            new Token("(", "operator"),
                            new Token("1", "integer"),
                            new Token(" ", "whitespace"),
                            new Token("+", "operator"),
                            new Token(" ", "whitespace"),
                            new Token("2", "integer"),
                            new Token(")", "operator"),
                            new Token(" ", "whitespace"),
                            new Token("-", "operator"),
                            new Token(" ", "whitespace"),
                            new Token("5", "integer")
                        }, ToArray(lexer));
        }

        public void TestStatement()
        {
            // Simple statement
            Simplexer lexer = new Simplexer("return x + 1");
            Assert.AreEqual(new Token[] {
                            new Token("return", "keyword"),
                            new Token(" ", "whitespace"),
                            new Token("x", "identifier"),
                            new Token(" ", "whitespace"),
                            new Token("+", "operator"),
                            new Token(" ", "whitespace"),
                            new Token("1", "integer")
                        }, ToArray(lexer));
        }

        private Token[] ToArray(Simplexer lexer)
        {
            List<Token> tokens = new List<Token>();
            while (lexer.MoveNext()) tokens.Add(lexer.Current);
            return tokens.ToArray();
        }
    }
}