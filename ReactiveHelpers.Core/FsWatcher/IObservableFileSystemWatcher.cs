using System;
using System.IO;
#if CORE
#else
using Ninject;
using Ninject.Parameters;
#endif
namespace ReactiveHelpers.Core.FsWatcher
{
    public interface IObservableFileSystemWatcher : IDisposable, IInitializable
    {
        IObservable<FileSystemEventArgs> Changed { get; }
        IObservable<RenamedEventArgs> Renamed { get; }
        IObservable<FileSystemEventArgs> Deleted { get; }
        IObservable<ErrorEventArgs> Errors { get; }
        IObservable<FileSystemEventArgs> Created { get; }
        IObservable<bool> StatusChanged { get; }

        void Start();
        void Stop();
    }
}
