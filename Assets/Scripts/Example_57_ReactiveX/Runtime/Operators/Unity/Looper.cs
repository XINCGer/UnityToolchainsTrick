using System;
using System.Collections;
using UnityEngine;

namespace CZToolKit.Core.ReactiveX
{
    public class Looper<T> : Operator<T>
    {
        float delay;
        float interval;
        int loopTime;
        Coroutine coroutine;

        public Looper(IObservable<T> _src, float _delay, float _interval, int _loopTime) : base(_src)
        {
            delay = Mathf.Max(0, _delay);
            interval = Mathf.Max(0, _interval);
            loopTime = _loopTime;
        }

        public override void OnNext(T _value)
        {
            coroutine = MainThreadDispatcher.Instance.StartCoroutine(Loop(delay, interval, loopTime, _value));
        }

        IEnumerator Loop(float _delay, float _interval, int _loopTime, T _value)
        {
            if (delay != 0)
                yield return new WaitForSeconds(_delay);

            base.OnNext(_value);

            WaitForSeconds seconds = new WaitForSeconds(interval);
            int currentLoops = 0;
            while (loopTime < 0 || loopTime > currentLoops++)
            {
                yield return seconds;
                base.OnNext(_value);
            }
        }

        public override void OnDispose()
        {
            if (!MainThreadDispatcher.IsNull)
                MainThreadDispatcher.Instance.StopCoroutine(coroutine);
        }
    }
}
