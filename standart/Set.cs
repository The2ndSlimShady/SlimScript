namespace SlimScript;

internal class Set : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk, string rule)
    {
        var name = line[1];
        var keyword = line[2];

        if (keyword.Text != "to")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"Unexpected keyword at line {Parser.lineNumber}. Expected 'to' got '{keyword.Text}'"
            );
            Program.Exit(ExitCode.GrammarError);
        }

        IVariable variable = Variable.Create(line.ToArray()[3..], chunk);

        chunk.SetVar(name.Text, variable);

        return chunk.GetVar(variable.Token.Text);
    }
}
