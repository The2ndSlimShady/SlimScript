using System.Text;
using System;

namespace SlimScript;

internal class PreProcessor
{
    private static readonly List<string> _defines = new();
    private static readonly Dictionary<string, string> _macros = new();

    public static string[] Process(string[] source, SourceChunk chunk)
    {
        var processedSource = PreProcess(source, chunk);

#if DEBUG
        if (!Program.interactive && Program.Debug && (Path.GetFileNameWithoutExtension(chunk._file) != "???"))
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

        return processedSource;
    }

    private static string[] PreProcess(string[] source, SourceChunk chunk)
    {
        if (Program.Debug)
            _defines.Add("DEBUG");

        List<string> preProcessed = new();

        bool inIf = false;
        bool ifCond = false;

        for (int i = 0; i < source.Length; i++)
        {
            string? item = source[i];

            if (string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item))
                continue;

            item = item.Trim();

            var realItem = item.Replace("@", string.Empty);
            var line = ProcessLine(realItem, chunk).Where(s => s != "EOL").ToArray();

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
                preProcessed.AddRange(ProcessLine(item, chunk));
                continue;
            }

            switch (line[0])
            {
                case "include":
                    string fileName = $"{line[1].Replace("\"", string.Empty).Replace('.','/')}.ss";
                    string file = $"{Directory.GetCurrentDirectory()}\\{fileName}";

                    if (!File.Exists(file))
                    {
#if DEBUG
                        file = $"{Program.BasePath}\\lib\\{fileName}";
#else
						file = GlobalSettings.GetPathToSystemFiles($"lib\\{fileName}");
#endif
                        if (!File.Exists(file))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Write.StandartOutput.WriteLine(
                                $"Cannot find file on path '{file}'.\nExpression: @{realItem}\nLine {i + 1}"
                            );
                            Program.Exit(ExitCode.PreProcessorError);
                        }
                    }

                    var range = File.ReadAllLines(file);
                    preProcessed.AddRange(PreProcess(range, chunk));
                    break;

                case "module":
                    if (_defines.Contains(line[1]))
                        return preProcessed.ToArray();
                    else
                        _defines.Add(line[1]);
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
                        Write.StandartOutput.WriteLine(
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
                    if (!_defines.Contains(line[1]))
                        _defines.Add(line[1]);
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Write.StandartOutput.WriteLine(
                            $"Cannot define '{line[1]}'. It's already defined.\nExpression: @{realItem}\nLine {i + 1}"
                        );
                        Program.Exit(ExitCode.PreProcessorError);
                    }
                    break;

                case "undef":
                    if (_defines.Contains(line[1]))
                        _defines.Remove(line[1]);
                    else if (_macros.ContainsKey(line[1]))
                        _macros.Remove(line[1]);
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Write.StandartOutput.WriteLine(
                            $"Cannot undef unexistent define.\nExpression: @{realItem}\nLine {i + 1}"
                        );
                        Program.Exit(ExitCode.PreProcessorError);
                    }
                    break;

                case "macro":
                    if (line.Length < 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Write.StandartOutput.WriteLine(
                            $"Cannot create macro with no values\nExpression: @{realItem}\nline {i + 1}"
                        );
                        Program.Exit(ExitCode.PreProcessorError);
                    }
                    if (!_macros.ContainsKey(line[1]))
                        _macros.Add(line[1], string.Concat(line[2..]));
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Write.StandartOutput.WriteLine(
                            $"Cannot create macro '{line[1]}'. It already exists.\nExpression: @{realItem}\nline {i + 1}"
                        );
                        Program.Exit(ExitCode.PreProcessorError);
                    }
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Write.StandartOutput.WriteLine(
                        $"Cannot determine given directive '{line[0]}'.\nExpression: @{realItem}\nLine {i + 1}"
                    );
                    Program.Exit(ExitCode.PreProcessorError);
                    break;
            }
        }

        return preProcessed.ToArray();
    }

    private static bool DetermineDirective(string[] line)
    {
        if (line[1] == "not")
        {
            var cnd = _defines.Contains(line[2]);

            return !cnd;
        }
        else
            return _defines.Contains(line[1]);
    }

    public static string[] ProcessLine(string[] line, SourceChunk chunk) => ProcessLine(string.Join(' ', line), chunk);

    public static string[] ProcessLine(string line, SourceChunk chunk)
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
                    if (_macros.ContainsKey(val))
                        processedSource.Add(_macros[val]);
                    else
                        processedSource.Add(val);

                    token = new();
                }
            }
            else
            {
                if (item == '-' && j + 1 < line.Length && line[j + 1] == '-')
                    break;
                if (item == '"')
                    isString = true;

                token.Append(item);
            }
        }

        if (token.Length != 0)
        {
            string val = token.ToString();

            if (_macros.ContainsKey(val))
                processedSource.Add(_macros[val]);
            else
                processedSource.Add(val);
        }

        if (isString)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Write.StandartOutput.WriteLine($"Unfinished string at expression: {line}\nFile: {chunk._file}");
            Console.ResetColor();

            Program.Exit(ExitCode.PreProcessorError);
        }

        processedSource.Add("EOL");

        return processedSource.ToArray();
    }
}
