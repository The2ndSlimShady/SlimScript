namespace SlimScript;

internal class Import : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var args = Operator.ReadyParams(line.ToArray(), chunk, 0);

        string file = string.Concat(args.Select(arg => arg.GetString()));

        SourceChunk fileChunk = new(file, chunk);

        return fileChunk.Run();
    }
}
