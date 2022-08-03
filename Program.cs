using System.Reflection;
using System.Diagnostics;
using System.Text;
using System;
using System.IO;
using System.IO.Compression;

namespace SlimScript;

internal class Program
{
    public static bool Debug = false;
    public static bool CompressStandalone = false;

    public static string BasePath { get; set; }

    public static ExitCode ExitCode { get; set; }
    public static SourceChunk MainChunk { get; set; }
    public static bool interactive = false;

    private static Stopwatch watch = new();

    public static void Main(string[] args)
    {
        watch.Start();

        // try
        // {
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

            BasePath = Directory.GetCurrentDirectory();

            Debug = args.Contains("-D");
            CompressStandalone = args.Contains("-C");

            MainChunk = new(args[0]);

            Directory.SetCurrentDirectory(BasePath);

            MainChunk.Run();

            watch.Stop();
            Console.WriteLine($"\nProgram Exited in {watch.ElapsedMilliseconds}ms");
            Exit(ExitCode.Normal);
        }
        // }
        // catch (Exception e)
        // {
        // var line = MainChunk.Lines[MainChunk.Parser.lineNumber - 1];
        // var message =
        //     $"An Exception occured during runtime.\nMessage: {e.Message}\nFile: {Path.GetFileNameWithoutExtension(MainChunk._file)}_p.sso\nLine: {MainChunk.Parser.lineNumber}\nExpression: {string.Join(' ', line.Select(t => t.Text))}";

        // Console.ForegroundColor = ConsoleColor.Red;
        // Console.WriteLine(message);

        // Exit(ExitCode.RuntimeError);

        //     throw e;
        // }
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

    public static byte[] Compress(byte[] data)
    {
        using var compressedStream = new MemoryStream();
        using var zipStream = new GZipStream(compressedStream, CompressionMode.Compress);
        zipStream.Write(data, 0, data.Length);
        zipStream.Close();
        return compressedStream.ToArray();
    }

    public static byte[] Decompress(byte[] data)
    {
        using var compressedStream = new MemoryStream(data);
        using var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var resultStream = new MemoryStream();
        zipStream.CopyTo(resultStream);
        return resultStream.ToArray();
    }

    public static void Exit(ExitCode code)
    {
        if (interactive)
            return;

        ExitCode = code;

        Console.ResetColor();
        Console.WriteLine($"\nExit Code: {(int)code} <{code}>");
        Environment.Exit((int)code);
    }
}
