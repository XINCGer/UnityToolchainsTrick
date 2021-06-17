using System;
using System.Collections.Generic;

namespace CZToolKit.Core.ReactiveX
{
    public static partial class Extension
    {
        /// <summary> 当src满足某条件 </summary>
        public static IObservable<T> Where<T>(this IObservable<T> _src, Func<T, bool> _filter)
        {
            return new Where<T>(_src, _filter);
        }

        public static IObservable<TOut> Select<TIn, TOut>(this IObservable<TIn> _src, Func<TIn, TOut> _filter)
        {
            return new Select<TIn, TOut>(_src, _filter);
        }

        public static IObservable<T> Foreach<T>(this IObservable<IEnumerable<T>> _src)
        {
            return new Foreach<IEnumerable<T>, T>(_src);
        }

        public static IObservable<T> First<T>(this IObservable<T> _src)
        {
            return new First<T>(_src);
        }

        public static IObservable<T> Execute<T>(this IObservable<T> _src, Action _action)
        {
            return new Execute<T>(_src, _action);
        }

        public static IObservable<T> TaskRun<T>(this IObservable<T> _src)
        {
            return new TaskRun<T>(_src);
        }

        public static IObservable<T> NewThread<T>(this IObservable<T> _src)
        {
            return new NewThread<T>(_src);
        }
    }
}
