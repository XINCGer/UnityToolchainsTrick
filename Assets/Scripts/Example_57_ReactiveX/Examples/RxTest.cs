using CZToolKit.Core.ReactiveX;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

public class RxTest : MonoBehaviour, IOnDestory
{
    public Action onDistroy { get; set; }

    private void Start()
    {
        List<int> nums = new List<int> { 10, 20, 30 };
        nums.ToObservable()
            .Foreach()
            .TaskRun()
            .Execute(() => { Thread.Sleep(1000); })
            .Where(_value => _value > 10)
            .Select(_value => _value * _value)
            .Subscribe(_result => { Debug.Log(_result); });

        this.ToObservable()
            .Delay(1)
            .Execute(() => { Debug.Log(1); })
            .Delay(1)
            .Execute(() => { Debug.Log(2); })
            .Delay(1)
            .Execute(() => { Debug.Log(3); })
            .Subscribe();

        float lastClickTime = 0;
        this.ToObservable()
            .EveryUpdate()
            .Where(_ => Input.GetButtonDown("Fire1"))
            .Subscribe(_ =>
            {
                if (Time.time - lastClickTime < 0.25f)
                    Debug.Log("双击");
                else
                {
                    Debug.Log("单击");
                    lastClickTime = Time.time;
                }
            });

        int clickCount = 0;
        Observable<RxTest> everyUpdate = this.ToObservable();
        everyUpdate
            .EveryUpdate()
            .Where(_ =>
            {
                return Input.GetKeyDown(KeyCode.A);
            })
            .Subscribe(_ =>
            {
                clickCount++;
            });

        Observable<RxTest> looper = this.ToObservable();
        looper.Looper(0, 1, 10)
            .Subscribe(_ =>
            {
                if (clickCount > 5)
                {
                    Debug.Log("loop Disposed");
                    looper.Dispose();
                }
                Debug.Log("loop");
            });


        Observable<RxTest> observable = this.ToObservable();
        observable
            .TaskRun()
            .Execute(() =>
            {
                Thread.Sleep(10000);
                Debug.Log("NewTask");
            })
            .OnDestroy(() => { observable.Dispose(); })
            .Subscribe();
    }

    private void OnDestroy()
    {
        onDistroy?.Invoke();
    }
}