namespace SlimScript;

internal class Set : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        if (line[1].Text != "index")
        {
            var name = line[1];
            var keyword = line[2];

            if (keyword.Text != "to")
                chunk.Error(
                    $"Unexpected keyword. Expected 'to' got '{keyword.Text}'",
                    ExitCode.DisordantTokenError
                );

            IVariable variable = Variable.Create(line.ToArray()[3..], chunk);

            chunk.SetVar(name.Text, variable);

            return chunk.GetVar(name.Text);
        }
        else
        {
            var indexTs = line.ToArray()[2..line.IndexOf(new("of"))];
            var indexT = Variable.Create(indexTs, chunk);

            if (indexT.Token.Type != TokenType.Number)
                chunk.Error(
                    $"Cannot convert from type '{indexT}' to type '<Number>'",
                    ExitCode.DisordantTokenError
                );

            var index = (Number)indexT;

            var arrayT = Variable.Create(
                line.ToArray()[(line.IndexOf(new("of")) + 1)..line.LastIndexOf(new("to"))],
                chunk
            );

            if (arrayT.GetType() != typeof(Array))
                chunk.Error(
                    $"Cannot find indexer on type '{arrayT.Token}'",
                    ExitCode.DisordantTokenError
                );

            var array = (Array)arrayT;

            array.Val[(int)index.Val] = Variable.Create(
                line.ToArray()[(line.IndexOf(new("to")) + 1)..],
                chunk
            );

            chunk.SetVar(array.Name, array);

            return chunk.GetVar(array.Name);
        }
    }
}
