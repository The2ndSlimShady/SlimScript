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
            { "not", "Not" },
            { "both", "Both" },
            { "any", "Any" }
        };

    public static string[] keywords = { "as", "begin", "end", "to", "then", "in" };

    public static string[] standarts =
    {
        "define",
        "func",
        "set",
        "do",
        "write",
        "delete",
        "return",
        "if",
        "elif",
        "else",
        "while",
        "get",
        "foreach",
        "index"
    };
}
