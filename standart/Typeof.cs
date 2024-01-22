using System.Collections;
namespace SlimScript;

internal class Typeof : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var variable = Variable.Create(line.ToArray()[1..], chunk);

        if (variable.Token.Type != TokenType.CLR)
            return new Word(new($"\"{variable.Token.Type}\""));
        else
            return new Word(
                new($"\"{(variable as CLR?)?.GetTypeString()}\"")
            );
    }
}
