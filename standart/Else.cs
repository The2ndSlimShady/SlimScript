namespace SlimScript;

internal class Else : Standart
{
    private static List<List<Token>>? _line;
    private static int _currentLevel = 0;
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

            chunk.Parser.block = (chunk.Parser.block.level, "Else");
            _currentLevel = chunk.Parser.block.level;

            i = 1;
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
            else if (
                chunk.Parser.block.level == _currentLevel && token.Text == "if"
                || token.Text == "elif"
            )
                chunk.Error(
                    $"'{token.Text}' block cannot be placed after else block.",
                    ExitCode.GrammarError
                );

            _line.Last().Add(token);
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
        _entered = false;

        var lineNum = parentChunk.Parser.lineNumber - chunk.Lines.Count - 1;
        var result = chunk.Run(lineNum);

        if (chunk.Parser.turn)
            parentChunk.Return();

        return result;
    }
}
