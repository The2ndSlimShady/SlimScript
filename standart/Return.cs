namespace SlimScript;

internal class Return : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        IVariable variable;

        if (line.Count != 1)
            variable = Variable.Create(line.ToArray()[1..], chunk);
        else
            variable = new Null();


        chunk.Return();

        return variable;
    }
}
