namespace DS2Backup;

public static class Constants
{
    public static readonly string DS2SavesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DarkSoulsII");
    public static readonly string DS2BackupsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "DarkSoulsIIBackups");
}