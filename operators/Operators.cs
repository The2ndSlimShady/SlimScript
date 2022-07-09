namespace SlimScript;

internal abstract class Operator
{
    public abstract IVariable Apply(Token[] parameters, SourceChunk chunk);

    public static List<Token> ReadyParams(Token[] parameters, SourceChunk chunk, int capacity = 2)
    {
        List<Token> realParams;

        if (capacity != 0)
            realParams = new(capacity);
        else
            realParams = new();

        for (int i = 1; i < parameters.Length; i++)
        {
            if (realParams.Capacity != 0 && realParams.Count == realParams.Capacity)
                break;

            Token param = parameters[i];

            if (param.Type == TokenType.Number || param.Type == TokenType.String)
                realParams.Add(param);
            else if (param.Type == TokenType.Identifier)
                realParams.Add(Identifier.Identify(parameters[i..], chunk).Token);
            else if (param.Type == TokenType.Standart)
            {
                realParams.Add(
                    (
                        Variable.CreateType(Parser.IdentifyAndGet(parameters[i..].ToList()))
                        as Standart
                    )
                        .Run(parameters[i..].ToList(), chunk)
                        .Token
                );

                i = parameters.Length;
            }
            else if (param.Type == TokenType.Operator)
            {
                realParams.Add(
                    (
                        Variable.CreateType(Parser.IdentifyAndGet(parameters[i..].ToList()))
                        as Operator
                    )
                        .Apply(parameters[i..], chunk)
                        .Token
                );

                i = parameters.Length;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"Cannot use token '{param.Text}' as operator parameter. line {Parser.lineNumber}"
                );

                Program.Exit(ExitCode.DisordantTokenError);
            }
        }

        return realParams;
    }

    public static bool IsTheSame(List<Token> realParams)
    {
        if (realParams[0].Type != realParams[1].Type)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot sum up types '{realParams[0]}' with '{realParams[1]}'. line {Parser.lineNumber}"
            );

            Program.Exit(ExitCode.DisordantTokenError);
            return false;
        }

        return true;
    }
}
