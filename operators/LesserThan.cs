using System.Text;

namespace SlimScript;

internal class LesserThan : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = ReadyParams(line, chunk);

        if (arg[0].Token.Type != arg[1].Token.Type || arg.Any(t => t.Token.Type != TokenType.Number))
        {
            chunk.Error(
                $"Cannot compare non-numeric types '{arg[0]}' and '{arg[1]}'.",
                ExitCode.DisordantTokenError
            );

            return new Null();
        }

        Number first = (Number)arg[0];
        Number second = (Number)arg[1];

        Bool result = new();
        result.Val = first.Val < second.Val;
        result.Token = new Token(result.Val.ToString().ToLower());

        return result;
    }
}
