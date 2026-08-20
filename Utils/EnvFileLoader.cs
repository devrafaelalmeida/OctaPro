namespace OctaPro.Utils;

public static class EnvFileLoader
{
    public static void Load(string fileName = ".env")
    {
        var path = FindFile(Directory.GetCurrentDirectory(), fileName);

        if (path is null)
            return;

        foreach (var rawLine in File.ReadAllLines(path))
        {
            var line = rawLine.Trim();

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                continue;

            var separatorIndex = line.IndexOf('=');

            if (separatorIndex <= 0)
                continue;

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim().Trim('"', '\'');

            if (string.IsNullOrWhiteSpace(key) || Environment.GetEnvironmentVariable(key) is not null)
                continue;

            Environment.SetEnvironmentVariable(key, value);
        }
    }

    private static string? FindFile(string startDirectory, string fileName)
    {
        var directory = new DirectoryInfo(startDirectory);

        while (directory is not null)
        {
            var path = Path.Combine(directory.FullName, fileName);

            if (File.Exists(path))
                return path;

            directory = directory.Parent;
        }

        return null;
    }
}
