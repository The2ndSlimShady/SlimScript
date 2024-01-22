using System.Text;

namespace SlimScript;

internal class Plus : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<IVariable> realParams = ReadyParams(parameters, chunk);

        IVariable param1;
        IVariable param2;

        if (realParams.Count == 0)
            chunk.Error($"Plus operator cannot take 0 operands.", ExitCode.GrammarError);

        if (realParams.Count == 1)
            (param1, param2) = (
                realParams[0],
                realParams[0].Token.Type == TokenType.Number ? new Number(new("0")) : new Word(new(""))
            );
        else
            (param1, param2) = (realParams[0], realParams[1]);

        if (param1.Token.Type != param2.Token.Type)
            chunk.Error(
                $"Cannot use plus operator on types '{realParams[0].Type}' and '{realParams[1].Type}'.",
                ExitCode.DisordantTokenError
            );

        if (param1.Token.Type == TokenType.Number)
            return SumNumbers((Number)param1, (Number)param2);
        else if (realParams[0].Token.Type == TokenType.Word)
            return SumStrings((Word)param1, (Word)param2);
        else
        {
            chunk.Error(
                $"Minus Operator Does Not Exists on type '{realParams[0]}'",
                ExitCode.DisordantTokenError
            );
            return new Null();
        }
    }

    private static Number SumNumbers(Number l, Number r) => l + r;

    private static Word SumStrings(Word l, Word r) => l + r;
}
