using System.Text;

namespace SlimScript;

internal class Not : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = Variable.Create(line[1..], chunk);
    
        if (arg.Token.Type != TokenType.Boolean)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"Not function does not exists on type '{arg.Token}'. line {Parser.lineNumber}");

            Program.Exit(ExitCode.DisordantTokenError);

            return null;
        }

        var val = !((Bool)arg).Val;
        arg.Value = val;
        arg.Token = new Token(arg.Value.ToString().ToLower());

        return arg;
    }
}
