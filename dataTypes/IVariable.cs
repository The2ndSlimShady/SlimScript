namespace SlimScript;

public interface IVariable
{
    Token Token { get; set; }

    TokenType Type { get; init; }

    string Name { get; set; }

    object Value { get; set; }

    string GetString() => Value.ToString() ?? "";

    string ToString() => GetString();
}
