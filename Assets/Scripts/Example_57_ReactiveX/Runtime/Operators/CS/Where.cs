using System;

namespace CZToolKit.Core.ReactiveX
{
    public class Where<T> : Operator<T>
    {
        Func<T, bool> filter;

        public Where(IObservable<T> _src, Func<T, bool> _filter) : base(_src)
        {
            filter = _filter;
        }

        public override void OnNext(T _value)
        {
            if (filter(_value))
                base.OnNext(_value);
        }
    }
}