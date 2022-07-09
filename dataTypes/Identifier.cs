namespace SlimScript;

internal class Identifier
{
    public static IVariable Identify(Token[] tokens, SourceChunk chunk)
    {
        var identifier = tokens[0];
        IVariable variable = null;

        if (identifier.Type != TokenType.Standart)
            variable = chunk.GetVar(identifier.Text);
        else
            variable = (Variable.CreateType(Parser.IdentifyAndGet(tokens.ToList())) as Standart).Run(
                tokens.ToList(),
                chunk,
                Parser.IdentifyRules(tokens.ToList())
            );

        return variable;
    }
}
