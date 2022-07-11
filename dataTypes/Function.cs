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

    public Function(SourceChunk code, SourceChunk currentChunk, string[] parameters, int parameterCount = 0, string name = "")
    {
        Val.Val = code;
        Val.begin = currentChunk.Parser.lineNumber - code.Lines.Count - 1;

        Token token = new()
        {
            Type = TokenType.Function,
            Text = name,
        };

        Name = name;

        Token = token;
        Val.count = parameterCount;
        Val.param = parameters;
    }

    public IVariable Run(Token[] paramTokens)
    {
        List<IVariable> parameters = new(Val.count);

        for (int i = 0; i < Val.count; i++)
            parameters.Add(Variable.Create(paramTokens[i..(i+1)], Val.Val.Parent));

        for (int i = 0; i < parameters.Count; i++)
        {
            IVariable param = parameters[i];
            Val.Val.CreateVar(Val.param[i], param);
        }

        return Val.Val.Run(Val.begin);
    }
}
