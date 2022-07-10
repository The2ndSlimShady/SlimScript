namespace SlimScript;

internal class Set : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var name = line[1];
        var keyword = line[2];

        if (keyword.Text != "to")
            chunk.Error($"Unexpected keyword. Expected 'to' got '{keyword.Text}'", ExitCode.DisordantTokenError);

        IVariable variable = Variable.Create(line.ToArray()[3..], chunk);

        chunk.SetVar(name.Text, variable);

        return chunk.GetVar(variable.Token.Text);
    }
}
