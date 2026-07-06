using System.Reflection;
using System.Text.Json;

static class Config
{
    public static string CommandUrl { get; }
    public static string UploadUrl { get; }
    public static int IntervalSeconds { get; }

    static Config()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "RemoteCommandClient.config.json";

        using Stream stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new Exception("Embedded config.json not found");

        using StreamReader reader = new(stream);
        string json = reader.ReadToEnd();

        var doc = JsonDocument.Parse(json);

        CommandUrl = doc.RootElement.GetProperty("CommandUrl").GetString()!;
        UploadUrl = doc.RootElement.GetProperty("UploadUrl").GetString()!;
        IntervalSeconds = doc.RootElement.GetProperty("IntervalSeconds").GetInt32();
    }
}
