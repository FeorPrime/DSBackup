using static System.IO.Compression.ZipFile;

namespace DS2Backup;

public sealed class BackupOperations
{
    public void Backup()
    {
        var zipFile = Path.Combine(Constants.DS2BackupsDirectory, DateTime.Now.ToString("dd-MM-yyyy-HH.mm.ss") + ".zip");

        if (File.Exists(zipFile)) { File.Delete(zipFile); }

        if (!Directory.Exists(Constants.DS2BackupsDirectory)) { Directory.CreateDirectory(Constants.DS2BackupsDirectory); }

        using var zipStream = new FileStream(zipFile, FileMode.Create);
        CreateFromDirectory(Constants.DS2SavesDirectory, zipStream);

        $"Successfully created backup: '{zipFile}'".PrintSuccess();
    }

    public void List()
    {
        var backupsDirectoryInfo = new DirectoryInfo(Constants.DS2BackupsDirectory).GetFiles().Select(f => f.Name).ToList();
        backupsDirectoryInfo.ForEach(f => f.PrintInfo());
    }

    public void Restore(string backupName)
    {
        var zipFile = Path.Combine(Constants.DS2BackupsDirectory, backupName);

        if (!File.Exists(zipFile))
        {
            $"Can't find '{backupName}' in '{Constants.DS2BackupsDirectory}' directory!".PrintError();
            return;
        }

        using var zipStream = new FileStream(zipFile, FileMode.Open);
        ExtractToDirectory(zipStream, Constants.DS2SavesDirectory, overwriteFiles: true);
        
        $"Backup '{zipFile}' successfully restored into '{Constants.DS2SavesDirectory}' directory!".PrintSuccess();
    }
}