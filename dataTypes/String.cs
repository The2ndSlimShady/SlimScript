namespace SlimScript;

internal struct Word : IVariable
{
    public Token Token { get; set; }

    public string Val = "";

    public string Name { get; set; } = "";

    public object Value
    {
        get => Val;
        set { Val = (string)value; }
    }

    public Word(Token[] tokens, SourceChunk chunk)
    {
        if (tokens[0].Type == TokenType.String)
        {
            Val = tokens[0].Text.Replace("\"", "");
            Token = tokens[0];
        }
        else
        {
            var rule = Parser.IdentifyAndGet(tokens.ToList(), chunk);

            var obj = Variable.CreateType(rule);

            Word value;

            if (obj.GetType().BaseType == typeof(Operator))
            {
                value = (Word)(obj as Operator).Apply(tokens, chunk);

                Val = value.Val;
                Token = value.Token;
            }
            else
            {
                chunk.Error($"Cannot create String from return of {tokens[0].Type}.", ExitCode.DisordantTokenError);

                Token = new Token();
            }
        }
    }

    public Word(Token token)
    {
        Val = token.Text.Replace("\"", "");
        Token = token;
    }

    public static Word operator +(Word left, Word Right)
    {
        var str = new Word();
        var token = new Token();

        str.Val = left.Val + Right.Val;

        token.Type = TokenType.String;
        token.Text = str.Val.ToString();

        str.Token = token;

        return str;
    }
}
