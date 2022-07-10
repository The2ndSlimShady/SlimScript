using System.Text;

namespace SlimScript;

internal class Plus : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<Token> realParams = ReadyParams(parameters, chunk);

        if (realParams[0].Type != realParams[1].Type)
        {
            chunk.Error(
                $"Cannot use minus operator on types '{realParams[0]}' and '{realParams[1]}'.",
                ExitCode.DisordantTokenError
            );
            return null;
        }

        if (realParams[0].Type == TokenType.Number)
            return SumNumbers(realParams[0], realParams[1]);
        else if (realParams[0].Type == TokenType.String)
            return SumStrings(realParams[0], realParams[1]);
        else
        {
            chunk.Error(
                $"Minus Operator Does Not Exists on type '{realParams[0]}'",
                ExitCode.DisordantTokenError
            );
            return null;
        }
    }

    private static Number SumNumbers(Token left, Token right)
    {
        Number l = new(left);
        Number r = new(right);

        return l + r;
    }

    private static Word SumStrings(Token left, Token right)
    {
        Word l = new(left);
        Word r = new(right);

        return l + r;
    }
}
