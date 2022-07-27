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

    public IVariable Run(IVariable[] parameters, SourceChunk chunk)
    {
        SourceChunk tempChunk = new(Val.Val.Lines, chunk);

        var lineNum = chunk.Parser.lineNumber - tempChunk.Lines.Count - 1;
        chunk.Parser.lineNumber = lineNum;

        for (int i = 0; i < parameters.Length; i++)
        {
            IVariable param = parameters[i];
            tempChunk.CreateVar(Val.param[i], param);
        }

        return tempChunk.Run(Val.begin);
    }
}
