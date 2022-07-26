using System.Text;

namespace SlimScript;

internal class Minus : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<IVariable> realParams = ReadyParams(parameters, chunk);

        IVariable param1;
        IVariable param2;

        if (realParams.Count == 0)
            chunk.Error($"Minus operator cannot take 0 operands.", ExitCode.GrammarError);

        if (realParams.Count == 1)
            (param1, param2) = (new Number(new("0")), realParams[0]);
        else
            (param1, param2) = (realParams[0], realParams[1]);
        
        if (param1.Token.Type != param2.Token.Type)
            chunk.Error($"Cannot use minus operator on types '{param1}' and '{param2}'.", ExitCode.DisordantTokenError);


        if (param1.Token.Type != TokenType.Number)
            chunk.Error($"Minus Operator Does Not Exists on type '{realParams[0]}'", ExitCode.DisordantTokenError);

        return MinusNumbers((Number)param1, (Number)param2);
    }

    private static Number MinusNumbers(Number left, Number right) => left - right;
}
