using System;

namespace CZToolKit.Core.ReactiveX
{
    public class Execute<T> : Operator<T>
    {
        Action func;
        public Execute(IObservable<T> _src, Action _func) : base(_src)
        {
            func = _func;
        }

        public override void OnNext(T _value)
        {
            func.Invoke();
            base.OnNext(_value);
        }
    }
}
