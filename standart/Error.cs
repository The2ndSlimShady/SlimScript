namespace SlimScript;

internal class Error : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var msgT = line.Count > 2 ? line.ToArray()[1..2] : line.ToArray()[1..];
        var codeT = line.Count > 2 ? line.ToArray()[2..] : new[] { new Token(new("\"\"")) };

        var message = Variable.Create(msgT, chunk);

        var code = Variable.Create(codeT, chunk);

        chunk.Error($"{message.Value}\nData: {code.Value}", ExitCode.CodeRuntimeError);

        return new Word(new("null"));
    }
}
