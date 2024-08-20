using System.Text;
using System.Collections;

namespace SlimScript;

public struct Word : IVariable
{
    public Token Token { get; set; } = new Token() { Type = TokenType.Unidentified };
    public TokenType Type { get; set; } = TokenType.Word;

    public string Val = "";

    public string Name { get; set; } = "";

    public object Value
    {
        get => Val;
        set => Val = (string)value;
    }

    public Word(Token[] tokens, SourceChunk chunk)
    {
        if (tokens[0].Type == TokenType.Word)
        {
            Val = ReplaceEscapes(tokens[0].Text.Replace("\"", string.Empty));
            Token = new(ReplaceEscapes(tokens[0].Text));
        }
        else
        {
            var rule = Parser.IdentifyAndGet(tokens.ToList(), chunk);

            var obj = Variable.CreateType(rule);

            Word? value;

            if (obj?.GetType().BaseType == typeof(Operator))
            {
                value = (Word?)(obj as Operator)?.Apply(tokens, chunk);

                Val = ReplaceEscapes(value?.Val ?? string.Empty);
                Token = new(ReplaceEscapes(value?.Token.Text ?? "\"\""));
            }
            else
            {
                chunk.Error(
                    $"Cannot create String from return of {tokens[0].Type}.",
                    ExitCode.DisordantTokenError
                );

                Token = new Token();
            }
        }
    }

    public Word(Token token)
    {
        Val = ReplaceEscapes(token.Text.Replace("\"", ""));
        Token = new(ReplaceEscapes(token.Text));
    }

    private static string ReplaceEscapes(string value)
    {
        for (int i = 0; i < Grammar.escape.Length - 1; i += 2)
            value = value.Replace(Grammar.escape[i], Grammar.escape[i + 1]);

        return value;
    }

    public static Word operator +(Word left, Word Right)
    {
        var str = new Word();
        var token = new Token();

        str.Val = left.Val + Right.Val;

        token.Type = TokenType.Word;
        token.Text = str.Val.ToString();

        str.Token = token;

        return str;
    }

    public IEnumerator<Word> GetEnumerator() =>
        Val.Select(c => (Word)Variable.ClrToVar(c)).GetEnumerator();

    public override string ToString() => Val;
}
