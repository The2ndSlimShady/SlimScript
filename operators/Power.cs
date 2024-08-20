using System.Text;
using System.Collections;

namespace SlimScript;

internal class Power : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<IVariable> realParams = ReadyParams(parameters, chunk);

        if (realParams.Any(t => t.Token.Type != TokenType.Number))
        {
            chunk.Error(
                $"Cannot multiply types '{realParams[0]}' with '{realParams[1]}'",
                ExitCode.DisordantTokenError
            );
            return new Null();
        }

        return PowerNumbers((Number)realParams[0], (Number)realParams[1]);
    }

    private static Number PowerNumbers(Number l, Number r)
    {
        Number val = new();
        val.Val = Math.Pow(l.Val, r.Val);
        val.Token = new(val.Val.ToString());

        return val;
    }
}
