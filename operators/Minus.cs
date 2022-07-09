using System.Text;

namespace SlimScript;

internal class Minus : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<Token> realParams = ReadyParams(parameters, chunk);

        if (realParams[0].Type != realParams[1].Type)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot use minus operator on types '{realParams[0]}' and '{realParams[1]}'. line {Parser.lineNumber}"
            );

            Program.Exit(ExitCode.DisordantTokenError);
            return null;
        }


        if (realParams[0].Type == TokenType.Number)
            return MinusNumbers(realParams[0], realParams[1]);
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Minus operator does not exist on type '{realParams[0]}'. line {Parser.lineNumber}"
            );

            Program.Exit(ExitCode.DisordantTokenError);
            return null;
        }
    }

    private static Number MinusNumbers(Token left, Token right)
    {
        Number l = new(left);
        Number r = new(right);

        return l - r;
    }
}
