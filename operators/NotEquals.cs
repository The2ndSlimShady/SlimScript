using System.Text;

namespace SlimScript;

internal class NotEquals : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        Bool val = (Bool)new Equals().Apply(line, chunk);

        val.Val = !val.Val;
        val.Token = new Token(val.Val.ToString().ToLower());

        return val;
    }
}
