using System;

namespace CZToolKit.Core.ReactiveX
{
    public class First<T> : Operator<T>
    {
        bool published = true;

        public First(IObservable<T> _src) : base(_src) { }

        public override void OnNext(T _value)
        {
            if (published)
            {
                published = false;
                base.OnNext(_value);
            }
        }
    }
}
