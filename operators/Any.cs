using System.Text;

namespace SlimScript;

internal class Any : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = ReadyParams(line, chunk);
    
        if (arg.Any(t => t.Token.Type != TokenType.Bool))
        {
            chunk.Error($"Not function does not exists on type '{arg}'.", ExitCode.DisordantTokenError);

            return new Null();
        }

        Bool first = arg[0] as Bool? ?? new Bool(new("false"));
        Bool second = arg[1] as Bool? ?? new Bool(new("false"));

        Bool result = new();
        result.Val = first.Val || second.Val;

        result.Token = new Token(result.Val.ToString().ToLower());

        return result;
    }
}
