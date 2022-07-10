using System.Data.Common;
using System.Reflection;

namespace SlimScript;

internal class Variable
{
    public static IVariable Create(Token[] parameters, SourceChunk chunk)
    {
        object realParam = null;

        for (int i = 0; i < parameters.Length; i++)
        {
            if (realParam != null)
                break;

            Token param = parameters[i];

            if (
                param.Type == TokenType.Number
                || param.Type == TokenType.String
                || param.Type == TokenType.Boolean
            )
                realParam = param;
            else if (param.Type == TokenType.Identifier)
                realParam = Identifier.Identify(parameters[i..], chunk);
            else if (param.Type == TokenType.Standart)
                realParam = (
                    CreateType(Parser.IdentifyAndGet(parameters[i..].ToList(), chunk)) as Standart
                ).Run(parameters[i..].ToList(), chunk);
            else if (param.Type == TokenType.Operator)
                realParam = (
                    CreateType(Parser.IdentifyAndGet(parameters[i..].ToList(), chunk)) as Operator
                ).Apply(parameters[i..], chunk);
            else
            {
                chunk.Error(
                    $"Cannot use token '{param.Text}' as operator parameter.",
                    ExitCode.DisordantTokenError
                );

                return new Number(new Token("-1"));
            }
        }

        if (realParam == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot define variable from null value. line {chunk.Parser.lineNumber}"
            );

            Program.Exit(ExitCode.NullReferenceError);
            return new Number(new Token("-1"));
        }

        if (parameters.Length == 1 && realParam.GetType() == typeof(Token))
            parameters = new[] { realParam }.Cast<Token>().ToArray();

        if (realParam.GetType().IsAssignableTo(typeof(IVariable)))
            return (IVariable)realParam;
        else
        {
            switch ((realParam as Token?)?.Type)
            {
                case TokenType.Number:
                    return new Number(parameters, chunk);
                case TokenType.String:
                    return new Word(parameters, chunk);
                case TokenType.Identifier:
                    return Identifier.Identify(parameters, chunk);
                case TokenType.Boolean:
                    return new Bool(parameters, chunk);
                default:
                    return new Number(new Token("-1"));
            }
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

    public static IVariable Copy(IVariable variable)
    {
        var returnVar = (IVariable)Activator.CreateInstance(variable.GetType());

        returnVar.Value = variable.Value;
        returnVar.Name = variable.Name;
        returnVar.Token = variable.Token;

        return returnVar;
    }
}
