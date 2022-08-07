namespace SlimScript;

internal class Func : Standart
{
    private static List<List<Token>>? _line;
    private static string _name = "";
    private static int _currentLevel = 0;
    private static string[] _params = { };

    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        if (_line == null)
        {
            _line = new();
            _line.Add(new());
        }

        var variable = new Token("someRandomToken");
        int i = 0;

        if (chunk.Parser.block.level == 0)
        {
            if (line.IndexOf(new("begin")) == -1)
                chunk.Error(
                    $"Cannot find keyword 'begin' to start block in line.",
                    ExitCode.GrammarError
                );

            variable = line[1];
            _name = variable.Text;
            chunk.Parser.block = (chunk.Parser.block.level + 1, "Func");
            _currentLevel = chunk.Parser.block.level;

            _params = line.ToArray()[2..line.IndexOf(new("begin"))].Select(t => t.Text).ToArray();

            i = line.IndexOf(new("begin")) + 1;
        }

        if (variable.Type != TokenType.Identifier)
            chunk.Error(
                $"Cannot create function with non identifier token '{variable.Text}'",
                ExitCode.DisordantTokenError
            );

        for (; i < line.Count; i++)
        {
            Token token = line[i];

            if (token.Text == "begin" || token.Text == "if")
                chunk.Parser.block.level++;

            if (token.Text == "end")
            {
                chunk.Parser.block.level--;

                if (chunk.Parser.block.level == _currentLevel - 1)
                    return Create(chunk);
            }

            _line.Last().Add(token);
        }

        _line.Add(new());
        return new Number();
    }

    private static IVariable Create(SourceChunk parentChunk)
    {
        _line = _line?.Where(l => l.Count != 0).ToList();

        SourceChunk chunk = new(_line?.Select(s => s).ToList() ?? new() { new() }, parentChunk);
        Function func = new(chunk, parentChunk, _params, _params.Length, _name);

        parentChunk.CreateVar(_name, func);

        _line?.Clear();
        _line = null;
        _name = "";
        _currentLevel = 0;
        _params = System.Array.Empty<string>();

        return parentChunk.GetVar(func.Name) ?? new Null();
    }
}
