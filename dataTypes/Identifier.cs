namespace SlimScript;

internal class Identifier
{
    public static IVariable Identify(Token[] tokens, SourceChunk chunk)
    {
        var identifier = tokens[0];
        IVariable variable = null;

        variable = chunk.GetVar(identifier.Text);

        return variable;
    }
}
