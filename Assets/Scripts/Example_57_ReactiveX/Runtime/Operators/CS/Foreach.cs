using System;
using System.Collections.Generic;

namespace CZToolKit.Core.ReactiveX
{
    public class Foreach<TIn, TOut> : Operator<TIn, TOut> where TIn : IEnumerable<TOut>
    {
        public Foreach(IObservable<TIn> _src) : base(_src) { }

        public override void OnNext(TIn _value)
        {
            foreach (TOut value in _value)
            {
                observer.OnNext(value);
            }
        }
    }
}
