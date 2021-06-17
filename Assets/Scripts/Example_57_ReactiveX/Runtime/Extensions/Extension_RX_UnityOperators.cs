using System;
using UnityEngine;
using UnityEngine.UI;

namespace CZToolKit.Core.ReactiveX
{
    public static partial class Extension
    {
        public static IObservable<T> EveryUpdate<T>(this IObservable<T> _src, UpdateType _updateType = UpdateType.Update)
        {
            return new EveryUpdate<T>(_src, _updateType);
        }

        /// <summary> 通过协程实现的延迟 </summary>
        public static IObservable<T> Delay<T>(this IObservable<T> _src, float delayTime)
        {
            return new Delay<T>(_src, delayTime);
        }

        public static IObservable<T> SelfDelay<T>(this IObservable<T> _src, float delayTime) where T : MonoBehaviour
        {
            return new SelfDelay<T>(_src, delayTime);
        }

        public static IObservable<Button> OnClick(this IObservable<Button> _src)
        {
            return new OnClick(_src);
        }

        public static IObservable<TIn> OnDestroy<TIn>(this IObservable<TIn> _src, Action _action) where TIn : class, IOnDestory
        {
            return new OnDestroy<TIn, TIn>(_src, _action);
        }

        /// <summary> 通过协程实现的Loop </summary>
        /// <param name="_delay"> 第一次执行的延迟 </param>
        /// <param name="_interval"> 循环间隔 </param>
        /// <param name="_loopTimes"> 循环次数 </param>
        public static IObservable<T> Looper<T>(this IObservable<T> _src, float _delay, float _interval, int _loopTimes = -1)
        {
            return new Looper<T>(_src, _delay, _interval, _loopTimes);
        }
    }
}
