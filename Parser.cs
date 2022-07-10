namespace SlimScript;

internal class Parser
{
    public static int lineNumber = 0;

    public static void Parse(SourceChunk chunk, List<List<Token>> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            List<Token>? line = lines[i];
            ParseLine(chunk, line);
        }
    }

    public static void ParseLine(SourceChunk chunk, List<Token> line)
    {
        lineNumber++;

        string? rule = null;
        object? obj = null;

        try
        {
            rule = IdentifyAndGet(line);
            obj = Variable.CreateType(rule);
        }
        catch (Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Cannot execute command '{rule?.ToLower()}'. Specified command not found. line {lineNumber}");
            
            Program.Exit(ExitCode.RuntimeError);
        }

        if (obj.GetType().IsSubclassOf(typeof(Standart)))
            (obj as Standart).Run(line, chunk);
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Error at line '{lineNumber}'. Cant parse expression '{string.Join(" ", line)}'"
            );

            Program.Exit(ExitCode.GrammarError);
        }
    }

    public static string IdentifyAndGet(List<Token> line)
    {
        try
        {
            var rule = line[0].Text;

            if (Grammar.standarts.Contains(rule))
                return rule.Replace(rule[0].ToString(), rule[0].ToString().ToUpper());
            else
                return Grammar.operators[rule];
        }
        catch (Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot find grammar rule of expression '{string.Join(' ', line)}'. line {Parser.lineNumber}"
            );

            Program.Exit(ExitCode.GrammarError);

            return null;
        }
    }
}
