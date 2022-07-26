using System;
using System.Text;

namespace SlimScript;

internal class SourceChunk
{
    public List<IVariable> Stack { get; set; }
    public SourceChunk? Parent { get; set; }
    public List<List<Token>> Lines { get; set; }
    public Parser Parser { get; set; }

    private string _file;

    public SourceChunk(string sourceFile)
    {
        _file = sourceFile;
        var dirName = new FileInfo(sourceFile).DirectoryName;
        Directory.SetCurrentDirectory(dirName);

        var source = File.ReadAllLines(new FileInfo(sourceFile).Name);

        Stack = new();
        Parent = null;
        var processedSource = PreProcessor.Process(source);
        Lines = Lexer.Lex(processedSource);
        Parser = new(this);
    }

    public SourceChunk()
    {
        Stack = new();
        Parent = null;
        Lines = new();
        Parser = new(this);
    }

    public SourceChunk(List<List<Token>> lines, SourceChunk chunk)
    {
        Stack = new();
        Parent = chunk;
        Lines = lines;
        Parser = new(this);
    }

    public IVariable Run(int lineNum = 0)
    {
        var result = Parser.Parse(Lines, lineNum);

        Destructor();

        return result;
    }

    public void Return() => Parser.turn = true;

    public IVariable RunInteractive(string source, int line)
    {
        var processed = PreProcessor.ProcessLine(source);
        Lines.Add(Lexer.LexLine(processed));

        var result = Parser.ParseLine(Lines[line]);

        return result;
    }

    private void Destructor() => Stack.Clear();

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
                return Parent.GetVar(name);
        }
        else
        {
            Error($"Variable named '{name}' does not exists.", ExitCode.NullReferenceError);
            return null;
        }
    }

    public void CreateVar(string name, IVariable variable, bool func = false)
    {
        variable.Name = name;

        if (VarExists(name) && !func)
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

    public void Error(string message, ExitCode code)
    {
        var line = Lines[Parser.lineNumber - 1];
        message =
            $"{message}\nFile: {_file}\nLine: {Parser.lineNumber}\nExpression: {string.Join(' ', line.Select(t => t.Text))}";

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);

        Program.Exit(code);
    }
}
