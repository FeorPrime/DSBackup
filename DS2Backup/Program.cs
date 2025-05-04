using Cocona;
using DS2Backup;

var builder = CoconaApp.CreateBuilder();

var app = builder.Build();

app.AddCommands<BackupOperations>();

if (args.Length < 1 || args.Contains("-h") || args.Contains("--help") )
{
    "This is DS2:SOTFS saves backup manager.\n".PrintHeader();
    "THIS PROGRAM WILL OVERWRITE YOUR'S SAVES WITHOUT ANY PROMPTS!!! USE CAREFULLY!!! READ CODE FIRST!!!\n".PrintWarning();
}

await app.RunAsync();
