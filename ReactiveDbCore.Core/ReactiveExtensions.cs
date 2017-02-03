using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveDbCore
{
    public static class ReactiveExtensions
    {
       /* public static IObservable<T> CountSubscribers<T>(this IObservable<T> source, Action<int> countChanged)
        {
            int count = 0;

            return Observable.Defer(() =>
            {
                count = Interlocked.Increment(ref count);
                countChanged(count);
                return source.Finally(() =>
                {
                    count = Interlocked.Decrement(ref count);
                    countChanged(count);
                });
            });
        }*/


    }
}
