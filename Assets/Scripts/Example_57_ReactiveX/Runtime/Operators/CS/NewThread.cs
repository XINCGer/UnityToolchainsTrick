using System;
using System.Threading;

namespace CZToolKit.Core.ReactiveX
{
    // 在一个新Task中执行接下来的任务
    public class NewThread<T> : Operator<T>
    {
        Thread thread;
        public NewThread(IObservable<T> _src) : base(_src) { }

        public override void OnNext(T _value)
        {
            thread = new Thread(() => { base.OnNext(_value); });
            thread.Start();
        }

        public override void OnDispose()
        {
            thread.Abort();
        }
    }
}
