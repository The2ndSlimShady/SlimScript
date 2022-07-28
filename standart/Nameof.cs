namespace SlimScript;

internal class Nameof : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var var = Variable.Create(line.ToArray()[1..], chunk);

        return new Word(new($"{var.Name}"));
    }
}