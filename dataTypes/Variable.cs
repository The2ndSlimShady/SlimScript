using System.Collections;
using System.Reflection;

namespace SlimScript;

public class Variable
{
    internal static IVariable Create(Token[] parameters, SourceChunk chunk)
    {
        object? realParam = null;

        if (parameters[0].Text.StartsWith('['))
            return new Array(parameters, chunk);

        if (parameters[0].Text.StartsWith('{'))
            return new CLR(parameters, chunk);

        for (int i = 0; i < parameters.Length; i++)
        {
            if (realParam != null)
                break;

            Token param = parameters[i];

            if (
                param.Type == TokenType.Number
                || param.Type == TokenType.Word
                || param.Type == TokenType.Bool
                || param.Type == TokenType.Function
                || param.Type == TokenType.Null
            )
                realParam = param;
            else if (param.Type == TokenType.CLR)
                realParam = CLR.Create(param.Text, parameters[i..], chunk);
            else if (param.Type == TokenType.Identifier)
                realParam = Identifier.Identify(parameters[i..], chunk);
            else if (param.Type == TokenType.Standart)
                realParam = (
                    CreateType(Parser.IdentifyAndGet(parameters[i..].ToList(), chunk)) as Standart
                )?.Run(parameters[i..].ToList(), chunk);
            else if (param.Type == TokenType.Operator)
                realParam = (
                    CreateType(Parser.IdentifyAndGet(parameters[i..].ToList(), chunk)) as Operator
                )?.Apply(parameters[i..], chunk);
            else
            {
                chunk.Error(
                    $"Cannot create variable from '{string.Join(' ', parameters.Select(t => t.Text))} -> where @param: {param.Text}'",
                    ExitCode.DisordantTokenError
                );

                return new Null();
            }
        }

        if (realParam == null)
        {
            chunk.Error($"Cannot create variable from null value.", ExitCode.NullReferenceError);
            return new Null();
        }

        if (parameters.Length == 1 && realParam.GetType() == typeof(Token))
            parameters = new[] { realParam }.Cast<Token>().ToArray();

        if (realParam.GetType().IsAssignableTo(typeof(IVariable)))
            return (IVariable)realParam;
        else
        {
            return ((realParam as Token?)?.Type) switch
            {
                TokenType.Number => new Number(parameters, chunk),
                TokenType.Word => new Word(parameters, chunk),
                TokenType.Function
                or TokenType.Identifier
                    => Identifier.Identify(parameters, chunk),
                TokenType.Bool => new Bool(parameters, chunk),
                _ => new Null(),
            };
        }
    }

    internal static object? CreateType(string type)
    {
        return (
            Activator
                .CreateInstance(
                    Assembly.GetExecutingAssembly().GetName().Name ?? "",
                    $"SlimScript.{type}"
                )
                ?.Unwrap()
        );
    }

    internal static IVariable Copy(IVariable variable)
    {
        if (variable.GetType() == typeof(Array))
        {
            Array arr =
                new(variable as Array ?? new Array())
                {
                    Token = new() { Type = TokenType.Array, Text = "" },
                    Value = variable.Value
                };

            return arr;
        }
        else if (variable.GetType() == typeof(CLR))
        {
            CLR clr = new(variable.Value);
            return clr;
        }

        var returnVar = (IVariable?)Activator.CreateInstance(variable.GetType()) ?? new Null();

        returnVar.Value = variable.Value;
        returnVar.Name = variable.Name;
		returnVar.Type = variable.Type;
        returnVar.Token = new()
        {
            Text = variable.Token.Text,
            // Type = Enum.Parse<TokenType>(variable.GetType().Name)
			Type = variable.Token.Type
        };

        return returnVar;
    }

    public static IVariable ClrToVar(object? clr)
    {		
        if (clr?.GetType() == typeof(CLR))
            return ClrToVar((clr as CLR?)?.Value);
        else if (clr?.GetType().IsAssignableTo(typeof(IVariable)) ?? false)
            return (IVariable)clr;
        else if (clr?.GetType() == typeof(string) || clr?.GetType() == typeof(char))
            return new Word(new($"\"{clr}\""));
        else if (double.TryParse(clr?.ToString() ?? "", out _))
            return new Number(new($"{clr}"));
        else if (clr?.GetType().IsAssignableTo(typeof(IEnumerable)) ?? false)
        {		
            Array arr = new();
				
            foreach (var item in (IEnumerable)clr)
                arr.Val.Add(ClrToVar(item));

            return arr;
        }
        else if (clr?.GetType() == typeof(bool))
            return new Bool(new Token($"{clr}".ToLower()));
        else if (clr == null)
            return new Null();
        else
            return new CLR(clr);
    }

    public static object? VarToClr(IVariable variable)
    {
        if (variable.Token.Type == TokenType.Array)
            return (variable as Array)?.Val.Select(v => v.Value).ToArray();
			//return new System.Collections.ArrayList((variable as Array)?.Val.Select(v => v.Value).ToArray());
        else if (variable.Token.Type == TokenType.Bool)
            return (variable as Bool?)?.Val;
        else if (variable.Token.Type == TokenType.Null)
            return null;
        else if (variable.Token.Type == TokenType.Number)
            return (variable as Number?)?.Val;
        else if (variable.Token.Type == TokenType.Word)
            return (variable as Word?)?.Val;
        else if (variable.Token.Type == TokenType.CLR)
            return (variable as CLR?)?.Val;
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Write.StandartOutput.WriteLine($"Cannot convert type {variable.Token} to clr type.");
            Program.Exit(ExitCode.RuntimeError);

            return null;
        }
    }
}
