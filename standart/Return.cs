namespace SlimScript;

internal class Return : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var variable = Variable.Create(line.ToArray()[1..], chunk);

        chunk.Return();

        return variable;
    }
}
