using System.Data.Common;
using System.Reflection;

namespace SlimScript;

internal class Variable
{
    public static IVariable Create(Token[] parameters, SourceChunk chunk)
    {
        Token? realParam = null;

        for (int i = 0; i < parameters.Length; i++)
        {
            if (realParam != null)
                break;

            Token param = parameters[i];

            if (param.Type == TokenType.Number || param.Type == TokenType.String)
                realParam = param;
            else if (param.Type == TokenType.Identifier)
                realParam = Identifier.Identify(parameters[i..], chunk).Token;
            else if (param.Type == TokenType.Standart)
                realParam = (
                    CreateType(Parser.IdentifyAndGet(parameters[i..].ToList())) as Standart
                )
                    .Run(
                        parameters[i..].ToList(),
                        chunk,
                        Parser.IdentifyRules(parameters[i..].ToList())
                    )
                    .Token;
            else if (param.Type == TokenType.Operator)
                realParam = (
                    CreateType(Parser.IdentifyAndGet(parameters[i..].ToList())) as Operator
                )
                    .Apply(parameters[i..], chunk)
                    .Token;
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"Cannot use token '{param.Text}' as operator parameter. line {Parser.lineNumber}"
                );

                Program.Exit(ExitCode.DisordantTokenError);
            }
        }

        if (realParam == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Cannot define variable from null value. line {Parser.lineNumber}");

            Program.Exit(ExitCode.NullReferenceError);
            return null;
        }

        if (parameters.Length == 1)
            parameters = new[] { realParam }.Cast<Token>().ToArray();

        switch (realParam?.Type)
        {
            case TokenType.Number:
                return new Number(parameters, chunk);
            case TokenType.String:
                return new Word(parameters, chunk);
            case TokenType.Identifier:
                return Identifier.Identify(parameters, chunk);
            default:
                return null;
        }
    }

    public static object? CreateType(string type)
    {
        return (
            Activator
                .CreateInstance(
                    Assembly.GetExecutingAssembly().GetName().Name,
                    $"SlimScript.{type}"
                )
                .Unwrap()
        );
    }
}
