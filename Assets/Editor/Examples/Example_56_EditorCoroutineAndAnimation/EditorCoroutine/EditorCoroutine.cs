using System.Collections;
using UnityEditor;

namespace CZToolKit.Core.Editors
{
    public class EditorCoroutine
    {
        IEnumerator enumerator;

        public bool IsRunning { get; private set; } = true;
        public double TimeSinceStartup { get; private set; }
        public object Current { get { return enumerator.Current; } }

        public EditorCoroutine(IEnumerator _enumerator)
        {
            enumerator = _enumerator;
        }

        public bool MoveNext()
        {
            TimeSinceStartup = EditorApplication.timeSinceStartup;
            return enumerator.MoveNext();
        }

        public void Stop() { IsRunning = false; }

    }
}