using System.Text;

namespace SlimScript;

internal class GreaterEqual : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
       var lesser = new LesserThan().Apply(line, chunk);

       lesser = new Not().Apply(new[] {lesser.Token, lesser.Token}, chunk);

        return lesser;
    }
}
