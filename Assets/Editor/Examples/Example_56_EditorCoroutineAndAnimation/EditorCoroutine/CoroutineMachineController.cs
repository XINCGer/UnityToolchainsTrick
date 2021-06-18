using System.Collections;
using System.Collections.Generic;

namespace CZToolKit.Core.Editors
{
    public class CoroutineMachineController
    {
        Queue<EditorCoroutine> coroutineQueue = new Queue<EditorCoroutine>();

        public void Update()
        {
            int count = coroutineQueue.Count;
            while (count-- > 0)
            {
                EditorCoroutine coroutine = coroutineQueue.Dequeue();
                if (!coroutine.IsRunning) continue;
                ICondition condition = coroutine.Current as ICondition;
                if (condition == null || condition.Result(coroutine))
                {
                    if (!coroutine.MoveNext())
                        continue;
                }
                coroutineQueue.Enqueue(coroutine);
            }
        }

        public EditorCoroutine StartCoroutine(IEnumerator _coroutine)
        {
            EditorCoroutine coroutine = new EditorCoroutine(_coroutine);
            coroutineQueue.Enqueue(coroutine);
            return coroutine;
        }

        public void StopCoroutine(EditorCoroutine _coroutine)
        {
            _coroutine.Stop();
        }
    }
}
