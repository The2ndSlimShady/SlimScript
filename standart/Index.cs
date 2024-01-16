namespace SlimScript;

internal class Index : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var indexTs = line.ToArray()[1..line.IndexOf(new("of"))];
        var indexT = Variable.Create(indexTs, chunk);

        if (indexT.Token.Type != TokenType.Number)
            chunk.Error(
                $"Cannot convert from type '{indexT}' to type '<Number>'",
                ExitCode.DisordantTokenError
            );

        var arrayT = Variable.Create(line.ToArray()[(line.IndexOf(new("of")) + 1)..], chunk);
        var index = (Number)indexT;

        if (arrayT.Token.Type == TokenType.Array)
        {
            var array = arrayT as Array ?? new Array();

            if (index.Val >= array.Val.Count)
                chunk.Error($"Index was out of the bounds of array", ExitCode.RuntimeError);

            return Variable.Copy(array.Val[(int)index.Val]);
        }
        else if (arrayT.Token.Type == TokenType.Word)
        {
            var str = (Word)arrayT;

            if (index.Val >= str.Val.Length)
                chunk.Error($"Index was out of the bounds of string", ExitCode.RuntimeError);

            return new Word(new(str.Val[(int)index.Val].ToString()));
        }
        else
        {
            chunk.Error(
                $"Cannot find indexer on type '{arrayT.Token}'",
                ExitCode.DisordantTokenError
            );

            return new Null();
        }
    }
}
