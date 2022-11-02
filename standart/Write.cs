namespace SlimScript;

internal class Write : Standart
{
    public static TextWriter StandartOutput { get; set; } = Console.Out;

    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var args = Operator.ReadyParams(line.ToArray(), chunk, 0);

        string writeStr = string.Concat(args.Select(t => t.GetString()));

        StandartOutput.WriteLine(writeStr);

        return new Word(new($"\"{writeStr}\""));
    }
}
