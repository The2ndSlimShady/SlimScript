using System.Text;

namespace SlimScript;

internal class LesserThan : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = ReadyParams(line, chunk);

        if (arg[0].Type != arg[1].Type || arg.Any(t => t.Type != TokenType.Number))
        {
            chunk.Error(
                $"Cannot compare non-numeric types '{arg[0]}' and '{arg[1]}'.",
                ExitCode.DisordantTokenError
            );

            return null;
        }

        Number first = new(arg[0]);
        Number second = new(arg[1]);

        Bool result = new();
        result.Val = first.Val < second.Val;
        result.Token = new Token(result.Val.ToString().ToLower());

        return result;
    }
}
