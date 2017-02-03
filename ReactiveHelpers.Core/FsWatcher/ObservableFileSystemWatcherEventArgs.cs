using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveHelpers.Core.FsWatcher
{
    public class ObservableFileSystemWatcherEventArgs
    {
        public ObservableFileSystemWatcherEventArgs(IObservableFileSystemWatcher sender, bool status)
        {
            this.Sender = sender;
            this.Status = status;
        }

        public bool Status { get; private set; }
        public IObservableFileSystemWatcher Sender { get; private set; }
    }
}
