using System;
using UnityEditor;

namespace CZToolKit.Core.Editors
{
    public interface ICondition
    {
        bool Result(EditorCoroutine _coroutine);
    }

    public class WaitForSecond_E : ICondition
    {
        readonly float time;

        public WaitForSecond_E(int _time) { time = _time; }

        public bool Result(EditorCoroutine _coroutine)
        {
            return EditorApplication.timeSinceStartup > _coroutine.TimeSinceStartup + time;
        }
    }

    public class WaitUntil_E : ICondition
    {
        readonly Func<bool> predicate;

        public WaitUntil_E(Func<bool> _predicate) { predicate = _predicate; }

        public bool Result(EditorCoroutine _coroutine)
        {
            return predicate();
        }
    }
}