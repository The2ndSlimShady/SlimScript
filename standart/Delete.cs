namespace SlimScript;

internal class Delete : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var name = line[1];

        if (name.Type != TokenType.Identifier)
            chunk.Error($"Cannot delete variable '{name.Text}'. Given token is not an identifier.", ExitCode.GrammarError);

        var variable = Variable.Create(new[] {name}, chunk);

        chunk.DeleteVar(variable.Name);

        return variable;
    }
}
