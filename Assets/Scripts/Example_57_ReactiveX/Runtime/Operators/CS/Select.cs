using System;

namespace CZToolKit.Core.ReactiveX
{
    public class Select<TIn, TOut> : Operator<TIn, TOut>
    {
        Func<TIn, TOut> selector;

        public Select(IObservable<TIn> _src, Func<TIn, TOut> _selector) : base(_src)
        {
            selector = _selector;
        }

        public override void OnNext(TIn _value)
        {
            observer.OnNext(selector(_value));
        }
    }
}
