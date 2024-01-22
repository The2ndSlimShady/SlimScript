namespace SlimScript;

public struct Number : IVariable
{
    public Token Token { get; set; }
    public TokenType Type { get; set; } = TokenType.Number;

    public double Val = 0;

    public string Name { get; set; } = "";

    public object Value
    {
        get => Val;
        set { Val = (double)value; }
    }

    public Number(Token[] tokens, SourceChunk chunk)
    {
        if (tokens[0].Type == TokenType.Number)
        {
            Val = Convert.ToDouble(tokens[0].Text.Replace(".", ","));
            Token = tokens[0];
        }
        else if (tokens[0].Type == TokenType.Identifier)
        {
            Token = new Token();
            Val = 0;
            Name = "";
        }
        else
        {
            var rule = Parser.IdentifyAndGet(tokens.ToList(), chunk);

            var obj = Variable.CreateType(rule);

            Number? value;

            if (obj?.GetType().BaseType == typeof(Operator))
            {
                value = (Number?)(obj as Operator)?.Apply(tokens, chunk);

                Val = value?.Val ?? 0;
                Token = value?.Token ?? new("0");
            }
            else if (obj?.GetType().BaseType == typeof(Standart))
            {
                value = (Number?)(obj as Standart)?.Run(tokens.ToList(), chunk);

                Val = value?.Val ?? 0;
                Token = value?.Token ?? new("0");
            }
            else
            {
                chunk.Error(
                    $"Cannot create Number from return of {tokens[0].Type}.",
                    ExitCode.DisordantTokenError
                );

                Token = new Token();
            }
        }
    }

    public Number(Token token)
    {
        Val = Convert.ToDouble(token.Text.Replace(".", ","));
        Token = token;
    }

    public static Number operator +(Number left, Number Right)
    {
        var num = new Number();
        var token = new Token();

        num.Val = left.Val + Right.Val;

        token.Type = TokenType.Number;
        token.Text = num.Val.ToString();

        num.Token = token;

        return num;
    }

    public static Number operator -(Number left, Number right)
    {
        var num = new Number();
        var token = new Token();

        num.Val = left.Val - right.Val;

        token.Type = TokenType.Number;
        token.Text = num.Val.ToString();

        num.Token = token;

        return num;
    }

    public string GetString() => Token.Text?.Replace(",", ".") ?? "";

    public override string ToString() => (this as IVariable).GetString();
}
