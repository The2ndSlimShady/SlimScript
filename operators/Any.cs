using System.Text;

namespace SlimScript;

internal class Any : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = ReadyParams(line, chunk);
    
        if (arg.Any(t => t.Type != TokenType.Boolean))
        {
            chunk.Error($"Not function does not exists on type '{arg}'.", ExitCode.DisordantTokenError);

            return null;
        }

        Bool first = new(arg[0]);
        Bool second = new(arg[1]);

        Bool result = new();
        result.Val = first.Val || second.Val;

        result.Token = new Token(result.Val.ToString().ToLower());

        return result;
    }
}
