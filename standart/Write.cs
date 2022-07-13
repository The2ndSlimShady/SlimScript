using System.Text;

namespace SlimScript;

internal class Write : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var args = Operator.ReadyParams(line.ToArray(), chunk, 0);

        string writeStr = string.Join("", args.Select(t => Variable.Create(new[] { t }, chunk).Value));

        Console.WriteLine(writeStr);

        return new Word(new($"\"{writeStr}\""));
    }
}
