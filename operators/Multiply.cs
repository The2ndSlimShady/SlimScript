using System.Text;
using System.Collections;

namespace SlimScript;

internal class Multiply : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<IVariable> realParams = ReadyParams(parameters, chunk);

        if (
            realParams.Any(
                t => t.Token.Type != TokenType.Number && t.Token.Type != TokenType.Word
            )
        )
        {
            chunk.Error(
                $"Cannot multiply types '{realParams[0]}' with '{realParams[1]}'",
                ExitCode.DisordantTokenError
            );
            return new Null();
        }

        if (realParams[0].Token.Type == TokenType.Number)
        {
            if (realParams[1].Token.Type == TokenType.Number)
                return MultiplyNumbers((Number)realParams[0], (Number)realParams[1]);
            else
                return MultiplyStrings((Word)realParams[1], (Number)realParams[0]);
        }
        else
        {
            if (realParams[1].Token.Type == TokenType.Number)
                return MultiplyStrings((Word)realParams[0], (Number)realParams[1]);
            else
            {
                chunk.Error(
                    $"Multiply Operator Does Not Exists on type '{realParams[0]}'",
                    ExitCode.DisordantTokenError
                );
                return new Null();
            }
        }
    }

    private static Number MultiplyNumbers(Number l, Number r)
    {
        Number val = new();
        val.Val = l.Val * r.Val;
        val.Token = new(val.Val.ToString());

        return val;
    }

    private static Word MultiplyStrings(Word word, Number num)
    {
        Word val = new();
        Token token =
            new($"\"{string.Concat(Enumerable.Repeat(word.Value, (int)Math.Floor(num.Val)))}\"");
        val.Val = token.Text;
        val.Token = token;

        return val;
    }
}
