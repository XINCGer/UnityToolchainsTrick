using System;
using System.Collections;
using UnityEngine;

namespace CZToolKit.Core.ReactiveX
{
    public class Delay<T> : Operator<T>
    {
        float delay;
        Coroutine coroutine;

        public Delay(IObservable<T> _src, float _delay) : base(_src)
        {
            delay = _delay;
        }

        public override void OnNext(T _value)
        {
            coroutine = MainThreadDispatcher.Instance.StartCoroutine(DDelay(_value));
        }

        IEnumerator DDelay(T _value)
        {
            yield return new WaitForSeconds(delay);
            observer.OnNext(_value);
        }

        public override void OnDispose()
        {
            if (!MainThreadDispatcher.IsNull)
                MainThreadDispatcher.Instance.StopCoroutine(coroutine);
        }
    }
    public class SelfDelay<T> : Operator<T> where T : MonoBehaviour
    {
        float delay;
        Coroutine coroutine;
        T obs;

        public SelfDelay(IObservable<T> _src, float _delay) : base(_src)
        {
            delay = _delay;
        }

        public override void OnNext(T _value)
        {
            obs = _value;
            coroutine = obs.StartCoroutine(DDelay(obs));
        }

        IEnumerator DDelay(T _value)
        {
            yield return new WaitForSeconds(delay);
            observer.OnNext(_value);
        }

        public override void OnDispose()
        {
            obs.StopCoroutine(coroutine);
        }
    }
}