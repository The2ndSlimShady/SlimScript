using Ceras;

using System.Reflection;
using System.Diagnostics;
using System.Text;
using System;
using System.IO;

namespace SlimScript;

internal class Program
{
    public static bool Debug = false;
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

                if (Path.GetExtension(args[0]) == ".ssbo" || args.Contains("-RB"))
                {
                    byte[] data = File.ReadAllBytes(args[0]);

                    var deserializer = GetSerializer();

                    MainChunk = deserializer.Deserialize<SourceChunk>(data);
                }
                else
                {
                    if (args.Contains("-D"))
                        Debug = true;

                    BasePath = Directory.GetCurrentDirectory();

                    MainChunk = new SourceChunk(args[0]);

                    Directory.SetCurrentDirectory(BasePath);

                    if (args.Contains("-BC"))
                        CompileByte();
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

    private static void CompileByte()
    {
        try
        {
            Directory.SetCurrentDirectory($"{BasePath}\\{Path.GetDirectoryName(MainChunk._file)}");

            Console.WriteLine("Starting byte-compile...");

            byte[]? data = null;

            GetSerializer().Serialize(MainChunk, ref data);

            File.WriteAllBytes($"{Path.GetFileNameWithoutExtension(MainChunk._file)}.ssbo", data);

            Console.WriteLine(
                $"Successfully Compiled {MainChunk._file} -> {Path.GetFileNameWithoutExtension(MainChunk._file)}.ssbo"
            );

            Exit(ExitCode.Normal);
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"An Exception Occured During Byte-Compilation.\nMessage: {e.Message}"
            );

            File.Delete($"{Path.GetFileNameWithoutExtension(MainChunk._file)}.ssbo");

            Exit(ExitCode.CompilationError);
        }
    }

    private static CerasSerializer GetSerializer()
    {
        var config = new SerializerConfig();
        config.ConfigType<SourceChunk>();

        Type[] types = new[]
        {
            typeof(SourceChunk),
            typeof(Parser),
            typeof(Token),
            typeof(IVariable),
            typeof(TokenType)
        };

        config.KnownTypes.AddRange(types);

        config
            .ConfigType<SourceChunk>()
            .ConfigProperty("Stack")
            .Include()
            .ConfigProperty("Parent")
            .Include()
            .ConfigProperty("Lines")
            .Include()
            .ConfigProperty("Parser")
            .Include()
            .ConfigField("_file")
            .Include();

        config
            .ConfigType<Parser>()
            .ConfigField("lineNumber")
            .Exclude()
            .ConfigField("turn")
            .Exclude()
            .ConfigField("block")
            .Exclude()
            .ConfigProperty("Chunk")
            .Include()
            // .ConstructBy(typeof(Parser).GetConstructor(new[] { typeof(SourceChunk) }));
            ;

        config
            .ConfigType<Token>()
            .ConfigProperty("Type")
            .Include()
            .ConfigProperty("Text")
            .Include();

        config
            .ConfigType<IVariable>()
            .ConfigProperty("Token")
            .Include()
            .ConfigProperty("Name")
            .Include()
            .ConfigProperty("Value")
            .Include();

        return new CerasSerializer(config);
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
