using Cocona;

namespace DS2Backup
{
    public class Program
    {
        private static readonly string SaveFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DarkSoulsII");
        private static readonly string BackupFolderPath = Path.Combine(SaveFolderPath, "Backups");

        public static void Main(string[] args)
        {
            CoconaApp.Run<Program>();
        }

        [Command("backup", Description = "Backup your Dark Souls II SOTFS save files.", Aliases = ["b"])]
        public static void BackupSave([Argument(Description = "Name of the backup checkpoint")] string checkpointName)
        {
            if (!Directory.Exists(SaveFolderPath))
            {
                "Error: Dark Souls II SOTFS save folder not found!".PrintError();
                return;
            }

            if (string.IsNullOrWhiteSpace(checkpointName))
            {
                "Backup name cannot be empty.".PrintError();
                return;
            }

            string backupCheckpointPath = Path.Combine(BackupFolderPath, checkpointName);

            if (Directory.Exists(backupCheckpointPath))
            {
                Console.Write($"A backup with the name '{checkpointName}' already exists. Overwrite? (y/n): ");
                string overwriteChoice = Console.ReadLine().Trim().ToLower();
                if (overwriteChoice != "y")
                {
                    "Backup aborted.".PrintWarning();
                    return;
                }
            }

            // Create the backup folder if it doesn't exist
            if (!Directory.Exists(BackupFolderPath))
            {
                Directory.CreateDirectory(BackupFolderPath);
            }

            // Copy all save files to the backup folder
            foreach (string file in Directory.GetFiles(SaveFolderPath, "*.sl2"))
            {
                string fileName = Path.GetFileName(file);
                string destinationPath = Path.Combine(backupCheckpointPath, fileName);

                Directory.CreateDirectory(backupCheckpointPath); // Ensure the folder exists
                File.Copy(file, destinationPath, true); // Overwrite if the file already exists
            }

            $"Backup successful! Checkpoint: {checkpointName}".PrintSuccess();
        }

        [Command("restore", Description = "Restore your Dark Souls II SOTFS save files from a backup.", Aliases = ["r"])]
        public static void RestoreSave()
        {
            if (!Directory.Exists(BackupFolderPath))
            {
                "No backups found!".PrintError();
                return;
            }

            Console.WriteLine("Available backups:");
            int index = 1;
            var checkpoints = Directory.GetDirectories(BackupFolderPath);
            foreach (var checkpoint in checkpoints)
            {
                Console.WriteLine($"{index++}. {Path.GetFileName(checkpoint)}");
            }

            Console.Write("Enter the number of the backup you want to restore: ");
            if (!int.TryParse(Console.ReadLine(), out int selected) || selected < 1 || selected > checkpoints.Length)
            {
                "Invalid selection.".PrintError();
                return;
            }

            string selectedCheckpointPath = checkpoints[selected - 1];

            Console.WriteLine($"Restoring from checkpoint: {Path.GetFileName(selectedCheckpointPath)}");

            foreach (string file in Directory.GetFiles(selectedCheckpointPath, "save*.sl2"))
            {
                string fileName = Path.GetFileName(file);
                string destinationPath = Path.Combine(SaveFolderPath, fileName);

                File.Copy(file, destinationPath, true); // Overwrite existing save files
            }

            "Restore complete!".PrintSuccess();
        }

        [Command("list", Description = "List all available backups.", Aliases = ["ls"])]
        public static void ListBackups()
        {
            if (!Directory.Exists(BackupFolderPath))
            {
                "No backups found!".PrintWarning();
                return;
            }

            Console.WriteLine("Available backups:");
            foreach (var checkpoint in Directory.GetDirectories(BackupFolderPath))
            {
                $"- {Path.GetFileName(checkpoint)}".PrintInfo();
            }
        }


    }
}