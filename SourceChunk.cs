using System;
using System.Text;

namespace SlimScript;

internal class SourceChunk
{
    public List<IVariable> Stack { get; set; }
    public SourceChunk? Parent { get; set; }
    private List<List<Token>> Lines { get; set; }

    public SourceChunk(string[] source, SourceChunk? parent = null)
    {
        Stack = new();
        Parent = parent;
        var processedSource = PreProcessor.Process(source);
        Lines = Lexer.Lex(processedSource);
    }

    public IVariable? GetVar(string name)
    {
        try
        {
            if (Stack.Any(obj => obj.Name == name))
                return Stack.Single(obj => obj.Name == name).Copy();
            else
                return Parent?.GetVar(name);
        }
        catch (Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Variable named '{name}' does not exists. line {Parser.lineNumber}");
            Program.Exit(ExitCode.NullReferenceError);
            return null;
        }
    }

    public void CreateVar(string name, IVariable variable)
    {
        variable.Name = name;

        if (VarExists(name))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot create variable named '{name}'. A variable with same name already exists. line {Parser.lineNumber}"
            );

            Program.Exit(ExitCode.MultipleDeclarationError);                                                                            
        }
        else
            Stack.Add(variable);
    }

    public void SetVar(string name, IVariable variable)
    {
        if (!VarExists(name))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Cannot set value of unexistent variable named '{name}'. line {Parser.lineNumber}"
            );

            Program.Exit(ExitCode.NullReferenceError);
        }
        else
        {
            var tmp = Stack.Where(v => v.Name != name).ToList();
            variable.Name = name;
            tmp.Add(variable);

            Stack = tmp;
        }
    }

    public bool VarExists(string variable)
    {
        bool first = true;
        bool second = true;

        if (Parent != null)
            first = Parent.VarExists(variable);

        second = Stack.Any(key => key.Name == variable);

        return second && first;
    }

    public void Run()
    {
        Parser.Parse(this, Lines);

        Destructor();
    }

    private void Destructor()
    {
        Stack.Clear();
    }
}
