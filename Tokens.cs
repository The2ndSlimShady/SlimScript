using System.Collections;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SlimScript;

public struct Token
{
    public TokenType Type { get; set; }

    public string Text { get; set; } = "";

    public Token(string source)
    {
        try
        {
            Text = source;

            if (double.TryParse(Text, out _))
                Type = TokenType.Number;
            else if (Text == "null")
                Type = TokenType.Null;
            else if (Text.StartsWith("\""))
                Type = TokenType.Word;
            else if (Text == "true" || Text == "false")
                Type = TokenType.Bool;
            else if (Grammar.operators.ContainsKey(Text))
                Type = TokenType.Operator;
            else if (Grammar.standarts.Contains(Text))
                Type = TokenType.Standart;
            else if (Grammar.keywords.Contains(Text))
                Type = TokenType.Keyword;
            else if (Text == "EOL")
                Type = TokenType.EOL;
            else if (Text.Contains("->") || Text.Contains(':'))
                Type = TokenType.CLR;
            else if (Text.Trim()[0] == '{' || Text.Trim()[0] == '[')
                Type = TokenType.Symbol;
            else if (!string.IsNullOrEmpty(Text))
                Type = TokenType.Identifier;
            else
            {
                Type = TokenType.Unidentified;

                Console.ForegroundColor = ConsoleColor.Red;
                Write.StandartOutput.WriteLine($"\nUnidentified Token: {Text}.");
                Program.Exit(ExitCode.UnidentifiedToken);
            }
        }
        catch (Exception)
        {
            Program.Exit(ExitCode.LexerError);
            Text = "";
            Type = TokenType.Unidentified;
        }
    }

    public Token()
    {
        Type = TokenType.Null;
        Text = "null";
    }

    public override string ToString() => $"<{Type}: {Text}>";

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        else if (obj is not Token)
            return false;
        else
            return (Token)obj == this;
    }

    public static bool operator ==(Token left, Token right) => !(left != right);

    public static bool operator !=(Token left, Token right) =>
        (left.Text != right.Text) && (left.Type != right.Type);
}

public enum TokenType
{
    Number,
    Operator,
    Bool,
    Identifier,
    Symbol,
    EOL,
    Keyword,
    Unidentified,
    Standart,
    Function,
    Word,
    Array,
    Null,
    CLR
}
