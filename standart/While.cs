namespace SlimScript;

internal class While : Standart
{
    private static List<List<Token>>? _line;
    private static int _currentLevel = 0;
    private static Token[] _conditionTokens = System.Array.Empty<Token>();

    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        if (_line == null)
            _line = new() { new() };

        int i = 0;

        if (chunk.Parser.block.level == 0)
        {
            if (line.IndexOf(new("begin")) == -1)
                chunk.Error($"Cannot find keyword 'begin' to start block.", ExitCode.GrammarError);

            chunk.Parser.block = (chunk.Parser.block.level + 1, "While");
            _currentLevel = chunk.Parser.block.level;

            _conditionTokens =
                line.ToArray()[1..line.IndexOf(new("begin"))] ?? System.Array.Empty<Token>();

            var condition = Variable.Create(_conditionTokens, chunk);

            if (condition.Token.Type != TokenType.Bool)
                chunk.Error(
                    $"Cannot evaluate boolean comprasion on type '{condition}'.",
                    ExitCode.DisordantTokenError
                );

            if (!(condition as Bool?)?.Val ?? false)
                return new Bool(new("false"));

            i = line.IndexOf(new("begin")) + 1;
        }

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
        return new Bool(new("true"));
    }

    private static IVariable Create(SourceChunk parentChunk)
    {
        _line = _line?.Where(l => l.Count != 0).ToList();
        IVariable result = new Null();
        bool ret = false;

        while (true)
        {
            var condition = (Bool)Variable.Create(_conditionTokens, parentChunk);

            if (!condition.Val)
                break;

            SourceChunk chunk =
                new(_line?.Select(s => s).ToList() ?? new List<List<Token>> { new() }, parentChunk)
                {
                    ChunkType = ChunkType.Loop
                };

            var lineNum = parentChunk.Parser.lineNumber - chunk.Lines.Count - 1;

            result = chunk.Run(lineNum);

            if (chunk.Parser.turn)
            {
                ret = true;
                break;
            }

            if ((result as Word?)?.Val == "break")
                break;
        }

        _line?.Clear();
        _line = null;
        _currentLevel = 0;
        _conditionTokens = System.Array.Empty<Token>();

        if (ret)
            parentChunk.Return();

        return result;
    }
}
