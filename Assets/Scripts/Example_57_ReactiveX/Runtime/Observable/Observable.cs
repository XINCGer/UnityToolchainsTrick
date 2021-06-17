using System;

namespace CZToolKit.Core.ReactiveX
{
    public class Observable<T> : IDisposable, IObservable<T>
    {
        readonly T _value;
        IObserver<T> observer;

        public Observable(T value)
        {
            _value = value;
        }

        public IDisposable Subscribe(IObserver<T> _observer)
        {
            observer = _observer;
            observer.OnNext(_value);
            observer.OnCompleted();
            return this;
        }

        public void Dispose()
        {
            (observer as IDisposable)?.Dispose();
        }
    }
}