namespace SlimScript;

internal class Grammar
{
    public static Dictionary<string, string> operators =
        new()
        {
            { "+", "Plus" },
            { "-", "Minus" },
            { "*", "Times" },
            { "/", "Divide" },
            { "=", "Equals" },
            { "!=", "NotEquals" },
            { "<", "LesserThan" },
            { ">", "GreaterThan" },
            { ">=", "GreatEqual" },
            { "<=", "LessEqual" },
            { "%", "Modulus" }
        };

    public static string[] keywords = { "as", "number", "begin", "end", "to" };

    public static string[] standarts = { "define", "function", "set", "not", "do", "write" };

    public static string[] rules =
    {
        "define <Identifier> <Keyword>",

        "function <Identifier> <Keyword>",

        "set <Identifier> <Keyword>",

        "write <String>",
        "write <Number>",
        "write <Identifier>",
        "write <Operator>",
        "write <Standart>",

        "+ <Number>",
        "+ <String>",
        "+ <Operator>",
        "+ <Standart>",
        "* <Number>",
        "* <Number>",
        "* <String>"
    };
}
