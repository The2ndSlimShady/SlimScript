namespace SlimScript;

internal abstract class Standart
{
    public abstract IVariable Run(List<Token> line, SourceChunk chunk);
}