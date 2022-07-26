using System.Text;

namespace SlimScript;

internal class Equals : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = ReadyParams(line, chunk);

        var first = arg[0] as Bool? ?? new(new("false"));
        var second = arg[1] as Bool? ?? new(new("false"));

        var val = Equals(first, second) || second.Val == first.Val;

        Bool result = new(new(val.ToString().ToLower()));

        return result;
    }
}
