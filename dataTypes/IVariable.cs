namespace SlimScript;

internal interface IVariable
{
    Token Token { get; set; }

    string Name { get; set; }

    object Value { get; set; }
}
