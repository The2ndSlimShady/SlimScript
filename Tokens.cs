using System;

namespace SlimScript;

internal struct Token
{
    public TokenType Type { get; set; }
    public string Text { get; set; }

    public Token(string source)
    {
        try
        {
            Text = source.Replace(" ", "");

            if (double.TryParse(Text, out _))
                Type = TokenType.Number;
            else if (Text == "true" || Text == "false")
                Type = TokenType.Boolean;
            else if (Grammar._operators.Contains(Text))
                Type = TokenType.Operator;
            else if (Grammar._keywords.Contains(Text))
                Type = TokenType.Keyword;
            else if (Text == "EOL")
                Type = TokenType.EOL;
            else if (!Text.Any(ch => Grammar._operators.Contains($"{ch}")) && !string.IsNullOrEmpty(Text))
                Type = TokenType.Identifier;
            else
            {
                Type = TokenType.Unidentified;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nUnidentified Token: {Text}");
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

	public override string ToString() => $"<{Type}>";

    public static bool operator==(Token left, Token right) => !(left != right);
    public static bool operator!=(Token left, Token right) => (left.Text != right.Text) && (left.Type != right.Type);
}

internal enum TokenType
{
    Number,
    Operator,
    Boolean,
    Identifier,
    EOL,
    Keyword,
    Function,
    Unidentified
}
