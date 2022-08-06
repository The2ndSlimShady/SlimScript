using System.Text;
using System.Xml.Schema;
using System.Text.Json;

namespace SlimScript;

public class GlobalSettings
{
    public static readonly string SystemFilesPath =
        $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SlimScript\\";

    public static string GetPathToSystemFiles(string path) => Path.Combine(SystemFilesPath, path);

    public static void ChangeStandartOutput(TextWriter writer) => Write.StandartOutput = writer;

    public static void ChangeStandartInput(TextReader reader) => Input.StandartInput = reader;

#if !DEBUG
    public static readonly AppInfo AppInfo = JsonSerializer.Deserialize<AppInfo>(
        File.ReadAllText(GetPathToSystemFiles("appinfo.json"))
    );
#else
    public static readonly AppInfo AppInfo = JsonSerializer.Deserialize<AppInfo>(
        File.ReadAllText("appinfo.json")
    );
#endif

    public static readonly VersionNumber Version = AppInfo.VersionNumber;

    public static readonly Dictionary<string, string> Contributors = AppInfo.Contributors;
}

public struct VersionNumber
{
    public int MajorVersion { get; set; }
    public int MinorVersion { get; set; }
    public int PatchVersion { get; set; }

    public override string ToString() => $"{MajorVersion}.{MinorVersion}.{PatchVersion}";
}

public struct AppInfo
{
    public VersionNumber VersionNumber { get; set; }
    public string AppName { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// Key is name and Value is github profile link.
    /// </summary>
    /// <value></value>
    public Dictionary<string, string> Contributors { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"AppName: {AppName}");
        sb.AppendLine($"Description: {Description}");
        sb.AppendLine($"Version: {VersionNumber}");
        sb.AppendLine($"Contributors:");

        foreach (var contributor in Contributors)
            sb.AppendLine($"\t{contributor.Key} - {contributor.Value}");

        return sb.ToString();
    }
}
