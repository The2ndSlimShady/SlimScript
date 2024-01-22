using System;
using System.Text;

namespace SlimScript;

public class SourceChunk
{
    public List<IVariable> Stack { get; set; }

    public SourceChunk? Parent { get; set; }

    public List<List<Token>> Lines { get; internal set; }

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

    public SourceChunk(string sourceFile, SourceChunk? parent = null)
    {
        _file = sourceFile;

        Stack = new();
        Parent = parent;

        if (!File.Exists(sourceFile))
            Error("Given file does not exists.", ExitCode.NoInputFile);

        var dirName = new FileInfo(sourceFile).DirectoryName ?? "./";
        Directory.SetCurrentDirectory(dirName);
        sourceFile = Path.GetFileName(sourceFile);

        string[] source;
        if (Path.GetExtension(sourceFile) == ".csso")
        {
            var tmpStr = Encoding.UTF8.GetString(Program.Decompress(File.ReadAllBytes(sourceFile)));
            source = tmpStr.Split(Environment.NewLine, StringSplitOptions.TrimEntries);
        }
        else
            source = File.ReadAllLines(
                new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), sourceFile)).Name
            );

        var processedSource = PreProcessor.Process(source, this);

        Lines = Lexer.Lex(processedSource, this);
        Parser = new(this);
    }

    public SourceChunk(string[] source)
    {
        Stack = new();
        Parent = null;

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

    internal IVariable RunInteractive(string source)
    {
        var processed = PreProcessor.ProcessLine(source, this);
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
                return Variable.Copy(variable);
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
			//Console.WriteLine(GetVar(name)?.Type);
			
            if (GetVar(name)?.Type != variable.Type && GetVar(name)?.Type != TokenType.Null)
            {
                Error(
                    $"Cannot set variable '{name}' to '{variable}'. Type '{GetVar(name)?.Type}' does not match '{variable.Type}'.",
                    ExitCode.DisordantTokenError
                );
            }
            else if (Stack.Any(v => v.Name == name))
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
