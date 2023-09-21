// See https://aka.ms/new-console-template for more information

if (args.Length == 0)
{
    Console.WriteLine("Please provide path to watch");
    return;
}

var watchedPath = args[0];
Console.WriteLine($"Watching Directory {watchedPath}");

var watcher = new FileSystemWatcher(watchedPath);
watcher.IncludeSubdirectories = true;
watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.LastAccess | NotifyFilters.FileName |
                       NotifyFilters.LastWrite;
// can do this to filter for certain types of files
// watcher.Filter = "*.txt";

watcher.Changed += OnChanged;
watcher.Created += OnChanged;
watcher.Deleted += OnChanged;
watcher.Renamed += OnRenamed;

watcher.EnableRaisingEvents = true;

while (true)
{
    // not needed for service, just console, since console will end without loop
    watcher.WaitForChanged(WatcherChangeTypes.All);
}

void OnChanged(object source, FileSystemEventArgs e)
{
    Console.WriteLine("OnChanged");
    Console.WriteLine($"File: {e.Name}");
    EnumerateFiles(watchedPath);
}

void OnRenamed(object source, RenamedEventArgs e)
{
    Console.WriteLine("OnRenamed");
    Console.WriteLine($"File: {e.Name}");
    EnumerateFiles(watchedPath);
}

static void EnumerateFiles(string path)
{
    var opts = new EnumerationOptions
    {
        RecurseSubdirectories = true,
        ReturnSpecialDirectories = false
    };
    var files = Directory.EnumerateFiles(path, "*", opts);
    Console.WriteLine("Enumerating files...");
    foreach (var file in files)
    {
        Console.WriteLine($"\"{file}\"");
    }
}