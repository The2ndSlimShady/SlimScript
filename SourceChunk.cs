using System;
using System.Text;

namespace SlimScript;

public class SourceChunk
{
    public List<IVariable> Stack { get; set; }

    public SourceChunk? Parent { get; set; }

    internal List<List<Token>> Lines { get; set; }

    internal Parser Parser { get; set; }

    internal string _file;

    internal ChunkType ChunkType { get; set; } = ChunkType.Normal;

    public SourceChunk()
    {
        Stack = new();
        Parent = null;
        Lines = new();
        Parser = new(this);
        _file = "???";
    }

    internal SourceChunk(List<List<Token>> lines, SourceChunk chunk)
    {
        Stack = new();
        Parent = chunk;
        Lines = lines;
        Parser = new(this);
        _file = "???";
    }

    public SourceChunk(string sourceFile)
    {
        var dirName = new FileInfo(sourceFile).DirectoryName ?? "./";
        Directory.SetCurrentDirectory(dirName);

        string[] source;
        if (Path.GetExtension(sourceFile) == ".csso")
        {
            var tmpStr = Encoding.UTF8.GetString(Program.Decompress(File.ReadAllBytes(sourceFile)));
            source = tmpStr.Split(Environment.NewLine, StringSplitOptions.TrimEntries);
        }
        else
            source = File.ReadAllLines(new FileInfo(sourceFile).Name);

        _file = sourceFile;

        Stack = new();
        Parent = null;

#if DEBUG
        Write.StandartOutput.WriteLine($"Pre-Processing Source Code...");
#endif

        var processedSource = PreProcessor.Process(source, this);
        Lines = Lexer.Lex(processedSource, this);
        Parser = new(this);
    }

    public SourceChunk(string[] source, int origin)
    {
        Stack = new();
        Parent = null;
#if DEBUG
        Write.StandartOutput.WriteLine($"Pre-Processing Source Code...");
#endif
        var processedSource = PreProcessor.Process(source, this);
        Lines = Lexer.Lex(processedSource, this);
        Parser = new(this);
        _file = "???";
    }

    public IVariable Run()
    {
        var result = Parser.Parse(Lines);

        return result;
    }

    internal IVariable Run(int lineNum)
    {
        var result = Parser.Parse(Lines, lineNum);

        return result;
    }

    internal void Return() => Parser.turn = true;

    public IVariable RunInteractive(string source)
    {
        var processed = PreProcessor.ProcessLine(source);
        Lines.Add(Lexer.LexLine(processed));

        var result = Parser.ParseLine(Lines.Last());

        return result;
    }

    public void ClearSource() => Stack.Clear();

    public IVariable? GetVar(string name)
    {
        if (VarExists(name))
        {
            if (Stack.Any(obj => obj.Name == name))
            {
                var variable = Stack.Single(obj => obj.Name == name);
                return Variable.Copy(variable, this);
            }
            else
                return Parent?.GetVar(name);
        }
        else
        {
            Error($"Variable named '{name}' does not exists.", ExitCode.NullReferenceError);
            return null;
        }
    }

    public void CreateVar(string name, IVariable variable)
    {
        variable.Name = name;

        if (Stack.Any(v => v.Name == name))
        {
            Error(
                $"Cannot create variable named '{name}'. A variable with same name already exists.",
                ExitCode.MultipleDeclarationError
            );
        }
        else
            Stack.Add(variable);
    }

    public void SetVar(string name, IVariable variable)
    {
        if (!VarExists(name))
        {
            Error(
                $"Cannot set value of unexistent variable named '{name}'.",
                ExitCode.NullReferenceError
            );
        }
        else
        {
            if (Stack.Any(v => v.Name == name))
            {
                var tmp = Stack.Where(v => v.Name != name).ToList();
                variable.Name = name;
                tmp.Add(variable);

                Stack = tmp;
            }
            else
                Parent?.SetVar(name, variable);
        }
    }

    public void DeleteVar(string name)
    {
        if (!VarExists(name))
        {
            Error(
                $"Cannot delete value of unexistent variable named '{name}'.",
                ExitCode.NullReferenceError
            );
        }
        else
        {
            var tmp = Stack.Where(v => v.Name != name).ToList();
            Stack = tmp;
        }
    }

    public bool VarExists(string variable)
    {
        if (Stack.Any(v => v.Name == variable))
            return true;
        else
            return Parent?.VarExists(variable) ?? false;
    }

    internal void Error(string message, ExitCode code)
    {
        SourceChunk ch = this;

        while (true)
        {
            if (ch.Parent == null)
                break;

            ch = ch.Parent;
        }

        var line = ch.Lines[Parser.lineNumber - 1];
        message =
            $"{message}\nFile: {Path.GetFileNameWithoutExtension(ch._file)}_p.sso\nLine: {Parser.lineNumber}\nExpression: {string.Join(' ', line.Select(t => t.Text))}";

        Console.ForegroundColor = ConsoleColor.Red;
        Write.StandartOutput.WriteLine(message);

        Program.Exit(code);
    }
}

internal enum ChunkType
{
    Loop,
    Normal
}