namespace SlimScript;

internal class Error : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        // var msgT = line.Count > 2 ? line.ToArray()[1..2] : line.ToArray()[1..];
        // var codeT = line.Count > 2 ? line.ToArray()[2..] : new[] { new Token(new("\"\"")) };

        List<Token> msgT;
        List<Token> codeT;

        if (line.Count > 2)
        {
            msgT = line.GetRange(1, 1);
            codeT = line.GetRange(2, line.Count - 2);
        }
        else
        {
            msgT = line.GetRange(1, line.Count - 1);
            codeT = new(){ new Token("\"Not Provided\"")};
        }

        var message = Variable.Create(msgT.ToArray(), chunk);

        var code = Variable.Create(codeT.ToArray(), chunk);

        chunk.Error($"{message.Value}\nData: {code.Value}", ExitCode.CodeRuntimeError);

        return new Null();
    }
}
