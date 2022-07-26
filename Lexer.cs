using System.Text;

namespace SlimScript;

internal class Lexer
{
    public static List<List<Token>> Lex(string[] source)
    {
        try
        {
            int _line = 0;
            List<List<Token>> Lines = new();

            void addLine()
            {
                _line++;
                Lines.Add(new());
            }

            addLine();

            for (int i = 0; i < source.Length; i++)
            {
                string element = source[i];

                Token token = new(element);

                if (token.Type == TokenType.EOL)
                {
                    addLine();
                    continue;
                }

                Lines[_line - 1].Add(token);
            }

            Lines = Lines.Where(line => line.Count != 0).ToList();

#if DEBUG
            if (!Program.interactive && Program.Debug)
            {
                StringBuilder sb = new();
                StringBuilder humanized = new();
                bool after = false;

                foreach (var tokens in Lines)
                {
                    foreach (var token in tokens)
                    {
                        sb.Append($"[{token}: {token.Text}]");
                        humanized.Append($"{(after ? " " : "")}{token.Text}");
                        after = true;
                    }
                    sb.AppendLine();
                    humanized.AppendLine();
                    after = false;
                }
                File.WriteAllText("post_lexer.ss", sb.ToString());
                File.WriteAllText("post_lexer_humanized.ss", humanized.ToString());
            }
#endif

            return Lines;
        }
        catch (Exception)
        {
            Program.Exit(ExitCode.LexerError);
            return null;
        }
    }

    public static List<Token> LexLine(string[] line)
    {
        List<Token> result = new();

        foreach (var item in line)
        {
            Token token = new(item);

            if (token.Type == TokenType.EOL)
                break;

            result.Add(token);
        }

        return result;
    }
}
