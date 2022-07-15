using System.Text;

namespace SlimScript;

internal class Plus : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<Token> realParams = ReadyParams(parameters, chunk);

        Token param1;
        Token param2;

        if (realParams.Count == 0)
            chunk.Error($"Plus operator cannot take 0 operands.", ExitCode.GrammarError);

        if (realParams.Count == 1)
            (param1, param2) = (
                realParams[0],
                new()
                {
                    Type = realParams[0].Type,
                    Text = realParams[0].Type == TokenType.Number ? "0" : ""
                }
            );
        else
            (param1, param2) = (realParams[0], realParams[1]);

        if (param1.Type != param2.Type)
            chunk.Error(
                $"Cannot use plus operator on types '{realParams[0]}' and '{realParams[1]}'.",
                ExitCode.DisordantTokenError
            );

        if (param1.Type == TokenType.Number)
            return SumNumbers(param1, param2);
        else if (realParams[0].Type == TokenType.String)
            return SumStrings(param1, param2);
        else
        {
            chunk.Error(
                $"Minus Operator Does Not Exists on type '{realParams[0]}'",
                ExitCode.DisordantTokenError
            );
            return new Word(new("null"));
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
