using System.Text;
using System;

namespace SlimScript;

internal class PreProcessor
{
    private static List<string> _includedModules = new();

    public static string[] Process(string[] source, SourceChunk chunk)
    {
        source = PrePreProcess(source, chunk);

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
        if (!Program.interactive && Program.Debug)
        {
            StringBuilder b = new();
            bool first = true;

            foreach (var item in processedSource)
            {
                if (item != "EOL")
                {
                    b.Append($"{(first ? string.Empty : " ")}{item}");
                    first = false;
                }
                else
                {
                    first = true;
                    b.Append(Environment.NewLine);
                }
            }

            File.WriteAllLines(
                $"{Path.GetFileNameWithoutExtension(chunk._file) ?? string.Empty}_p.sso",
                b.ToString()
                    .Split(Environment.NewLine)
                    .Where(l => !string.IsNullOrEmpty(l) || !string.IsNullOrWhiteSpace(l))
            );
        }
#endif
        return processedSource.ToArray();
    }

    private static string[] PrePreProcess(string[] source, SourceChunk chunk)
    {
        List<string> prepreprocessed = new();

        bool inIf = false;
        bool ifCond = false;

        for (int i = 0; i < source.Length; i++)
        {
            string? item = source[i];

            item = item.Trim();

            var realItem = item.Replace("@", string.Empty);
            var line = ProcessLine(realItem).Where(s => s != "EOL").ToArray();

            if (inIf)
            {
                if (!ifCond)
                {
                    if (!item.StartsWith("@"))
                        continue;

                    switch (line[0])
                    {
                        case "elif":
                            var cntd = DetermineDirective(line);

                            ifCond = cntd;
                            break;

                        case "else":
                            ifCond = true;
                            break;

                        case "endif":
                            ifCond = false;
                            inIf = false;
                            break;
                    }

                    continue;
                }
                else if (line.Length != 0 && line[0] == "endif")
                {
                    ifCond = false;
                    inIf = false;

                    continue;
                }
            }
            else if (ifCond)
            {
                if (line.Length != 0 && line[0] == "endif")
                {
                    ifCond = false;
                    inIf = false;
                }

                continue;
            }

            if (!item.StartsWith('@'))
            {
                prepreprocessed.Add(item);
                continue;
            }

            switch (line[0])
            {
                case "include":
                    string fileName = $"{line[1].Replace("\"", string.Empty)}.ss";
                    string file = $"{Directory.GetCurrentDirectory()}\\{fileName}";
                    if (!File.Exists(file))
                    {
#if !DEBUG
                        file =
                            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SlimScript\\lib\\{fileName}";
#else
                        file = $"{Program.BasePath}\\lib\\{fileName}";
#endif
                        if (!File.Exists(file))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(
                                $"Cannot find file on path '{file}'.\nExpression: @{realItem}\nLine {i + 1}"
                            );
                            Program.Exit(ExitCode.PreProcessorError);
                        }
                    }

                    var range = File.ReadAllLines(file);
                    prepreprocessed.Add(string.Join(" ", Process(range, chunk)));
                    break;

                case "module":
                    if (_includedModules.Contains(line[1]))
                        return prepreprocessed.ToArray();
                    else
                        _includedModules.Add(line[1]);
                    break;

                case "if":
                    var cntd = DetermineDirective(line);

                    inIf = true;
                    ifCond = cntd;
                    break;

                case "elif":
                case "else":
                    if (!inIf)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            $"Cannot start if-else block with {line[0]}.\nExpression: @{realItem}\nLine {i + 1}"
                        );
                        Program.Exit(ExitCode.PreProcessorError);
                    }
                    else
                    {
                        ifCond = true;
                        inIf = false;
                    }
                    break;

                case "define":
                    if (!_includedModules.Contains(line[1]))
                        _includedModules.Add(line[1]);
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            $"Cannot define '{line[1]}'. It's already defined.\nExpression: @{realItem}\nLine {i + 1}"
                        );
                        Program.Exit(ExitCode.PreProcessorError);
                    }
                    break;

                case "undef":
                    if (_includedModules.Contains(line[1]))
                        _includedModules.Remove(line[1]);
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            $"Cannot undef unexistent define.\nExpression: @{realItem}\nLine {i + 1}"
                        );
                        Program.Exit(ExitCode.PreProcessorError);
                    }
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        $"Cannot determine given directive '{line[0]}'.\nExpression: @{realItem}\nLine {i + 1}"
                    );
                    Program.Exit(ExitCode.PreProcessorError);
                    break;
            }
        }

        return prepreprocessed.ToArray();
    }

    private static bool DetermineDirective(string[] line)
    {
        if (line[1] == "not")
        {
            var cnd = _includedModules.Contains(line[2]);

            return !cnd;
        }
        else
            return _includedModules.Contains(line[1]);
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
