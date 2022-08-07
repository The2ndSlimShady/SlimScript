namespace SlimScript;

internal class Append : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var varT = line.ToArray()[1..line.LastIndexOf(new("to"))];
        var var = Variable.Create(varT, chunk);

        var arrayT = Variable.Create(line.ToArray()[(line.LastIndexOf(new("to")) + 1)..], chunk);

        if (arrayT.GetType() != typeof(Array))
            chunk.Error(
                $"Cannot find indexer on type '{arrayT.Token}'",
                ExitCode.DisordantTokenError
            );

        var array = arrayT as Array ?? new Array();

        array.Val.Add(var);

        chunk.SetVar(array.Name, array);

        return chunk.GetVar(array.Name) ?? new Null();
    }
}
