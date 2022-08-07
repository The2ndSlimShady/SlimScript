namespace SlimScript;

internal class Delete : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var name = line[1];

        if (name.Text == "index")
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
                line.ToArray()[(line.IndexOf(new("of")) + 1)..],
                chunk
            );

            if (arrayT.GetType() != typeof(Array))
                chunk.Error(
                    $"Cannot find indexer on type '{arrayT.Token}'",
                    ExitCode.DisordantTokenError
                );

            var array = (Array)arrayT;

            array.Val.RemoveAt((int)index.Val);

            chunk.SetVar(array.Name, array);

            return chunk.GetVar(array.Name) ?? new Null();
        }

        if (name.Type != TokenType.Identifier)
            chunk.Error($"Cannot delete variable '{name.Text}'. Given token is not an identifier.", ExitCode.GrammarError);

        var variable = Variable.Create(new[] {name}, chunk);

        chunk.DeleteVar(variable.Name);

        return variable;
    }
}
