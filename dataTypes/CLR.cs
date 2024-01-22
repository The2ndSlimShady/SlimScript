using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections;

namespace SlimScript;

public struct CLR : IVariable
{
    private string _name = "";

    public Token Token { get; set; }

    public TokenType Type { get; set; } = TokenType.CLR;

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

        Val = System.Type.GetType(str);

        if (Val == null)
            chunk.Error($"Cannot create CLR RuntimeType from '{str}'.", ExitCode.RuntimeError);
    }

    /// <summary>
    /// To make operations on CLR or Type variable
    /// </summary>
    /// <param name="value"></param>
    /// <param name="chunk"></param>
    internal static IVariable Create(string value, Token[] parameters, SourceChunk chunk)
    {
        string[] seperators = new string[2] { ":", "->" };

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

        for (int i = 0; i <= expressions.Count - 3; i++)
        {
            if (expressions[i + 1] == "->")
            {
                object?[]? invokeParams = Operator
                    .ReadyParams(parameters, chunk, 0)
                    .Select(var => Variable.VarToClr(var))
                    .ToArray();

                var paramTypes = invokeParams.Select(p => p?.GetType() ?? typeof(Null)).ToArray();

                List<Type> genericParams = new();

                MethodBase? method = null;

                bool selectMethod(MethodBase method)
                {
                    bool allOk = false;
                    var methodParams = method.GetParameters();

                    if (methodParams.Length == 0 && paramTypes.Length == 0)
                        return true;

                    if (methodParams.Length != paramTypes.Length)
                        return false;

                    for (int i = 0; i < methodParams.Length; i++)
                    {
                        int index = Math.Min(i, paramTypes.Length - 1);

                        var param = methodParams[i].ParameterType;

                        if (index < 0)
                            return false;

                        var paramType = paramTypes[index];

                        allOk = paramType == param || paramType.IsAssignableTo(param);

                        if (!allOk)
                        {
                            try
                            {
                                allOk = Variable.ClrToVar(paramType).GetType() == param;

                                object? temp;

                                if (allOk)
                                    temp = Variable.ClrToVar(paramType);
                                else
                                {
                                    if (invokeParams[index]?.GetType().IsEnum ?? false)
                                        temp = invokeParams[index];
                                    else
                                        temp = Convert.ChangeType(invokeParams[index], param);
                                }

                                allOk = temp?.GetType() == param;

                                if (allOk)
                                {
                                    paramType = temp?.GetType();
                                    invokeParams[index] = temp;
                                }
                            }
                            catch (Exception)
                            {
                                allOk = false;
                            }
                        }
                        else if (
                            invokeParams[index]
                                ?.GetType()
                                .GetInterfaces()
                                .Contains(typeof(IConvertible))
                            ?? false
                                && !param.IsGenericParameter
                                && allOk
                                && !((Type)invokeParams[index]).IsEnum
                        )
                            invokeParams[index] = Convert.ChangeType(invokeParams[index], param);
                        else if (param.IsGenericParameter)
                            genericParams.Add(paramType);
                    }

                    return allOk;
                }

                if (expressions[i + 2] != "new")
                {
                    if (theVal?.GetType() != typeof(string).GetType())
                        method = theVal?.GetType().GetRuntimeMethod(expressions[i + 2], paramTypes);
                    else
                        method = (theVal as Type)?.GetRuntimeMethod(expressions[i + 2], paramTypes);

                    if (method == null)
                    {
                        if (theVal?.GetType() != typeof(string).GetType())
                            method = theVal
                                ?.GetType()
                                .GetRuntimeMethods()
                                .Where(m => m.Name == expressions[i + 2] && selectMethod(m))
                                .Last();
                        else
                            method = (theVal as Type)
                                ?.GetRuntimeMethods()
                                .Where(m => m.Name == expressions[i + 2] && selectMethod(m))
                                .Last();
                    }
                }
                else
                {
                    method = (theVal as Type)?.GetConstructor(paramTypes);

                    if (method == null)
                        method = (theVal as Type)
                            ?.GetConstructors()
                            .Where(ctor => selectMethod(ctor))
                            .Last();
                }

                if (method?.IsGenericMethod ?? false)
                    method = (method as MethodInfo)?.MakeGenericMethod(genericParams.ToArray());

                if (method?.IsConstructor ?? false)
                    theVal = Activator.CreateInstance(theVal as Type ?? typeof(Null), invokeParams);
                else if ((method as MethodInfo)?.ReturnType == typeof(void))
                    method?.Invoke(theVal, invokeParams);
                else
                    theVal = method?.Invoke(theVal, invokeParams);

                i++;
            }
            else if (expressions[i + 1] == ":")
            {
                if (expressions[i + 2] == "thisGet" || expressions[i + 2] == "thisSet")
                {
                    object?[]? invokeParams = Operator
                        .ReadyParams(parameters, chunk, 0)
                        .Select(var => Variable.VarToClr(var))
                        .ToArray();

                    var val = invokeParams.Length != 1 ? invokeParams[1] : null;

                    Type? tmpVal = null;

                    if (theVal?.GetType() == typeof(string).GetType())
                        tmpVal = theVal as Type;
                    else
                        tmpVal = theVal?.GetType();

                    var indexer = tmpVal
                        ?.GetProperties()
                        .Single(p => p.GetIndexParameters().Length != 0);

                    var index = Convert.ChangeType(invokeParams[0], indexer?.GetIndexParameters()[0].ParameterType ?? typeof(int));

                    if (expressions[i + 2] == "thisGet")
                        theVal = indexer?.GetValue(theVal, new[] { index });
                    else
                        indexer?.SetValue(theVal, val, new[] { index });
                }
                else
                {
                    if (parameters.Length == 1)
                    {
                        object? tmpVal = null;

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
                    }
                    else
                    {
                        object? tmpProp = null;

                        if (theVal?.GetType() == typeof(string).GetType())
                            tmpProp = (theVal as Type)?.GetRuntimeProperty(expressions[i + 2]);
                        else
                            tmpProp = theVal?.GetType().GetRuntimeProperty(expressions[i + 2]);

						

                        if (tmpProp != null)
                        {
							var newVal = Variable.VarToClr(Variable.Create(parameters[1..], chunk));
							newVal = Convert.ChangeType(newVal, (tmpProp as PropertyInfo)?.PropertyType);
							(tmpProp as PropertyInfo)?.SetValue(theVal, newVal);
						}
                        else
                        {
                            if (theVal?.GetType() == typeof(string).GetType())
                                tmpProp = (theVal as Type)?.GetRuntimeField(expressions[i + 2]);
                            else
                                tmpProp = theVal?.GetType().GetRuntimeField(expressions[i + 2]);

                            (tmpProp as FieldInfo)?.SetValue(
                                theVal,
                                Variable.VarToClr(Variable.Create(parameters[1..], chunk))
                            );
                        }
                    }
                }

                i++;
            }
        }

        if (theVal?.GetType().IsAssignableTo(typeof(IVariable)) ?? false)
            return Variable.ClrToVar((theVal as IVariable)?.Value);

        return new CLR(theVal);
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
        if (CLRType == CLRType.Object)
            return Value.ToString() ?? "";
        else if (CLRType == CLRType.Type)
            return ((Type?)Value)?.FullName ?? "";
        else
            return "null";
    }

    public string GetTypeString()
    {
        if (CLRType == CLRType.Type)
            return $"CLR : {(Val as Type)?.FullName}";
        else
            return $"CLR : {Val?.GetType().FullName}";
    }

    public override string ToString() => GetString();
}

public enum CLRType
{
    Null,
    Type,
    Object
}
