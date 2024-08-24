using System.Data.SqlTypes;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using SlimScript;

internal class Program
{
    public static bool Debug = false;
    public static bool CompressStandalone = false;

    public static string? BasePath { get; set; }

    public static ExitCode ExitCode { get; set; }
    public static SourceChunk? MainChunk { get; set; }
    public static bool interactive = false;

    private static readonly Stopwatch watch = new();

    public static void Main(string[] args)
    {	
         try
         {
            if (args.Length != 0 && args[0] == "-i")
                RunInteractive();
            else if (args.Length != 0 && (args[0] == "-h" || args[0] == "--help"))
                PrintHelp();
            else
            {
                if (args.Length == 0 || !File.Exists(args[0]))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Write.StandartOutput.WriteLine("No Input File...");

                    Exit(ExitCode.NoInputFile);
                }

                BasePath = Directory.GetCurrentDirectory();

                Debug = args.Contains("-D");
                CompressStandalone = args.Contains("-C");

                watch.Start();
                MainChunk = new(args[0]);

                Directory.SetCurrentDirectory(BasePath);

                var indexOfArgsBegin = System.Array.IndexOf(args, "%p");
                indexOfArgsBegin = indexOfArgsBegin == -1 ? -2 : indexOfArgsBegin;

                try
                {
                    MainChunk.CreateVar("os.args", Variable.ClrToVar(args[(indexOfArgsBegin + 1)..]));
                    MainChunk.CreateVar(
                        "os.argc",
                        Variable.ClrToVar(args.Length - (indexOfArgsBegin + 1))
                    );
                }
                catch (IndexOutOfRangeException)
                {
                    MainChunk.CreateVar("os.args", new Null());
                    MainChunk.CreateVar("os.argc", Variable.ClrToVar(0));
                }
                catch (ArgumentOutOfRangeException)
                {
                    MainChunk.CreateVar("os.args", new Null());
                    MainChunk.CreateVar("os.argc", Variable.ClrToVar(0));
                }

                MainChunk.Run();

                if (MainChunk.VarExists("main"))
                {
                    ((Function?)MainChunk.GetVar("main"))?.Run(
                        MainChunk.GetVar("os.args") ?? new Null(),
                        MainChunk.GetVar("os.argc") ?? Variable.ClrToVar(0)
                    );
                }

                watch.Stop();
                Write.StandartOutput.WriteLine($"\nProgram Exited in {watch.ElapsedMilliseconds}ms");
                Exit(ExitCode.Normal);
            }
        }
        catch (Exception e)
        {
            var line = MainChunk?.Lines[MainChunk.Parser.lineNumber - 1];
            var message =
                $"An Exception in source code occured during runtime.\nMessage: {e.Message}\nSource:\n{e.StackTrace}\nFile: {Path.GetFileNameWithoutExtension(MainChunk?._file)}_p.sso\nLine: {MainChunk?.Parser.lineNumber}\nExpression: {string.Join(' ', line?.Select(t => t.Text) ?? new[] { "" })}";

            Console.ForegroundColor = ConsoleColor.Red;
            Write.StandartOutput.WriteLine(message);

            Exit(ExitCode.RuntimeError);
        }
    }

    private static void PrintHelp()
    {
        StringBuilder sb = new();
        sb.Append($"\n{GlobalSettings.AppInfo}\n");
        sb.AppendLine("Usage:");
        sb.AppendLine("\tSlimScript <file>");
        sb.AppendLine("\tSlimScript <file> <flags>");
        sb.AppendLine("\tSlimScript <file> <flags> %p <arguments>");
        sb.AppendLine("\tSlimScript -i");
        sb.AppendLine("\tSlimScript -h --help");

        sb.AppendLine("\nAvailable Flags:");
        sb.AppendLine(
            "\t-D              Run With Debug Mode. Generates lexer output (for curious ones). (SlimScript <file> -D)"
        );
        sb.AppendLine(
            "\t-C              Generates Compressed Standalone Script File That Has No\n\t\t\tDependencies On Any Other File.                                 (SlimScript <file> -C)"
        );
        sb.AppendLine(
            "\t-i              Run Interactive Mode                                            (SlimScript -i)"
        );
        sb.AppendLine(
            "\t-h --help       Show This Output.                                               (SlimScript -h --help)"
        );

        sb.AppendLine("\n\tPassing Arguments:");
        sb.AppendLine(
            "\t\t\tSlimScript <file> <flags> %p <arguments>       Arguments are passed to main function as an array."
        );
        sb.AppendLine();

        Write.StandartOutput.Write(sb);
    }

    private static void RunInteractive()
    {
        interactive = true;

        int line = 0;

        MainChunk = new();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Write.StandartOutput.Write("\n>>> ");

            Console.ResetColor();
            string input = Console.ReadLine() ?? "";

            if (string.IsNullOrEmpty(input))
                continue;

            if (input == "qqq")
                break;

            MainChunk.RunInteractive(input);

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
        Write.StandartOutput.WriteLine($"\nExit Code: {(int)code} <{code}>");

        Write.StandartOutput.Flush();
        Write.StandartOutput.Close();

        Environment.Exit((int)code);
    }
}
