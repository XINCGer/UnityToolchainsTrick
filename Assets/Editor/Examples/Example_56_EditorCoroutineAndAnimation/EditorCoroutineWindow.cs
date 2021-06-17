using CZToolKit.Core;
using CZToolKit.Core.Editors;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.Examples
{
    public class EditorCoroutineWindow : BasicEditorWindow
    {
        [MenuItem("Tools/编辑器协程和动画")]
        public static void Open()
        {
            GetWindow<EditorCoroutineWindow>();
        }


        Vector2 pivot = new Vector2(0.5f, 0.5f);
        float scale;
        Rect r = new Rect(200, 300, 300, 300);
        EasingType easingType;

        private void OnEnable()
        {
            StartCoroutine(Test());
        }

        IEnumerator Test()
        {
            while (true)
            {
                scale = Easing.Tween(0, 1, (float)(EditorApplication.timeSinceStartup % 2 / 2), easingType);
                Repaint();
                yield return null;
            }
        }
        
        private void OnGUI()
        {
            easingType = (EasingType)EditorGUILayout.EnumPopup(easingType);
            pivot = EditorGUILayout.Vector2Field("Pivot", pivot);
            EditorGUILayout.FloatField("Scale", scale);
            EditorGUILayout.RectField(GetScale(r, scale, pivot));
            GUI.Box(GetScale(r, scale, pivot), "");

        }

        public static Rect GetScale(Rect _rect, float _scale, Vector2 _pivot)
        {
            Vector2 absPosition = _rect.position + _rect.size * _pivot;
            _rect.size *= _scale;
            _rect.position = absPosition - _rect.size * _pivot;
            return _rect;
        }
    }
}
