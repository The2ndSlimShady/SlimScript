namespace SlimScript;

internal class Write : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk, string rule)
    {
        var value = line[1];

        IVariable variable = null;

        if (value.Type != TokenType.Standart)
            variable = Variable.Create(line.ToArray()[1..], chunk);
        // else
        //     Variable.Create();

        Console.WriteLine(variable.Value);

        return variable;
    }
}