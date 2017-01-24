﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Concurrency;
using Splat;

namespace ReactiveCore
{
    internal static class ReactiveCoreApp
    {
        static ReactiveCoreApp()
        {
            DefaultExceptionHandler = Observer.Create<Exception>(ex => {
                

                MainThreadScheduler.Schedule(() => {
                    throw new Exception(
                        "An OnError occurred on an object (usually ObservableAsPropertyHelper) that would break a binding or command. To prevent this, Subscribe to the ThrownExceptions property of your objects",
                        ex);
                });
            });
        }

        public static IObserver<Exception> DefaultExceptionHandler { get; private set; }

        [ThreadStatic]
        static IScheduler _UnitTestMainThreadScheduler;
        static IScheduler _MainThreadScheduler;

        /// <summary>
        /// MainThreadScheduler is the scheduler used to schedule work items that
        /// should be run "on the UI thread". In normal mode, this will be
        /// DispatcherScheduler, and in Unit Test mode this will be Immediate,
        /// to simplify writing common unit tests.
        /// </summary>
        public static IScheduler MainThreadScheduler
        {
            get
            {
                return _UnitTestMainThreadScheduler ?? _MainThreadScheduler;
            }
            set
            {
                // N.B. The ThreadStatic dance here is for the unit test case -
                // often, each test will override MainThreadScheduler with their
                // own TestScheduler, and if this wasn't ThreadStatic, they would
                // stomp on each other, causing test cases to randomly fail,
                // then pass when you rerun them.
                if (ModeDetector.InUnitTestRunner())
                {
                    _UnitTestMainThreadScheduler = value;
                    _MainThreadScheduler = _MainThreadScheduler ?? value;
                }
                else
                {
                    _MainThreadScheduler = value;
                }
            }
        }
    }
}
