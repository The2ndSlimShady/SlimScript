using System.Text;

namespace SlimScript;

internal class LesserEqual : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        Bool val = new();

        var first = (Bool)new Equals().Apply(line, chunk);
        var second = (Bool)new LesserThan().Apply(line, chunk);

        val.Val = first.Val || second.Val;
        val.Token = new Token(val.Val.ToString().ToLower());

        return val;
    }
}
