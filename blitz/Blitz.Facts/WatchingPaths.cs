using System;
using System.IO;
using Xbehave;
using Xunit;

namespace Blitz.Facts
{
    public class WatchingPaths : IDisposable
    {
        private string _tempDirectoryPath;
        private string _watchedFilePath;

        public WatchingPaths()
        {
            _tempDirectoryPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_tempDirectoryPath);
            _watchedFilePath = Path.Combine(_tempDirectoryPath, "test.txt");
        }

        [Scenario]
        public void when_file_from_watched_directory_changes()
        {
            WatchedPath watchedPath = null;
            bool subscriberNotified = false;
            "given a file exists"
                .f(() => { File.AppendAllText(_watchedFilePath, "foo"); });

            "and it's path is watched"
                .f(() => {
                    watchedPath = new WatchedPath(
                        Path.GetDirectoryName(_watchedFilePath));
                    watchedPath.OnFileChanged(
                        () => { subscriberNotified = true; });
                    watchedPath.StartWatching();
                });

            "when the file changes"
                .f(() => { File.AppendAllText(_watchedFilePath, "bar"); });

            "then subscribers are notified"
                .f(() => { Assert.True(subscriberNotified); });
        }

        public void Dispose()
        {
            try { Directory.Delete(_tempDirectoryPath, true); }
            catch { }
        }
    }
}
