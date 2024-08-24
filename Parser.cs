using System.Globalization;

namespace SlimScript;

internal class Parser
{
    public int lineNumber;

    public bool turn = false;

    public (int level, string block) block = (0, "");

    public SourceChunk? Chunk { get; set; }

    public Parser(SourceChunk chunk) => Chunk = chunk;

    public IVariable Parse(List<List<Token>> lines, int lineNum = 0)
    {
        turn = false;
        lineNumber = lineNum;
        block = (0, "");
        IVariable result = new Number();

        for (int i = 0; i < lines.Count; i++)
        {
            if (turn)
                break;

            List<Token>? line = lines[i];
            result = ParseLine(line);

            if (Chunk?.ChunkType == ChunkType.Loop && (result as Word?)?.Val == "break")
                break;
        }

        if (Chunk?.Parent == null && block.level != 0)
            Chunk?.Error($"Unclosed block detected.", ExitCode.ParserError);

        return result;
    }

    public IVariable ParseLine(List<Token> line)
    {
        lineNumber++;

        if (block.level != 0)
        {
            var exprBlock = Variable.CreateType(block.block);

            if (!exprBlock?.GetType().IsSubclassOf(typeof(Standart)) ?? false)
            {
                Chunk?.Error($"Error at line '{lineNumber}'.", ExitCode.GrammarError);

                return new Number(new Token("-1"));
            }

            return (exprBlock as Standart)?.Run(line, Chunk ?? new()) ?? new Null();
        }

        string? rule = null;
        object? obj;

        try
        {
			rule = IdentifyAndGet(line, Chunk ?? new());

			if (rule == "CLR")
				return Variable.Create(line.ToArray(), Chunk ?? new());

			obj = Variable.CreateType(rule);
        }
        catch (Exception)
        {
			Chunk?.Error($"Cannot execute command '{rule?.ToLower()}'.", ExitCode.RuntimeError);

			return new Null();
        }

        if (obj?.GetType().IsSubclassOf(typeof(Standart)) ?? false)
            return (obj as Standart)?.Run(line, Chunk ?? new()) ?? new Null();
        else
        {
            Chunk?.Error(
                $"Given expression '{rule}' is not a Standart function.",
                ExitCode.GrammarError
            );

            return new Number(new Token("-1"));
        }
    }

    public static string IdentifyAndGet(List<Token> line, SourceChunk chunk)
    {
        var rule = line[0].Text;

        return IdentifyAndGet(rule, chunk);
    }

    public static string IdentifyAndGet(string expression, SourceChunk chunk)
    {
        try
        {
            if (Grammar.standarts.Contains(expression))
                return $"{expression[0].ToString().ToUpper(CultureInfo.GetCultureInfoByIetfLanguageTag("en-us"))}{expression[1..]}";
            else if (expression.Contains("->") || expression.Contains(':'))
                return "CLR";
            else if (Grammar.operators.ContainsKey(expression))
                return Grammar.operators[expression];
            else
            {
               chunk.Error(
                    $"Cannot find grammar rule of expression '{expression}'.",
                    ExitCode.GrammarError
                );

                return "";
            }
        }
        catch (Exception)
        {
            chunk.Error(
                $"Cannot find grammar rule of expression '{expression}'.",
                ExitCode.GrammarError
            );

            return "";
        }
    }
}
