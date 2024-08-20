namespace SlimScript;

internal class Do : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var name = line[1];

        if (name.Type != TokenType.Identifier)
            chunk.Error($"Cannot execute non-function variable '{name.Text}'. Given token is not an identifier.", ExitCode.GrammarError);

        Function? variable = (Function?)chunk.GetVar(name.Text);
        var parameters = Operator.ReadyParams(line.ToArray()[1..], chunk, 0).ToArray();

        return variable?.Run(parameters, chunk) ?? new Null();
    }
}
