using System;

namespace CZToolKit.Core.ReactiveX
{
    public class OnDestroy<TIn, TOut> : Operator<TIn, TOut> where TIn : IOnDestory where TOut : class, TIn
    {
        Action action;

        public OnDestroy(IObservable<TIn> _src, Action _action) : base(_src)
        {
            action = _action;
        }

        public override void OnNext(TIn _onDestroy)
        {
            _onDestroy.onDistroy += action;
            observer.OnNext(_onDestroy as TOut);
        }

        public override void OnDispose() { }
    }
}
