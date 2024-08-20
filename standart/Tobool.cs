using System.Runtime.InteropServices;
namespace SlimScript;

internal class Tobool : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var variable = Variable.Create(line.ToArray()[1..], chunk);

        try
        {
            switch (variable.Token.Type)
            {
                case TokenType.Number:
                    if ((double)variable.Value == 0)
                        return new Bool(new("false"));
                    else
                        return new Bool(new("true"));

                case TokenType.Bool:
                    return variable;

                case TokenType.Word:
                    return new Bool()
                    {
                        Token = new(variable.Token.Text.ToLower()),
						Type = TokenType.Bool,
                        Value = Convert.ToBoolean(variable.Token.Text.Replace("\"", string.Empty))
                    };
					
				case TokenType.Null:
					return new Bool()
					{
						Token = new("false"),
						Value = false
					};

                default:
                    chunk.Error(
                        $"Cannot convert type {variable.Token} to <Number>",
                        ExitCode.DisordantTokenError
                    );
                    break;
            }
        }
        catch (Exception e)
        {
            chunk.Error(
                $"Cannot convert \"{variable.GetString()}\" to Boolean. {e.Message}",
                ExitCode.RuntimeError
            );
            return new Null();
        }

        return new Null();
    }
}
