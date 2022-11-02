using System.Runtime.InteropServices;
namespace SlimScript;

internal class Tobool : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var var = Variable.Create(line.ToArray()[1..], chunk);

        try
        {
            switch (var.Token.Type)
            {
                case TokenType.Number:
                    if ((double)var.Value == 0)
                        return new Bool(new("false"));
                    else
                        return new Bool(new("true"));

                case TokenType.Bool:
                    return var;

                case TokenType.Word:
                    return new Bool()
                    {
                        Token = new(var.Token.Text.ToLower()),
                        Value = Convert.ToBoolean(var.Token.Text.Replace("\"", string.Empty))
                    };

                default:
                    chunk.Error(
                        $"Cannot convert type {var.Token} to <Number>",
                        ExitCode.DisordantTokenError
                    );
                    break;
            }
        }
        catch (Exception e)
        {
            chunk.Error(
                $"Cannot convert \"{var.GetString()}\" to Boolean. {e.Message}",
                ExitCode.RuntimeError
            );
            return new Null();
        }

        return new Null();
    }
}
