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
        if (args.Length == 0 || !File.Exists(args[0]))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Input File...");

            Exit(ExitCode.NoInputFile);
        }
        string[] source = File.ReadAllLines(args[0]);
        string[] processedSource = PreProcessor.Process(source);

#if DEBUG
        StringBuilder b = new();
        bool isIndent = true;
        foreach (var item in processedSource)
        {
            if (item != "EOL")
            {
                b.Append($" {item}");
                isIndent = false;
            }
            else
            {
                b.Append(Environment.NewLine);
                isIndent = true;
            }
        }

        File.WriteAllText("post_process.ss", b.ToString());
#endif

        MainChunk = new SourceChunk(processedSource);

        MainChunk.Run();

        Exit(ExitCode.Normal);
    }

    public static void Exit(ExitCode code)
    {
        ExitCode = code;

        Console.ResetColor();
        Console.WriteLine($"\nExit Code: {(int)code} <{code}>");
        Environment.Exit((int)code);
    }
}
