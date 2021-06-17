using System;

namespace CZToolKit.Core.ReactiveX
{
    public static partial class Extension
    {
        public static Observable<T> ToObservable<T>(this T _src)
        {
            return new Observable<T>(_src);
        }

        public static IDisposable Subscribe<T>(this IObservable<T> _src)
        {
            return _src.Subscribe(new Subscribe<T>());
        }

        public static IDisposable Subscribe<T>(this IObservable<T> _src, Action<T> _onNext)
        {
            return _src.Subscribe(new Subscribe<T>(_onNext));
        }

        public static IDisposable Subscribe<T>(this IObservable<T> _src, Action<T> _onNext, Action<Exception> _onError)
        {
            return _src.Subscribe(new Subscribe<T>(_onNext, _onError));
        }

        public static IDisposable Subscribe<T>(this IObservable<T> _src, Action<T> _onNext, Action _onCompleted)
        {
            return _src.Subscribe(new Subscribe<T>(_onNext, _onCompleted));
        }

        public static IDisposable Subscribe<T>(this IObservable<T> _src, Action<T> _onNext, Action<Exception> _onError, Action _onCompleted)
        {
            return _src.Subscribe(new Subscribe<T>(_onNext, _onError, _onCompleted));
        }
    }
}
