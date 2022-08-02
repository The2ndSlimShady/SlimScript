using System;
using System.Diagnostics.CodeAnalysis;

namespace SlimScript;

public struct Token
{
    public TokenType Type { get; set; }

    public string Text { get; set; }

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
                Type = TokenType.Boolean;
            else if (Grammar.operators.ContainsKey(Text))
                Type = TokenType.Operator;
            else if (Grammar.standarts.Contains(Text))
                Type = TokenType.Standart;
            else if (Grammar.keywords.Contains(Text))
                Type = TokenType.Keyword;
            else if (Text == "EOL")
                Type = TokenType.EOL;
            else if (Text.Contains("->") || Text.Contains('.'))
                Type = TokenType.CLR;
            else if (!string.IsNullOrEmpty(Text))
                Type = TokenType.Identifier;
            else
            {
                Type = TokenType.Unidentified;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nUnidentified Token: {Text}.");
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

    public override string ToString() => $"<{Type}>";

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(Token left, Token right) => !(left != right);

    public static bool operator !=(Token left, Token right) =>
        (left.Text != right.Text) && (left.Type != right.Type);
}

public enum TokenType
{
    Number,
    Operator,
    Boolean,
    Identifier,
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
