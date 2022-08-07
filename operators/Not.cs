using System.Text;

namespace SlimScript;

internal class Not : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = Variable.Create(line[1..], chunk);

        if (arg.Token.Type != TokenType.Bool)
        {
            chunk.Error(
                $"Not Function Does Not Exists on type '{arg.Token}'",
                ExitCode.DisordantTokenError
            );

            return new Null();
        }

        var val = !((Bool)arg).Val;
        arg.Value = val;
        arg.Token = new Token(arg.Value.ToString()?.ToLower()??"null");

        return arg;
    }
}
