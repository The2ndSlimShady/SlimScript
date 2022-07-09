namespace SlimScript;

internal struct Bool : IVariable
{
    public Token Token { get; set; }

    public bool Val = false;

    public string Name { get; set; } = "";

    public object Value
    {
        get => Val;
        set { Val = (bool)value; }
    }

    public Bool(Token[] tokens, SourceChunk chunk)
    {
        if (tokens[0].Type == TokenType.Boolean)
        {
            Val = Convert.ToBoolean(tokens[0].Text);
            Token = tokens[0];
        }
        else if (tokens[0].Type == TokenType.Identifier)
        {
            Token = new Token();
            Val = false;
            Name = "";
        }
        else
        {
            var rule = Parser.IdentifyAndGet(tokens.ToList());

            var obj = Variable.CreateType(rule);

            Bool value;

            if (obj.GetType().BaseType == typeof(Operator))
            {
                value = (Bool)(obj as Operator).Apply(tokens, chunk);

                Val = value.Val;
                Token = value.Token;
            }
            else if (obj.GetType().BaseType == typeof(Standart))
            {
                value = (Bool)(obj as Standart).Run(tokens.ToList(), chunk);

                Val = value.Val;
                Token = value.Token;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"Cannot create Bool from return of {tokens[0].Type}. Line: {Parser.lineNumber}"
                );

                Program.Exit(ExitCode.DisordantTokenError);

                Token = new Token();
            }
        }
    }

    public Bool(Token token)
    {
        Val = Convert.ToBoolean(token.Text);
        Token = token;
    }
}
