namespace SlimScript;

internal class Tonumber : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var var = Variable.Create(line.ToArray()[1..], chunk);

        switch (var.Token.Type)
        {
            case TokenType.Number:
            case TokenType.Bool:
            case TokenType.Word:
                break;

            default:
                chunk.Error(
                    $"Cannot convert type {var.Token} to <Number>",
                    ExitCode.DisordantTokenError
                );
                break;
        }

        try
        {
            return new Number(new($"{Convert.ToDouble(var.Value.ToString()?.Replace(".", ","))}"));
        }
        catch (Exception e)
        {
            chunk.Error($"Cannot convert \"{var.GetString()}\" to Number. {e.Message}", ExitCode.RuntimeError);
            return new Null();
        }
    }
}
