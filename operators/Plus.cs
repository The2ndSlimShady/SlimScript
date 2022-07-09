using System.Text;

namespace SlimScript;

internal class Plus : Operator
{
    public override IVariable Apply(Token[] parameters, SourceChunk chunk)
    {
        List<Token> realParams = new(2);

        for (int i = 1; i < parameters.Length; i++)
        {
            if (realParams.Count == 2)
                break;

            Token param = parameters[i];

            if (param.Type == TokenType.Number || param.Type == TokenType.String)
                realParams.Add(param);
            else if (param.Type == TokenType.Identifier)
                realParams.Add(Identifier.Identify(parameters[i..], chunk).Token);
            else if (param.Type == TokenType.Standart)
                realParams.Add(
                    (
                        Variable.CreateType(Parser.IdentifyAndGet(parameters[i..].ToList()))
                        as Standart
                    )
                        .Run(
                            parameters[i..].ToList(),
                            chunk,
                            Parser.IdentifyRules(parameters[i..].ToList())
                        )
                        .Token
                );
            else if (param.Type == TokenType.Operator)
                realParams.Add(
                    (
                        Variable.CreateType(Parser.IdentifyAndGet(parameters[i..].ToList()))
                        as Operator
                    )
                        .Apply(parameters[i..], chunk)
                        .Token
                );
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"Cannot use token '{param.Text}' as operator parameter. line {Parser.lineNumber}"
                );

                Program.Exit(ExitCode.DisordantTokenError);
            }
        }

        if (realParams[0].Type != realParams[1].Type)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot sum up types '{realParams[0]}' with '{realParams[1]}'. line {Parser.lineNumber}"
            );

            Program.Exit(ExitCode.DisordantTokenError);
            return null;
        }
        else
        {
            if (realParams[0].Type == TokenType.Number)
                return SumNumbers(realParams[0], realParams[1]);
            else if (realParams[0].Type == TokenType.String)
                return SumStrings(realParams[0], realParams[1]);
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
    }

    private static Number SumNumbers(Token left, Token right)
    {
        Number l = new (left);
        Number r = new (right);

        return l + r;
    }

    private static Word SumStrings(Token left, Token right)
    {
        Word l = new (left);
        Word r = new (right);

        return l + r;
    }
}