namespace SlimScript;

internal class Break : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk) =>
        new Word(new("\"break\""));
}
