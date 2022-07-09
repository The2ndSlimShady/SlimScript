namespace SlimScript;

internal class Define : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var name = line[1];
        var keyword = line[2];

        if (keyword.Text != "as")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Unexpected keyword at line {Parser.lineNumber}. Expected 'as' got '{keyword.Text}'"
            );
            Program.Exit(ExitCode.GrammarError);
        }

        IVariable variable = Variable.Create(line.ToArray()[3..], chunk);
        chunk.CreateVar(name.Text, variable);

        return chunk.GetVar(variable.Name);
    }
}
