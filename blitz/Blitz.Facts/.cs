using System;
using System.Collections.Generic;
using System.IO;

namespace Blitz.Facts
{
    internal class WatchedPath
    {
        private FileSystemWatcher _pathWatcher;
        private IList<Action> _onChagedHandlers = new List<Action>();

        public WatchedPath(string tempDirectoryPath)
        {
            _pathWatcher = new FileSystemWatcher(tempDirectoryPath);
            _pathWatcher.NotifyFilter =
                            NotifyFilters.LastAccess |
                            NotifyFilters.LastWrite |
                            NotifyFilters.FileName;
            _pathWatcher.Changed += WatchedPathContentChanged;
        }

        public void StartWatching()
        {
            _pathWatcher.EnableRaisingEvents = true;
        }

        private void WatchedPathContentChanged(object sender, FileSystemEventArgs e)
        {
            foreach (var handler in _onChagedHandlers)
            {
                handler();
            }
        }

        internal void OnFileChanged(Action onChanged)
        {
            _onChagedHandlers.Add(onChanged);
        }
    }
}