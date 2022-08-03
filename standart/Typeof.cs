using System.Collections;
namespace SlimScript;

internal class Typeof : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var var = Variable.Create(line.ToArray()[1..], chunk);

        if (var.Token.Type != TokenType.CLR)
            return new Word(new($"\"{var.Token.Type}\""));
        else
            return new Word(
                new($"\"{(var as CLR?)?.GetTypeString()}\"")
            );
    }
}
