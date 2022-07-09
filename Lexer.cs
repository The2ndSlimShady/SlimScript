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

            for (int i = 0; i < source.Length; )
            {
                string element = source[i];

                if (element == "EOL")
                    addLine();

                while (true)
                {
                    element = source[i];

                    Token token = new(element);

                    if (token.Type == TokenType.EOL)
                    {
                        i++;
                        addLine();
                        break;
                    }

                    Lines[_line - 1].Add(token);
                    i++;
                }
            }

            Lines = Lines.Where(line => line.Count != 0).ToList();

#if DEBUG
            StringBuilder sb = new();
#endif
            StringBuilder humanized = new();
            bool after = false;

            foreach (var tokens in Lines)
            {
                foreach (var token in tokens)
                {
#if DEBUG
                    sb.Append($"[{token}: {token.Text}]");
#endif
                    humanized.Append($"{(after ? " " : "")}{token.Text}");
                    after = true;
                }

#if DEBUG
                sb.AppendLine();
#endif
                humanized.AppendLine();
                after = false;
            }

#if DEBUG
            File.WriteAllText("post_lexer.ss", sb.ToString());
#endif
            File.WriteAllText("post_lexer_humanized.ss", humanized.ToString());

            return Lines;
        }
        catch (Exception)
        {
            Program.Exit(ExitCode.LexerError);
            return null;
        }
    }
}
