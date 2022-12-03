using System.Text;

namespace SlimScript;

internal class Lexer
{
    public static List<List<Token>> Lex(string[] source, SourceChunk chunk)
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

                Lines[_line -1].Add(token);
            }

            Lines = Lines.Where(line => line.Count != 0).ToList();

            if (!Program.interactive)
            {
                StringBuilder? sb = null;

                if (Program.Debug)
                    sb = new();

                StringBuilder? compressed = null;

                if (Program.CompressStandalone)
                    compressed = new();

                bool first = false;

                if (sb == null && compressed == null)
                    return Lines;

                foreach (var tokens in Lines)
                {
                    first = true;

                    foreach (var token in tokens)
                    {
                        sb?.Append($"[{token}: {token.Text}] ");

                        compressed?.Append($"{(first ? string.Empty : " ")}{token.Text}");

                        first = false;
                    }

                    sb?.AppendLine();

                    compressed?.AppendLine();
                }

                if (Program.Debug && (Path.GetFileNameWithoutExtension(chunk._file) != "???"))
                    File.WriteAllText(
                        $"{Path.GetFileNameWithoutExtension(chunk._file) ?? string.Empty}_l.sso",
                        sb?.ToString()
                    );

                if (Program.CompressStandalone)
                    File.WriteAllBytes(
                        $"{Path.GetFileNameWithoutExtension(chunk._file) ?? string.Empty}.csso",
                        Program.Compress(Encoding.UTF8.GetBytes(compressed?.ToString() ?? ""))
                    );

                if (Program.CompressStandalone)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Write.StandartOutput.WriteLine(
                        $"Successfully created and compressed standalone script file {Path.GetFileNameWithoutExtension(chunk._file) ?? string.Empty}.csso"
                    );

                    Program.Exit(ExitCode.Normal);
                }
            }

            return Lines;
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Write.StandartOutput.WriteLine(
                $"An Error occured during Lexical Analysis.\nMessage:{e.Message}"
            );
            Program.Exit(ExitCode.LexerError);
            return new();
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
