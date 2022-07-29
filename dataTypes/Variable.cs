using System.Collections;
using System.Reflection;

namespace SlimScript;

public class Variable
{
    internal static IVariable Create(Token[] parameters, SourceChunk chunk)
    {
        object realParam = null;

        if (parameters[0].Text == "[")
            return new Array(parameters, chunk);

        for (int i = 0; i < parameters.Length; i++)
        {
            if (realParam != null)
                break;

            Token param = parameters[i];

            if (
                param.Type == TokenType.Number
                || param.Type == TokenType.String
                || param.Type == TokenType.Boolean
                || param.Type == TokenType.Function
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
                case TokenType.Function:
                case TokenType.Identifier:
                    return Identifier.Identify(parameters, chunk);
                case TokenType.Boolean:
                    return new Bool(parameters, chunk);
                default:
                    return new Number(new Token("-1"));
            }
        }
    }

    internal static object? CreateType(string type)
    {
        return (
            Activator
                .CreateInstance(
                    Assembly.GetExecutingAssembly().GetName().Name,
                    $"SlimScript.{type}"
                )
                ?.Unwrap()
        );
    }

    internal static IVariable Copy(IVariable variable, SourceChunk chunk)
    {
        if (variable.GetType() == typeof(Array))
        {
            Array arr = new(variable as Array ?? new Array(), chunk);
            arr.Token = new() { Type = TokenType.Array, Text = "null" };
            arr.Value = variable.Value;

            return arr;
        }

        var returnVar =
            (IVariable?)Activator.CreateInstance(variable.GetType()) ?? new Word(new("null"));

        returnVar.Value = variable.Value;
        returnVar.Name = variable.Name;
        returnVar.Token = variable.Token;

        return returnVar;
    }

    public static IVariable ClrToVar(object clr)
    {
        if (clr.GetType().IsAssignableTo(typeof(IVariable)))
            return (IVariable)clr;
        if (double.TryParse(clr.ToString(), out _))
            return new Number(new($"{clr}"));
        else if (clr.GetType() == typeof(string))
            return new Word(new($"\"{clr}\""));
        else if (clr.GetType().IsAssignableTo(typeof(IEnumerable)))
        {
            Array arr =
                new()
                {
                    Token = new("null") { Type = TokenType.Array },
                    Val = new()
                };

            foreach (var item in (IEnumerable)clr)
                arr.Val.Add(ClrToVar(item));

            return arr;
        }
        else if (clr.GetType() == typeof(bool))
            return new Bool(new($"{clr}".ToLower()));
        else
        {
            //TODO: DotNetInterface
            
            throw new Exception("Type <DotNetInterface> is not supperted yet.");
        }
    }
}
