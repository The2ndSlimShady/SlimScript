using System.Text;

namespace SlimScript;

internal class Both : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = ReadyParams(line, chunk);
    
        if (arg.Any(t => t.Token.Type != TokenType.Boolean))
        {
            chunk.Error($"Both function does not exists on type '{arg}'.", ExitCode.DisordantTokenError);

            return null;
        }

        Bool first = arg[0] as Bool? ?? new(new("false"));
        Bool second = arg[1] as Bool? ?? new(new("false"));

        Bool result = new();
        result.Val = first.Val && second.Val;

        result.Token = new Token(result.Val.ToString().ToLower());

        return result;
    }
}
