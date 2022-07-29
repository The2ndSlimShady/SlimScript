using System.Reflection;
using System.Diagnostics;
using System.Text;
using System;
using System.IO;

namespace SlimScript;

internal class Program
{
    public static bool Debug = false;
    public static bool Humanize = false;

    public static string BasePath { get; set; }

    public static ExitCode ExitCode { get; set; }
    public static SourceChunk MainChunk { get; set; }
    public static bool interactive = false;

    private static Stopwatch watch = new();

    public static void Main(string[] args)
    {
        watch.Start();

        try
        {
            if (args.Length != 0 && args[0] == "-i")
                RunInteractive();
            else
            {
                if (args.Length == 0 || !File.Exists(args[0]))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Input File...");

                    Exit(ExitCode.NoInputFile);
                }

                if (Path.GetExtension(args[0]) == ".sso" || args.Contains("-FF"))
                {
                    BasePath = Directory.GetCurrentDirectory();

                    MainChunk = new SourceChunk(args[0]);

                    Directory.SetCurrentDirectory(BasePath);
                }
                else
                {
                    Debug = args.Contains("-D") || args.Contains("-DH");
                    Humanize = args.Contains("-H") || args.Contains("-DH");

                    BasePath = Directory.GetCurrentDirectory();

                    MainChunk = new SourceChunk(args[0]);

                    Directory.SetCurrentDirectory(BasePath);
                }

                MainChunk.Run();

                Exit(ExitCode.Normal);
            }
        }
        catch (StackOverflowException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"An Error [StakcOverFlowError] Occured due to inifinite recursion. \nMessage: {e.Message}"
            );
            Exit(ExitCode.RuntimeError);
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"An Excepton Occured During Runtime.\nMessage: {e.Message}\nline {MainChunk.Parser.lineNumber}"
            );
            Exit(ExitCode.RuntimeError);
        }
    }

    private static void RunInteractive()
    {
        interactive = true;

        int line = 0;

        MainChunk = new();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\n>>> ");

            Console.ResetColor();
            string input = Console.ReadLine() ?? "";

            if (string.IsNullOrEmpty(input))
                continue;

            if (input == "qqq")
                break;

            MainChunk.RunInteractive(input, line);

            line++;
        }

        Exit(ExitCode.Normal);
    }

    public static void Exit(ExitCode code)
    {
        if (interactive)
            return;

        ExitCode = code;

        Console.ResetColor();
        Console.WriteLine($"\nProgram Exited in {watch.ElapsedMilliseconds}ms");
        Console.WriteLine($"Exit Code: {(int)code} <{code}>");
        Environment.Exit((int)code);
    }
}
