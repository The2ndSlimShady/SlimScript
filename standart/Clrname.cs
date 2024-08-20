namespace SlimScript;

internal class Clrname : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var var = Variable.Create(line.ToArray()[1..], chunk);

        return new Word(new($"\"{var.GetType().FullName}\""));
    }
}