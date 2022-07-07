using System.Text;
using System;
using System.IO;

namespace SlimScript;

internal class Program
{
    public static ExitCode ExitCode { get; set; }
    public static SourceChunk MainChunk { get; set; }

    public static void Main(string[] args)
    {
        if (args.Length == 0 || (args[0] != "-i" && !File.Exists(args[0])))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Input File...");

            Exit(ExitCode.NoInputFile);
        }

        if (args[0] == "-i")
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\n>>> ");
                Console.ResetColor();
                string? source = Console.ReadLine();

                List<Token> tokenList = new();

                foreach (string expression in source?.Split(" "))
                    tokenList.Add(new Token(expression));

                foreach (Token token in tokenList)
                    Console.Write($" [{token}: {token.Text}]");
            }
        }
        else
        {
            string[] source = File.ReadAllLines(args[0]);
            string[] processedSource = PreProcessor.Process(source);
            
            #if DEBUG
            StringBuilder b = new StringBuilder();
            foreach (var item in processedSource)
            {
                if (item != "EOL")
                    b.Append($" {item}");
                else
                    b.Append(Environment.NewLine);
            }

            File.WriteAllText("post_process.ss", b.ToString());
            #endif

            MainChunk = new SourceChunk(processedSource);

            Exit(ExitCode.Normal);
        }
    }

    public static void Exit(ExitCode code)
    {
        ExitCode = code;

        Console.ResetColor();
        Console.WriteLine($"\nExit Code: {(int)code} <{code}>");
        Environment.Exit((int)code);
    }
}
