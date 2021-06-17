using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Events;

namespace CZToolKit.Core.Editors
{
    public abstract class BasicEditor : Editor
    {
        Dictionary<string, UnityAction<SerializedProperty>> customDrawers;

        protected virtual void OnEnable()
        {
            EditorApplication.update += Update;
            customDrawers = new Dictionary<string, UnityAction<SerializedProperty>>();
            RegisterDrawers();
        }

        private void OnDisable()
        {
            EditorApplication.update -= Update;
        }

        Stack<EditorCoroutine> coroutineStack = new Stack<EditorCoroutine>();

        protected virtual void Update()
        {
            int count = coroutineStack.Count;
            while (count-- > 0)
            {
                EditorCoroutine coroutine = coroutineStack.Pop();
                if (!coroutine.IsRunning) continue;
                ICondition condition = coroutine.Current as ICondition;
                if (condition == null || condition.Result(coroutine))
                {
                    if (!coroutine.MoveNext())
                        continue;
                }
                coroutineStack.Push(coroutine);
            }
        }

        public EditorCoroutine StartCoroutine(IEnumerator _coroutine)
        {
            EditorCoroutine coroutine = new EditorCoroutine(_coroutine);
            coroutineStack.Push(coroutine);
            return coroutine;
        }

        public void StopCoroutine(EditorCoroutine _coroutine)
        {
            _coroutine.Stop();
        }

        protected void RegisterDrawer(string propertyPath, UnityAction<SerializedProperty> drawer)
        {
            customDrawers[propertyPath] = drawer;
        }

        protected virtual void RegisterDrawers()
        {
            RegisterDrawer("m_Script", DrawScript);
        }

        public override void OnInspectorGUI()
        {
            //EditorGUI.BeginChangeCheck();
            SerializedProperty iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            do
            {
                UnityAction<SerializedProperty> drawer;
                if (customDrawers.TryGetValue(iterator.propertyPath, out drawer))
                    drawer(iterator);
                else
                    EditorGUILayout.PropertyField(iterator);
            } while (iterator.NextVisible(false));

            //if (EditorGUI.EndChangeCheck())
            //{
            //    serializedObject.ApplyModifiedProperties();
            //    EditorUtility.SetDirty(target);
            //}
        }

        private void DrawScript(SerializedProperty _serializedProperty)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(_serializedProperty);
            EditorGUI.EndDisabledGroup();
        }
    }
}
