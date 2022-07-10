namespace SlimScript;

internal struct Function : IVariable
{
    public Token Token { get; set; }

    public SourceChunk Val;

    public string Name { get; set; } = "";

    public object Value
    {
        get => Val;
        set => Val = (SourceChunk)value;
    }

    public Function(SourceChunk code)
    {
        Val = code;

        Token token = new()
        {
            Type = TokenType.Function
        };

        Token = token;
    }

    public IVariable Run() => Val.Run();
}
