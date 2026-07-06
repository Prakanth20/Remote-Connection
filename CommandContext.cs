using System;
using System.IO;

static class CommandContext
{
    public static string CurrentDirectory { get; private set; }
        = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    public static bool HandleCd(string command)
    {
        if (!command.StartsWith("cd ", StringComparison.OrdinalIgnoreCase))
            return false;

        string target = command.Substring(3).Trim();

        string newPath = Path.IsPathRooted(target)
            ? target
            : Path.GetFullPath(Path.Combine(CurrentDirectory, target));

        if (Directory.Exists(newPath))
        {
            CurrentDirectory = newPath;
            return true;
        }

        throw new DirectoryNotFoundException($"Directory not found: {newPath}");
    }
}
