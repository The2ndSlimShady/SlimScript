using System.Collections;

namespace SlimScript;

public struct Null : IVariable
{
    public Token Token { get; set; }

    public TokenType Type { get; set; } = TokenType.Null;

    public string Name { get; set; } = "";

    public object Value
    {
        get => null;
        set => _ = value;
    }

    public Null() => Token = new("null");

    public string GetString() => "null";

    public override string ToString() => (this as IVariable).GetString();
}
