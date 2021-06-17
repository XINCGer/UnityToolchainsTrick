using System;

namespace CZToolKit.Core.ReactiveX
{
    public abstract class Operator : IDisposable
    {
        public abstract void Dispose();
    }

    public abstract class Operator<T> : Operator, IObservable<T>, IObserver<T>
    {
        protected IObservable<T> src;
        protected IObserver<T> observer;


        public Operator(IObservable<T> _src)
        {
            src = _src;
        }

        public virtual void OnNext(T _value)
        {
            observer.OnNext(_value);
        }

        public virtual void OnError(Exception _error)
        {
            observer.OnError(_error);
        }

        public virtual void OnCompleted()
        {
            observer.OnCompleted();
        }

        public virtual IDisposable Subscribe(IObserver<T> _observer)
        {
            observer = _observer;
            return src.Subscribe(this);
        }

        public sealed override void Dispose()
        {
            (observer as IDisposable)?.Dispose();
            OnDispose();
        }

        public virtual void OnDispose() { }
    }

    public abstract class Operator<TIn, TOut> : Operator, IObservable<TOut>, IObserver<TIn>
    {
        protected IObservable<TIn> src;
        protected IObserver<TOut> observer;


        public Operator(IObservable<TIn> _src)
        {
            src = _src;
        }

        public abstract void OnNext(TIn _value);

        public virtual void OnError(Exception _error)
        {
            observer.OnError(_error);
        }

        public virtual void OnCompleted()
        {
            observer.OnCompleted();
        }

        public virtual IDisposable Subscribe(IObserver<TOut> _observer)
        {
            observer = _observer;
            return src.Subscribe(this);
        }

        public sealed override void Dispose()
        {
            (observer as IDisposable)?.Dispose();
            OnDispose();
        }

        public virtual void OnDispose() { }
    }
}