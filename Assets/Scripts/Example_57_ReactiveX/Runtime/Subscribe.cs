using System;

namespace CZToolKit.Core.ReactiveX
{
    class Subscribe<T> : IObserver<T>
    {
        public static readonly Action<T> Ignore = (T t) => { };
        public static readonly Action<Exception> Throw = (ex) => { throw ex; };
        public static readonly Action None = () => { };

        readonly Action<T> onNext;
        readonly Action<Exception> onError;
        readonly Action onCompleted;

        public Subscribe(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            this.onNext = onNext;
            this.onError = onError;
            this.onCompleted = onCompleted;
        }

        public Subscribe()
        {
            onNext = Ignore;
            onError = Throw;
            onCompleted = None;
        }

        public Subscribe(Action<T> _onNext)
        {
            onNext = _onNext;
            onError = Throw;
            onCompleted = None;
        }

        public Subscribe(Action<T> _onNext, Action<Exception> _onError)
        {
            onNext = _onNext;
            onError = _onError;
            onCompleted = None;
        }

        public Subscribe(Action<T> _onNext, Action _onCompleted)
        {
            onNext = _onNext;
            onError = Throw;
            onCompleted = _onCompleted;
        }

        public void OnNext(T _value)
        {
            onNext?.Invoke(_value);
        }

        public void OnError(Exception _error)
        {
            onError(_error);
        }

        public void OnCompleted()
        {
            onCompleted();
        }
    }
}