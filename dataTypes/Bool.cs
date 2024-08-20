using System;

namespace SlimScript;

public struct Bool : IVariable
{
    public Token Token { get; set; }
    public TokenType Type { get; set; } = TokenType.Bool;

    public bool Val = false;

    public string Name { get; set; } = "";

    public object Value
    {
        get => Val;
        set { Val = (bool)value; }
    }

    internal Bool(Token[] tokens, SourceChunk chunk)
    {
        if (tokens[0].Type == TokenType.Bool)
        {
			Type = TokenType.Bool;
            Val = Convert.ToBoolean(tokens[0].Text);
            Token = tokens[0];
        }
        else if (tokens[0].Type == TokenType.Identifier)
        {
			Type = TokenType.Bool;
            Token = new Token(){ Type = TokenType.Bool };
            Val = false;
            Name = "";
        }
        else
        {
			Type = TokenType.Bool;
            var rule = Parser.IdentifyAndGet(tokens.ToList(), chunk);

            var obj = Variable.CreateType(rule);

            Bool? value;

            if (obj?.GetType().BaseType == typeof(Operator))
            {
                value = (Bool?)(obj as Operator)?.Apply(tokens, chunk);

                Val = value?.Val ?? false;
                Token = value?.Token ?? new("false");
            }
            else if (obj?.GetType().BaseType == typeof(Standart))
            {
                value = (Bool?)(obj as Standart)?.Run(tokens.ToList(), chunk);

                Token = value?.Token ?? new("false");
                Val = value?.Val ?? false;
            }
            else
            {
                chunk.Error(
                    $"Cannot create Bool from return of {tokens[0].Type}.",
                    ExitCode.DisordantTokenError
                );
                Token = new("false");
            }
        }
    }

    internal Bool(Token token)
    {
        Val = Convert.ToBoolean(token.Text);
        Token = token;
    }

    public override string ToString() => (this as IVariable).GetString();
}
