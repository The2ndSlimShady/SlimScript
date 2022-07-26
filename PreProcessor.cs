using System.Text;
using System;

namespace SlimScript;

internal class PreProcessor
{
    public static string[] Process(string[] source)
    {
        source = PrePreProcess(source);

        List<string> processedSource = new();

        bool isString = false;
        StringBuilder token = new();

        for (int i = 0; i < source.Length; i++)
        {
            string element = source[i];

            if (element.StartsWith("--"))
                continue;

            for (int j = 0; j < element.Length; j++)
            {
                char item = element[j];

                if (isString)
                {
                    token.Append(item);

                    if (item == '"')
                        isString = false;
                }
                else if (item == ' ')
                {
                    string val = token.ToString();

                    if (!string.IsNullOrEmpty(val))
                    {
                        processedSource.Add(val);
                        token = new();
                    }
                }
                else
                {
                    if (item == '-' && element[j + 1] == '-')
                        break;
                    if (item == '"')
                        isString = true;

                    token.Append(item);
                }
            }

            if (token.Length != 0)
                processedSource.Add(token.ToString());

            processedSource.Add("EOL");
            isString = false;
            token = new();
        }

#if DEBUG
        if (!Program.interactive)
        {
            StringBuilder b = new();
            bool isIndent = true;
            foreach (var item in processedSource)
            {
                if (item != "EOL")
                {
                    b.Append($" {item}");
                    isIndent = false;
                }
                else
                {
                    b.Append(Environment.NewLine);
                    isIndent = true;
                }
            }

            File.WriteAllText("post_process.ss", b.ToString());
        }
#endif
        return processedSource.ToArray();
    }

    private static string[] PrePreProcess(string[] source)
    {
        List<string> prepreprocessed = new();

        for (int i = 0; i < source.Length; i++)
        {
            string? item = source[i];

            if (!item.StartsWith('@'))
            {
                prepreprocessed.Add(item);
                continue;
            }

            var realItem = item.Replace("@", string.Empty);
            var line = ProcessLine(realItem).Where(s => s != "EOL").ToArray();

            switch (line[0])
            {
                case "include":
                    string file = $"{Directory.GetCurrentDirectory()}\\{line[1].Replace("\"", string.Empty)}.ss";
                    if (!File.Exists(file))
                    {
                        //TODO: Standart Library Modules

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            $"Cannot find file on path '{file}'.\nExpression: @{realItem}\nLine {i + 1}"
                        );
                        Program.Exit(ExitCode.PreProcessorError);
                    }

                    var range = File.ReadAllLines(file);
                    prepreprocessed.Add(string.Join(" ", Process(range)));
                    break;
            }
        }

        return prepreprocessed.ToArray();
    }

    public static string[] ProcessLine(string line)
    {
        List<string> processedSource = new();

        bool isString = false;
        StringBuilder token = new();

        for (int j = 0; j < line.Length; j++)
        {
            char item = line[j];

            if (isString)
            {
                token.Append(item);

                if (item == '"')
                {
                    isString = false;

                    processedSource.Add(token.ToString());
                    token = new();
                }
            }
            else if (item == ' ')
            {
                string val = token.ToString();

                if (!string.IsNullOrEmpty(val))
                {
                    processedSource.Add(val);
                    token = new();
                }
            }
            else
            {
                if (item == '-' && line[j + 1] == '-')
                    break;
                if (item == '"')
                    isString = true;

                token.Append(item);
            }
        }

        if (token.Length != 0)
            processedSource.Add(token.ToString());

        processedSource.Add("EOL");

        return processedSource.ToArray();
    }
}
