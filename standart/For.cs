namespace SlimScript;

internal class For : Standart
{
    private static List<List<Token>>? _line;
    private static int _currentLevel = 0;
    private static (Token[] name, Token[] condition, Token[] action) _loopData = (
        name: System.Array.Empty<Token>(),
        condition: System.Array.Empty<Token>(),
        action: System.Array.Empty<Token>()
    );

    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        if (_line == null)
            _line = new() { new() };

        int i = 0;

        if (chunk.Parser.block.level == 0)
        {
            chunk.Parser.turn = false;

            if (line.LastIndexOf(new("begin")) == -1)
                chunk.Error($"Cannot find keyword 'begin' to start block.", ExitCode.GrammarError);

            chunk.Parser.block = (chunk.Parser.block.level + 1, "For");
            _currentLevel = chunk.Parser.block.level;

            List<List<Token>> tokens = new() { new() };
            line = line.GetRange(1, line.LastIndexOf(new("begin")) - 1);

            foreach (var item in line)
            {
                if (item.Text == "||")
                {
                    tokens.Add(new());
                    continue;
                }

                if (item.Text.Contains("||"))
                {
                    tokens
                        .Last()
                        .Add(new(item.Text.Replace(",", string.Empty).Replace("||", string.Empty)));
                    tokens.Add(new());
                    continue;
                }

                tokens.Last().Add(item);
            }

            if (tokens.Count < 3)
                chunk.Error($"Cannot create for loop from given arguments.", ExitCode.GrammarError);

            _loopData.name = tokens[0].ToArray();

            _loopData.condition = tokens[1].ToArray();
            _loopData.action = tokens[2].ToArray();

            i = line.Count;
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
		bool inLoopVar = false;
		_loopData.name = _loopData.name.Prepend(new("define")).ToArray();
		
		if (_loopData.name.Contains(new Token("as")))
        {
			inLoopVar = true;
			Variable.Create(_loopData.name, parentChunk);
		}
		else if (!parentChunk.VarExists(_loopData.name[1].Text))
			parentChunk.Error(
				$"No variable named '{_loopData.name[1].Text}' found.", 
				ExitCode.NullReferenceError
			);
		
		
        var condt = Variable.Create(_loopData.condition, parentChunk);

        if (condt.Token.Type != TokenType.Bool)
            parentChunk.Error(
                $"Cannot use type '{condt.Token}' for for loop.",
                ExitCode.DisordantTokenError
            );

        for (; (condt as Bool?)?.Val ?? false; )
        {
            SourceChunk chunk =
                new(_line?.ToList() ?? new() { new() }, parentChunk) { ChunkType = ChunkType.Loop };

            var lineNum = parentChunk.Parser.lineNumber - chunk.Lines.Count - 1;
            result = chunk.Run(lineNum);

            if (chunk.Parser.turn)
            {
                ret = true;
                break;
            }

            if ((result as Word?)?.Val == "break")
                break;

            chunk = new(
                new List<List<Token>>()
                {
                    new()
                    {
                        new("set"),
                        _loopData.name[1],
                        new("to"),
                        new("+"),
                        _loopData.name[1],
                        _loopData.action[0]
                    }
                },
                parentChunk
            );

            chunk.Run();

            condt = Variable.Create(_loopData.condition, parentChunk);
        }

        if (inLoopVar)
		{
			_loopData.name = _loopData.name.Skip(1).Take(1).Prepend(new("delete")).ToArray();
			Variable.Create(_loopData.name, parentChunk);
		}

        _line?.Clear();
        _line = null;
        _currentLevel = 0;
        _loopData = (
            name: System.Array.Empty<Token>(),
            condition: System.Array.Empty<Token>(),
            action: System.Array.Empty<Token>()
        );

        if (ret)
            parentChunk.Return();

        return result;
    }
}
