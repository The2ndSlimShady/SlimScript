using System.Text;

namespace SlimScript;

internal class Equals : Operator
{
    public override IVariable Apply(Token[] line, SourceChunk chunk)
    {
        var arg = ReadyParams(line, chunk);
    
        if (arg[0].Type != arg[1].Type)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"Cannot apply Equals operation on types '{arg[0]}' and '{arg[1]}. line {Parser.lineNumber}'");

            Program.Exit(ExitCode.DisordantTokenError);

            return null;
        }

        Token first = arg[0];
        Token second = arg[1];

        Bool result = new();
        result.Val = first.Text == second.Text;

        result.Token = new Token(result.Val.ToString().ToLower());

        return result;
    }
}
