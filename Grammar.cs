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
            { ">=", "GreatEqual" },
            { "<=", "LessEqual" },
            { "%", "Modulus" },
            { "not", "Not" },
            { "both", "Both" }
        };

    public static string[] keywords = { "as", "begin", "end", "to" };

    public static string[] standarts = { "define", "function", "set", "do", "write" };
}
