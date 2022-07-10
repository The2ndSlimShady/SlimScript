using System.Text;
using System.Collections;

namespace SlimScript;

internal class Multiply : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<Token> realParams = ReadyParams(parameters, chunk);

        if (realParams.Any(t => t.Type != TokenType.Number && t.Type != TokenType.String))
        {
            chunk.Error(
                $"Cannot multiply types '{realParams[0]}' with '{realParams[1]}'",
                ExitCode.DisordantTokenError
            );
            return null;
        }

        if (realParams[0].Type == TokenType.Number)
        {
            if (realParams[1].Type == TokenType.Number)
                return MultiplyNumbers(realParams[0], realParams[1]);
            else
                return MultiplyStrings(realParams[1], realParams[0]);
        }
        else
        {
            if (realParams[1].Type == TokenType.Number)
                return MultiplyStrings(realParams[0], realParams[1]);
            else
            {
                chunk.Error(
                    $"Multiply Operator Does Not Exists on type '{realParams[0]}'",
                    ExitCode.DisordantTokenError
                );
                return null;
            }
        }
    }

    private static Number MultiplyNumbers(Token left, Token right)
    {
        Number l = new(left);
        Number r = new(right);

        Number val = new();
        val.Val = l.Val * r.Val;
        val.Token = new(val.Val.ToString());

        return val;
    }

    private static Word MultiplyStrings(Token str, Token times)
    {
        Word word = new(str);
        Number num = new(times);

        Word val = new();
        Token token =
            new($"\"{string.Concat(Enumerable.Repeat(word.Value, (int)Math.Floor(num.Val)))}\"");
        val.Val = token.Text;
        val.Token = token;

        return val;
    }
}
