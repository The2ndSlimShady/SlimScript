using System.Text;

namespace SlimScript;

internal class Both : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = ReadyParams(line, chunk);

        if (arg.Any(t => t.Token.Type != TokenType.Bool))
        {
            chunk.Error(
                $"Both function does not exists on type '{arg}'.",
                ExitCode.DisordantTokenError
            );

            return new Null();
        }

        Bool first = (Bool)arg[0];
        Bool second = (Bool)arg[1];

        Bool result = new();
        result.Val = first.Val && second.Val;

        result.Token = new Token(result.Val.ToString().ToLower());

        return result;
    }
}
