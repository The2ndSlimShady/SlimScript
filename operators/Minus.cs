using System.Text;

namespace SlimScript;

internal class Minus : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<Token> realParams = ReadyParams(parameters, chunk);

        Token param1;
        Token param2;

        if (realParams.Count == 0)
            chunk.Error($"Minus operator cannot take 0 operands.", ExitCode.GrammarError);

        if (realParams.Count == 1)
            (param1, param2) = (new("0"), realParams[0]);
        else
            (param1, param2) = (realParams[0], realParams[1]);
        
        if (param1.Type != param2.Type)
            chunk.Error($"Cannot use minus operator on types '{param1}' and '{param2}'.", ExitCode.DisordantTokenError);


        if (param1.Type != TokenType.Number)
            chunk.Error($"Minus Operator Does Not Exists on type '{realParams[0]}'", ExitCode.DisordantTokenError);

        return MinusNumbers(param1, param2);
    }

    private static Number MinusNumbers(Token left, Token right)
    {
        Number l = new(left);
        Number r = new(right);

        return l - r;
    }
}
