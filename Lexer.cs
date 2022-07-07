using System.Text;

namespace SlimScript;

internal class Lexer
{


    public static List<List<Token>> Lex(string[] source)
    {
        int _line = 0;
        List<List<Token>> Lines = new();

        void addLine()
        {
            _line++;
            Lines.Add(new());
        }

        Lines = new();

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
        foreach (var tokens in Lines)
        {
            foreach (var token in tokens)
                sb.Append($"[{token}: {token.Text}]");
            sb.AppendLine();
        }

        File.WriteAllText("post_lexer.ss", sb.ToString());
#endif

        return Lines;
    }
}
