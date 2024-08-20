namespace SlimScript;

internal class Elif : Standart
{
    private static List<List<Token>>? _line;
    private static int _currentLevel = 0;
    private static bool _condition = false;
    private static bool _entered = false;

    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        if (_line == null)
            _line = new() { new() };

        int i = 0;

        if (chunk.Parser.block.level == 0)
            chunk.Error($"Cannot start if-else block with elif.", ExitCode.GrammarError);
        else if (!_entered)
        {
            _entered = true;

            if (!line.Contains(new("then")))
                chunk.Error($"Cannot find keyword 'then' to start block.", ExitCode.GrammarError);

            chunk.Parser.block = (chunk.Parser.block.level, "Elif");
            _currentLevel = chunk.Parser.block.level;

            var conditionTokens = line.ToArray()[1..line.IndexOf(new("then"))];
            var condition = Variable.Create(conditionTokens, chunk);

            if (condition.Token.Type != TokenType.Bool)
                chunk.Error(
                    $"Cannot evaluate boolean comprasion on type '{condition}'.",
                    ExitCode.DisordantTokenError
                );

            if (!(condition as Bool?)?.Val ?? false)
                return new Bool(new("false"));

            _condition = true;

            i = line.IndexOf(new("then")) + 1;
        }

        for (; i < line.Count; i++)
        {
            Token token = line[i];

            if (_condition)
            {
                if (token.Text == "begin" || token.Text == "if")
                    chunk.Parser.block.level++;

                if (token.Text == "end" || token.Text == "elif" || token.Text == "else")
                {
                    if (token.Text == "end")
                        chunk.Parser.block.level--;

                    if (chunk.Parser.block.level == _currentLevel - 1)
                        return Create(chunk);
                    else if (chunk.Parser.block.level == _currentLevel)
                    {
                        var result = Create(chunk);
                        _entered = true;
                        return result;
                    }
                }

                _line.Last().Add(token);
            }
            else
            {
                if (token.Text == "begin" || token.Text == "if")
                    chunk.Parser.block.level++;

                if (token.Text == "end")
                {
                    chunk.Parser.block.level--;

                    if (chunk.Parser.block.level == _currentLevel - 1)
                    {
                        _entered = false;
                        return new Bool(new("false"));
                    }
                }
                else if (chunk.Parser.block.level == _currentLevel)
                {
                    if (token.Text == "elif")
                    {
                        chunk.Parser.block = (chunk.Parser.block.level, "Elif");

                        _condition = false;
                        _line.Clear();
                        _line = null;
                        _currentLevel = chunk.Parser.block.level;
                        _entered = false;

                        return new Elif().Run(line, chunk);
                    }
                    else if (token.Text == "else")
                    {
                        chunk.Parser.block = (chunk.Parser.block.level, "Else");
                        return new Else().Run(line, chunk);
                    }
                }
            }
        }

        _line.Add(new());
        return new Bool(new("true"));
    }

    private static IVariable Create(SourceChunk parentChunk)
    {
        _line = _line?.Where(l => l.Count != 0).ToList();

        SourceChunk chunk = new(_line?.Select(s => s).ToList() ?? new() { new() }, parentChunk);

        _line?.Clear();
        _line = null;
        _currentLevel = 0;
        _condition = false;
        _entered = false;

        var lineNum = parentChunk.Parser.lineNumber - chunk.Lines.Count - 1;
        var result = chunk.Run(lineNum);

        if (chunk.Parser.turn)
            parentChunk.Return();

        return result;
    }
}
