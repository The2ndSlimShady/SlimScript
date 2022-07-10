namespace SlimScript;

internal class Grammar
{
    public static Dictionary<string, string> operators =
        new()
        {
            { "+", "Plus" },
            { "-", "Minus" },
            { "*", "Multiply" },
            { "/", "Divide" },
            { "=", "Equals" },
            { "!=", "NotEquals" },
            { "<", "LesserThan" },
            { ">", "GreaterThan" },
            { ">=", "GreaterEqual" },
            { "<=", "LesserEqual" },
            { "%", "Modulus" },
            { "not", "Not" },
            { "both", "Both" },
            { "any", "Any" }
        };

    public static string[] keywords = { "as", "begin", "end", "to" };

    public static string[] standarts = { "define", "function", "set", "do", "write", "delete" };
}
