namespace SlimScript;

internal class Divide : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<Token> realParams = ReadyParams(parameters, chunk);

        if (realParams[0].Type != realParams[1].Type)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot multiply types '{realParams[0]}' with '{realParams[1]}'. line {Parser.lineNumber}"
            );

            Program.Exit(ExitCode.DisordantTokenError);
            return null;
        }

        if (realParams[0].Type == TokenType.Number)
            return DivideNumbers(realParams[0], realParams[1]);
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Plus operator does not exist on type '{realParams[0]}'. line {Parser.lineNumber}"
            );

            Program.Exit(ExitCode.DisordantTokenError);
            return null;
        }
    }

    private static Number DivideNumbers(Token left, Token right)
    {
        Number l = new(left);
        Number r = new(right);

        Number val = new();
        val.Val = l.Val / r.Val;
        val.Token = new(val.Val.ToString());

        return val;
    }
}
