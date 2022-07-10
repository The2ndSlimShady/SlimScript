namespace SlimScript;

internal class Func : Standart
{
    private static List<List<Token>>? _line;
    private static string _name = "";

    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {

        if (_line == null)
        {
            _line = new();
            _line.Add(new());
        }

        var variable = new Token(" ");

        if (!chunk.Parser.function)
        {
            variable = line[1];
            _name = variable.Text;
            chunk.Parser.function = true;
        }

        if (variable.Type != TokenType.Identifier)
            chunk.Error(
                $"Cannot create function with non identifier token '{variable.Text}'",
                ExitCode.DisordantTokenError
            );

        int i = line[0].Text == "func" ? 3 : 0;

        for (; i < line.Count; i++)
        {
            Token token = line[i];

            if (token.Text == "end")
            {
                chunk.Parser.function = false;
                return Create(chunk);
            }

            _line.Last().Add(token);
        }

        _line.Add(new());
        return new Number();
    }

    private static IVariable Create(SourceChunk parentChunk)
    {
        _line = _line.Where(l => l.Count != 0).ToList();

        SourceChunk chunk = new(_line.Select(s => s).ToList(), parentChunk);
        Function func = new(chunk) { Name = _name };

        parentChunk.CreateVar(_name, func);

        _line.Clear();
        _line = null;
        _name = "";

        return parentChunk.GetVar(func.Name);
    }
}
