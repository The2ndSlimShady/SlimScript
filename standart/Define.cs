namespace SlimScript;

internal class Define : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var name = line[1];
        var keyword = line[2];

        if (keyword.Text != "as")
            chunk.Error($"Unexpected keyword. Expected 'as' got '{keyword.Text}'", ExitCode.DisordantTokenError);

        IVariable variable = Variable.Create(line.ToArray()[3..], chunk);
		
        chunk.CreateVar(name.Text, variable);

        return chunk.GetVar(variable.Name) ?? new Null();
    }
}
