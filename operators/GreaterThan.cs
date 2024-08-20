namespace SlimScript;

internal class GreaterThan : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
       var lesser = new LesserEqual().Apply(line, chunk);

       lesser = new Not().Apply(new[] {lesser.Token, lesser.Token}, chunk);

       return lesser;
    }
}
