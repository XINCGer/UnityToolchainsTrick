using System.Collections;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.Core.Editors
{
    public abstract class BasicEditorWindow : EditorWindow
    {
        static GUIStyle csIconStyle;
        static GUIContent csIcon;

        public static GUIStyle CSIconStyle
        {
            get
            {
                if (csIconStyle == null)
                {
                    csIconStyle = new GUIStyle(GUI.skin.button);
                    csIconStyle.padding.left = 0;
                    csIconStyle.padding.right = 0;
                    csIconStyle.padding.top = 0;
                    csIconStyle.padding.bottom = 0;
                }
                return csIconStyle;
            }
        }
        public static GUIContent CSIcon
        {
            get
            {
                if (csIcon == null)
                    csIcon = new GUIContent(EditorGUIUtility.FindTexture("cs Script Icon"), "打开窗口脚本");
                return csIcon;
            }
        }

        MonoScript monoScript;
        MonoScript MonoScript
        {
            get
            {
                if (monoScript == null)
                    monoScript = MonoScript.FromScriptableObject(this);
                return monoScript;
            }
        }

        protected virtual void ShowButton(Rect rect)
        {
            rect.x -= 8;
            rect.width = 20;
            if (GUI.Button(rect, CSIcon, CSIconStyle))
                AssetDatabase.OpenAsset(MonoScript);
        }

        CoroutineMachineController CoroutineMachine = new CoroutineMachineController();

        protected virtual void Update()
        {
            CoroutineMachine.Update();
        }

        public EditorCoroutine StartCoroutine(IEnumerator _coroutine)
        {
            return CoroutineMachine.StartCoroutine(_coroutine);
        }

        public void StopCoroutine(EditorCoroutine _coroutine)
        {
            CoroutineMachine.StopCoroutine(_coroutine);
        }
    }
}