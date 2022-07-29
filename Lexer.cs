using System;
using System.Text;

namespace SlimScript;

internal class Lexer
{
    public static List<List<Token>> Lex(string[] source)
    {
        #if DEBUG
        Console.WriteLine($"Lexing Source Code...");
        #endif

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

            if (!Program.interactive && Program.Debug)
            {
                StringBuilder sb = new();
                StringBuilder? humanized = null;

                if (Program.Humanize)
                    humanized = new();
                
                bool first = false;

                foreach (var tokens in Lines)
                {
                    first = true;

                    foreach (var token in tokens)
                    {
                        sb.Append($"[{token}: {token.Text}]$");

                        humanized?.Append($"{(first ? string.Empty : " ")}{token.Text}");

                        first = false;                   
                    }

                    sb.AppendLine();

                    humanized?.AppendLine();
                }

                File.WriteAllText("post_lexer.sso", sb.ToString());

                if (Program.Humanize)
                    File.WriteAllText("post_lexer_humanized.sso", humanized?.ToString());
            }

            return Lines;
        }
        catch (Exception)
        {
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
