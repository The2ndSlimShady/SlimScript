namespace SlimScript;

internal class Grammar
{
    public static readonly Dictionary<string, string> operators =
        new()
        {
            { "+", "Plus" },
            { "-", "Minus" },
            { "*", "Multiply" },
            { "^", "Power" },
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

    public static readonly string[] keywords = { "as", "begin", "end", "to", "then", "in", "of" };

    public static readonly string[] standarts =
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
        "foreach",
        "index",
        "append",
        "for",
        "error",
        "typeof",
        "tostring",
        "tonumber",
        "tobool",
        "clrname",
        "input",
        "break",
        "import"
    };

    public static readonly string[] escape =
    {
        "`n", Environment.NewLine,
        "`b", "\b",
        "`t", "\t",
		"`q", "\""
    };
}
