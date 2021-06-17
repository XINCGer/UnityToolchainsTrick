using System;
using System.Collections;
using UnityEngine;

namespace CZToolKit.Core.ReactiveX
{
    public enum UpdateType { FixedUpdate, Update, LateUpdate }
    public class EveryUpdate<T> : Operator<T>
    {
        UpdateType updateType = UpdateType.Update;

        public EveryUpdate(IObservable<T> _src, UpdateType _updateType) : base(_src)
        {
            src = _src;
            updateType = _updateType;
        }

        Coroutine coroutine;
        public override void OnNext(T _value)
        {
            coroutine = MainThreadDispatcher.Instance.StartCoroutine(Update(() =>
            {
                observer.OnNext(_value);
            }));
        }

        public IEnumerator Update(Action _action)
        {
            switch (updateType)
            {
                case UpdateType.FixedUpdate:
                    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
                    while (true)
                    {
                        _action();
                        yield return waitForFixedUpdate;
                    }
                case UpdateType.Update:
                    while (true)
                    {
                        _action();
                        yield return true;
                    }
                case UpdateType.LateUpdate:
                    WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
                    while (true)
                    {
                        _action();
                        yield return waitForEndOfFrame;
                    }
            }
        }

        public override void OnDispose()
        {
            MainThreadDispatcher.Instance.StopCoroutine(coroutine);
        }
    }
}