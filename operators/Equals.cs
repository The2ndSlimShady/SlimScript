using System.Text;

namespace SlimScript;

internal class Equals : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = ReadyParams(line, chunk);

        var first = Variable.Create(new[]{arg[0]}, chunk).Value;
        var second = Variable.Create(new[]{arg[1]}, chunk).Value;

        var val = Equals(first, second) || second == first;

        Bool result = new(new(val.ToString().ToLower()));

        return result;
    }
}
