namespace SlimScript;

internal class Foreach : Standart
{
    private static List<List<Token>>? _line;
    private static int _currentLevel = 0;
    private static (string name, IVariable array) _loopData = ("", new Array());

    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        if (_line == null)
            _line = new() { new() };

        int i = 0;

        if (chunk.Parser.block.level == 0)
        {
            chunk.Parser.turn = false;

            if (line.IndexOf(new("begin")) == -1)
                chunk.Error($"Cannot find keyword 'begin' to start block.", ExitCode.GrammarError);

            chunk.Parser.block = (chunk.Parser.block.level + 1, "Foreach");
            _currentLevel = chunk.Parser.block.level;

            Token[] nameTokens = line.ToArray()[1..line.IndexOf(new("in"))];

            if (nameTokens.Length != 1)
                chunk.Error($"Cannot create multiple loop variables.", ExitCode.GrammarError);

            _loopData.name = nameTokens[0].Text;

            var arr = Variable.Create(line.ToArray()[3..], chunk);

            if (arr.GetType() != typeof(Array) && arr.GetType() != typeof(Word))
                chunk.Error(
                    $"Cannot use type '{arr.Token}' for foreach loop.",
                    ExitCode.DisordantTokenError
                );

            _loopData.array = arr;

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
        _line = _line.Where(l => l.Count != 0).ToList();
        IVariable result = new Word(new("\"null\""));
        bool ret = false;

        if (_loopData.array.Token.Type == TokenType.Array)
        {
            foreach (IVariable var in ((Array)_loopData.array).Val)
            {
                SourceChunk chunk = new(_line.Select(s => s).ToList(), parentChunk);

                var lineNum = parentChunk.Parser.lineNumber - chunk.Lines.Count - 1;

                chunk.Parser.lineNumber = lineNum;

                chunk.CreateVar(_loopData.name, var);

                result = chunk.Run(lineNum);

                if (chunk.Parser.turn)
                {
                    ret = true;
                    break;
                }
            }
        }
        else
        {
            foreach (char ch in ((Word)_loopData.array).Val)
            {
                SourceChunk chunk = new(_line.Select(s => s).ToList(), parentChunk);

                var lineNum = parentChunk.Parser.lineNumber - chunk.Lines.Count - 1;

                chunk.Parser.lineNumber = lineNum;

                chunk.CreateVar(_loopData.name, new Word(new(ch.ToString())));

                result = chunk.Run(lineNum);

                if (chunk.Parser.turn)
                {
                    ret = true;
                    break;
                }
            }
        }

        _line.Clear();
        _line = null;
        _currentLevel = 0;
        _loopData = ("", new Array());

        if (ret)
            parentChunk.Return();

        return result;
    }
}
