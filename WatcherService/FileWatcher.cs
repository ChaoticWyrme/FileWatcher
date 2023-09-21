using System.Data.SqlTypes;

namespace WatcherService;

public sealed class FileWatcher
{
    public string WatchedPath { get; }

    private FileSystemWatcher _watcher;

    public event FileSystemEventHandler? Changed
    {
        add
        {
            _watcher.Changed += value;
        }
        remove
        {
            _watcher.Changed -= value;
        }
    }
    
    public FileWatcher(string watchedPath)
    {
        WatchedPath = watchedPath;
        _watcher = new FileSystemWatcher(watchedPath);
        _watcher.IncludeSubdirectories = true;
        _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName |
                                NotifyFilters.DirectoryName;
    }

    public void Start()
    {
        _watcher.EnableRaisingEvents = true;
    }

    public void Stop()
    {
        _watcher.EnableRaisingEvents = false;
    }

    public static IEnumerable<string> EnumerateFiles(string path)
    {
        var opts = new EnumerationOptions
        {
            RecurseSubdirectories = true,
            ReturnSpecialDirectories = false
        };
        return Directory.EnumerateFiles(path, "*", opts);
    }
}