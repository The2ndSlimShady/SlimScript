using System.Runtime.Serialization;

namespace SlimScript;

internal class Parser
{
<<<<<<< HEAD
    public int lineNumber = 0;

    public bool turn = false;

    public bool function = false;
=======
    public int lineNumber;

    public bool turn = false;

    public (int level, string block) block = (0, "");
>>>>>>> dev

    public SourceChunk Chunk { get; set; }

    public Parser(SourceChunk chunk) => Chunk = chunk;

<<<<<<< HEAD
    public IVariable Parse(List<List<Token>> lines)
    {
        turn = false;
=======
    public IVariable Parse(List<List<Token>> lines, int lineNum = 0)
    {
        turn = false;
        lineNumber = lineNum;
        block = (0, "");
>>>>>>> dev
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

<<<<<<< HEAD
        if (function)
            return new Func().Run(line, Chunk);
=======
        if (block.level != 0)
        {
            var exprBlock = Variable.CreateType(block.block);

            if (!exprBlock.GetType().IsSubclassOf(typeof(Standart)))
            {
                Chunk.Error($"Error at line '{lineNumber}'.", ExitCode.GrammarError);

                return new Number(new Token("-1"));
            }

            return (exprBlock as Standart).Run(line, Chunk);
        }
>>>>>>> dev

        string? rule = null;
        object? obj;

        try
        {
            rule = IdentifyAndGet(line, Chunk);
            obj = Variable.CreateType(rule);
        }
        catch (Exception)
        {
<<<<<<< HEAD
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot execute command '{rule?.ToLower()}'. Specified command not found. line {lineNumber}"
            );

            Program.Exit(ExitCode.RuntimeError);
=======
            Chunk.Error($"Cannot execute command '{rule?.ToLower()}'.", ExitCode.RuntimeError);
>>>>>>> dev

            return new Number(new Token("-1"));
        }

        if (obj.GetType().IsSubclassOf(typeof(Standart)))
            return (obj as Standart).Run(line, Chunk);
        else
        {
            Chunk.Error($"Given expression '{rule}' is not a Standart function.", ExitCode.GrammarError);

<<<<<<< HEAD
            Program.Exit(ExitCode.GrammarError);

=======
>>>>>>> dev
            return new Number(new Token("-1"));
        }
    }

    public static string IdentifyAndGet(List<Token> line, SourceChunk chunk)
<<<<<<< HEAD
    {
        try
        {
            var rule = line[0].Text;

            if (Grammar.standarts.Contains(rule))
                return $"{rule[0].ToString().ToUpper()}{rule[1..]}";
=======
    {
        var rule = line[0].Text;

        return IdentifyAndGet(rule, chunk);
    }

    public static string IdentifyAndGet(string expression, SourceChunk chunk)
    {
        try
        {
            if (Grammar.standarts.Contains(expression))
                return $"{expression[0].ToString().ToUpper()}{expression[1..]}";
>>>>>>> dev
            else
                return Grammar.operators[expression];
        }
        catch (Exception)
        {
<<<<<<< HEAD
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot find grammar rule of expression '{string.Join(' ', line)}'. line {chunk.Parser.lineNumber}"
            );

            Program.Exit(ExitCode.GrammarError);

=======
            chunk.Error(
                $"Cannot find grammar rule of expression '{expression}'.",
                ExitCode.GrammarError
            );

>>>>>>> dev
            return "";
        }
    }
}
