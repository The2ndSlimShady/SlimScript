using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections;

namespace SlimScript;

public struct CLR : IVariable
{
    private string _name = "";

    public Token Token { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            Token = new(_name);
        }
    }

    internal object? Val { get; set; } = null;

    public object Value
    {
        get => Val ?? new Null();
        set => Val = value;
    }

    public CLRType CLRType { get; set; } = CLRType.Null;

    /// <summary>
    /// To create a Type object for creating CLR instances
    /// </summary>
    /// <param name="tokens"></param>
    /// <param name="chunk"></param>
    internal CLR(Token[] tokens, SourceChunk chunk)
    {
        Token = new() { Type = TokenType.CLR };

        CLRType = CLRType.Type;

        Token typeToken = tokens[0];

        string str = typeToken.Text[1..^1];

        StringBuilder typeName = new();

        foreach (var item in str.Split("::"))
            typeName.Append($".{item}");

        if (typeName.Length == 0)
        {
            CLRType = CLRType.Null;
            return;
        }

        str = typeName.Remove(0, 1).ToString();

        Val = Type.GetType(str);

        if (Val == null)
            chunk.Error($"Cannot create CLR RuntimeType from '{str}'.", ExitCode.RuntimeError);
    }

    /// <summary>
    /// To make operations on CLR or Type variable
    /// </summary>
    /// <param name="value"></param>
    /// <param name="chunk"></param>
    internal CLR(string value, Token[] parameters, SourceChunk chunk)
    {
        string[] seperators = new string[2] { ".", "->" };

        List<string> expressions = new() { "" };

        for (int i = 0; i < value.Length; i++)
        {
            if (seperators.Contains(value[i..(i + 1)]))
            {
                expressions.Add(value[i..(i + 1)]);
                expressions.Add("");
            }
            else if ((i + 1) != value.Length && seperators.Contains(value[i..(i + 2)]))
            {
                expressions.Add(value[i..(i + 2)]);
                i++;
                expressions.Add("");
            }
            else
                expressions[^1] += value[i];
        }

        object? theVal = chunk.VarExists(expressions[0])
            ? chunk.GetVar(expressions[0])?.Value
            : null;

        for (int i = 0; i < expressions.Count; i++)
        {
            if (expressions[i + 1] == "->")
            {
                object?[]? invokeParams = Operator
                    .ReadyParams(parameters, chunk, 0)
                    .Select(var => Variable.VarToClr(var))
                    .ToArray();

                var paramTypes = invokeParams.Select(p => p.GetType()).ToArray();

                MethodBase? method = null;

                if (expressions[i + 2] != "new")
                {
                    if (theVal?.GetType() != typeof(string).GetType())
                        method = theVal?.GetType().GetRuntimeMethod(expressions[i + 2], paramTypes);
                    else
                        method = (theVal as Type)?.GetRuntimeMethod(expressions[i + 2], paramTypes);
                }

                else
                    method = (theVal as Type)
                        ?.GetConstructors()
                        .Single(ctor =>
                        {
                            bool allOk = false;
                            var ctorParams = ctor.GetParameters();

                            if (ctorParams.Length == 0 && paramTypes.Length == 0)
                                return true;

                            for (int i = 0; i < ctorParams.Length; i++)
                            {
                                int index = Math.Min(i, paramTypes.Length - 1);

                                var param = ctorParams[i].ParameterType;

                                if (index < 0)
                                    return false;

                                var paramType = paramTypes[i];

                                allOk = paramType == param;
                            }

                            return allOk;
                        });

                if (method?.IsConstructor ?? false)
                    theVal = Activator.CreateInstance(theVal as Type, invokeParams);
                else if ((method as MethodInfo)?.ReturnType == typeof(void))
                    method?.Invoke(theVal, invokeParams);
                else
                    theVal = method?.Invoke(theVal, invokeParams);

                i += 2;
            }
            else if (expressions[i + 1] == ".")
            {
                object? tmpVal;

                if (theVal?.GetType() == typeof(string).GetType())
                    tmpVal = (theVal as Type)
                        ?.GetRuntimeProperty(expressions[i + 2])
                        ?.GetValue(theVal);
                else
                    tmpVal = theVal
                        ?.GetType()
                        .GetRuntimeProperty(expressions[i + 2])
                        ?.GetValue(theVal);

                if (tmpVal != null)
                    theVal = tmpVal;
                else
                {
                    if (theVal?.GetType() == typeof(string).GetType())
                        theVal = (theVal as Type)
                            ?.GetRuntimeField(expressions[i + 2])
                            ?.GetValue(theVal);
                    else
                        theVal = theVal
                            ?.GetType()
                            .GetRuntimeField(expressions[i + 2])
                            ?.GetValue(theVal);
                }

                i += 2;
            }

            if (i == expressions.Count - 3)
                break;
        }

        this = new(theVal);
    }

    /// <summary>
    /// To create CLR value from  object directly
    /// </summary>
    /// <param name="value"></param>
    public CLR(object? value)
    {
        Token = new() { Type = TokenType.CLR };
        Val = value;

        if (Val?.GetType() == typeof(string).GetType())
            CLRType = CLRType.Type;
        else if (Val == null)
            CLRType = CLRType.Null;
        else
            CLRType = CLRType.Object;
    }

    public string GetString()
    {
        if (Val == null)
            return "null";
        else if (CLRType == CLRType.Object)
            return Val.ToString() ?? "null";
        else if (CLRType == CLRType.Type)
            return ((Type?)Val)?.FullName ?? "null";
        else
            return "null";
    }

    public override string ToString() => GetString();
}

public enum CLRType
{
    Null,
    Type,
    Object
}
