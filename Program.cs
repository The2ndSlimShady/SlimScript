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

        MainChunk = new SourceChunk(File.ReadAllLines(args[0]));

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
