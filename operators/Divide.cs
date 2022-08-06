namespace SlimScript;

internal class Divide : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<IVariable> realParams = ReadyParams(parameters, chunk);

        if (realParams[0].Token.Type != realParams[1].Token.Type)
        {
            chunk.Error(
                $"Cannot multiply types '{realParams[0]}' with '{realParams[1]}'.",
                ExitCode.DisordantTokenError
            );
            return new Null();
        }

        if (realParams[0].Token.Type == TokenType.Number)
            return DivideNumbers((Number)realParams[0], (Number)realParams[1]);
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Write.StandartOutput.WriteLine(
                $"Plus operator does not exist on type '{realParams[0]}'. line {chunk.Parser.lineNumber}"
            );

            Program.Exit(ExitCode.DisordantTokenError);
            return new Null();
        }
    }

    private static Number DivideNumbers(Number left, Number right)
    {
        Number val = new();
        val.Val = left.Val / right.Val;
        val.Token = new(val.Val.ToString());

        return val;
    }
}
