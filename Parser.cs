using System.Reflection;
using System.Text;
using System;

namespace SlimScript;

internal class Parser
{
    public static int lineNumber = 0;

    public static void Parse(SourceChunk chunk, List<List<Token>> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            lineNumber = i + 1;
            List<Token>? line = lines[i];
            string rule = IdentifyAndGet(line);

            var obj = Variable.CreateType(rule);

            if (obj.GetType().IsSubclassOf(typeof(Standart)))
                (obj as Standart).Run(line, chunk, IdentifyRules(line));
            else if (obj.GetType() == typeof(Identifier))
                (obj as Identifier)
                    .GetType()
                    .GetMethod("Identify")
                    .Invoke(null, line.Cast<object?>().ToArray());
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"Error at line '{lineNumber}'. Cant parse expression '{string.Join(" ", line)}'"
                );

                Program.Exit(ExitCode.GrammarError);
            }
        }
    }

    public static string IdentifyRules(List<Token> line)
    {
        StringBuilder sb = new();
        sb.Append(line[0].Text);

        for (int i = 1; i < line.Count; i++)
        {
            Token token = line[i];
            sb.Append($" {token}");
        }

        var matching = Grammar.rules.SingleOrDefault(rule => sb.ToString().Contains(rule));

        if (string.IsNullOrEmpty(matching))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Grammar error at line: {lineNumber}. Cannot find grammar rule of expression '{string.Join(" ", line.Select(t => t.Text))}'."
            );
            Program.Exit(ExitCode.GrammarError);
            return null;
        }
        else
            return matching;
    }

    public static string IdentifyAndGet(List<Token> line)
    {
        var matching = IdentifyRules(line);

        var rule = matching.Split(" ")[0];

        if (Grammar.standarts.Contains(rule))
            return rule.Replace(rule[0].ToString(), rule[0].ToString().ToUpper());
        else
            return Grammar.operators[rule];
    }
}
