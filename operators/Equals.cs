using System.Text;

namespace SlimScript;

internal class Equals : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = ReadyParams(line, chunk);

        var first = arg[0];
        var second = arg[1];

        var val = first.GetString() == second.GetString();

        Bool result = new(new(val.ToString().ToLower()));

        return result;
    }
}
