namespace SlimScript;

public class Identifier
{
    public static IVariable Identify(Token[] tokens, SourceChunk chunk)
    {
        var identifier = tokens[0];
        IVariable? variable;

        variable = chunk.GetVar(identifier.Text);

        if (variable == null)
            chunk.Error($"Cannot get value of unexistent variable '{identifier.Text}'.", ExitCode.NullReferenceError);

        return variable ?? new Null();
    }
}
