namespace SlimScript;

internal struct Function : IVariable
{
    public Token Token { get; set; }

    public (SourceChunk Val, int begin, int count, string[] param) Val;

    public string Name { get; set; } = "";

    public object Value
    {
        get => Val;
        set => Val = (ValueTuple<SourceChunk, int, int, string[]>)value;
    }

    public Function(
        SourceChunk code,
        SourceChunk currentChunk,
        string[] parameters,
        int parameterCount = 0,
        string name = ""
    )
    {
        Val.Val = code;
        Val.begin = currentChunk.Parser.lineNumber - code.Lines.Count - 1;

        Token token = new() { Type = TokenType.Function, Text = name, };

        Name = name;

        Token = token;
        Val.count = parameterCount;
        Val.param = parameters;
    }

    public IVariable Run(Token[] paramTokens, SourceChunk chunk)
    {
        List<IVariable> parameters = new(Val.count);

        SourceChunk tempChunk = new(Val.Val.Lines, chunk);

        for (int i = 0; i < Val.count; i++)
            parameters.Add(Variable.Create(paramTokens[i..(i + 1)], chunk));

        for (int i = 0; i < parameters.Count; i++)
        {
            IVariable param = parameters[i];
            tempChunk.CreateVar(Val.param[i], param, true);
        }

        return tempChunk.Run(Val.begin);
    }
}
