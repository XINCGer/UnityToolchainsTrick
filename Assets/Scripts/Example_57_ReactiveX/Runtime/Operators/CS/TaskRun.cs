using System;
using System.Threading.Tasks;

namespace CZToolKit.Core.ReactiveX
{
    // 在一个新Task中执行接下来的任务
    public class TaskRun<T> : Operator<T>
    {
        Task task;
        public TaskRun(IObservable<T> _src) : base(_src) { }

        public override void OnNext(T _value)
        {
            task = Task.Run(() => { base.OnNext(_value); });
        }

        public override void OnDispose()
        {
            task.Dispose();
        }
    }
}
