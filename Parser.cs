namespace SlimScript;

internal class Parser
{
    public int lineNumber = 0;

    public bool turn = false;

    public bool function = false;

    public SourceChunk Chunk { get; set; }

    public Parser(SourceChunk chunk) => Chunk = chunk;

    public IVariable Parse(List<List<Token>> lines)
    {
        turn = false;
        IVariable result = new Number();

        for (int i = 0; i < lines.Count; i++)
        {
            if (turn)
                break;

            List<Token>? line = lines[i];
            result = ParseLine(line);
        }

        return result;
    }

    public IVariable ParseLine(List<Token> line)
    {
        lineNumber++;

        if (function)
            return new Func().Run(line, Chunk);

        string? rule = null;
        object? obj;

        try
        {
            rule = IdentifyAndGet(line, Chunk);
            obj = Variable.CreateType(rule);
        }
        catch (Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot execute command '{rule?.ToLower()}'. Specified command not found. line {lineNumber}"
            );

            Program.Exit(ExitCode.RuntimeError);

            return new Number(new Token("-1"));
        }

        if (obj.GetType().IsSubclassOf(typeof(Standart)))
            return (obj as Standart).Run(line, Chunk);
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Error at line '{lineNumber}'. Cant parse expression '{string.Join(" ", line)}'"
            );

            Program.Exit(ExitCode.GrammarError);

            return new Number(new Token("-1"));
        }
    }

    public static string IdentifyAndGet(List<Token> line, SourceChunk chunk)
    {
        try
        {
            var rule = line[0].Text;

            if (Grammar.standarts.Contains(rule))
                return $"{rule[0].ToString().ToUpper()}{rule[1..]}";
            else
                return Grammar.operators[rule];
        }
        catch (Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot find grammar rule of expression '{string.Join(' ', line)}'. line {chunk.Parser.lineNumber}"
            );

            Program.Exit(ExitCode.GrammarError);

            return "";
        }
    }
}
