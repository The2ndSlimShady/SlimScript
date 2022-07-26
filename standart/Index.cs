namespace SlimScript;

internal class Index : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var indexT = line[1];

        if (indexT.Type != TokenType.Number)
            chunk.Error(
                $"Cannot convert from type '{indexT}' to type '<Number>'",
                ExitCode.DisordantTokenError
            );

        var arrayT = Variable.Create(line.ToArray()[2..], chunk);
        var index = new Number(indexT);

        if (arrayT.GetType() != typeof(Array))
            chunk.Error(
                $"Cannot find indexer on type '{arrayT.Token}'",
                ExitCode.DisordantTokenError
            );

        var array = arrayT as Array;

        if (index.Val >= array.Val.Count)
            chunk.Error($"Index was out of the bounds of array", ExitCode.RuntimeError);

        return array.Val[(int)index.Val];
    }
}
