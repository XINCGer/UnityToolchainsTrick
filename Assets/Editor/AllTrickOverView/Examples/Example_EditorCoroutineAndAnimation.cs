using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_EditorCoroutineAndAnimation : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("EditorCoroutineAndAnimation",
                "编辑器协程和动画",
                "EditorWindow",
                "using CZToolKit.Core;\nusing CZToolKit.Core.Editors;\nusing System.Collections;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace CZToolKit.Examples\n{\n    public class EditorCoroutineWindow : BasicEditorWindow\n    {\n        [MenuItem(\"Tools/编辑器协程和动画\")]\n        public static void Open()\n        {\n            GetWindow<EditorCoroutineWindow>();\n        }\n\n\n        Vector2 pivot = new Vector2(0.5f, 0.5f);\n        float scale;\n        Rect r = new Rect(200, 300, 300, 300);\n        EasingType easingType;\n\n        private void OnEnable()\n        {\n            StartCoroutine(Test());\n        }\n\n        IEnumerator Test()\n        {\n            while (true)\n            {\n                scale = Easing.Tween(0, 1, (float)(EditorApplication.timeSinceStartup % 2 / 2), easingType);\n                Repaint();\n                yield return null;\n            }\n        }\n        \n        private void OnGUI()\n        {\n            easingType = (EasingType)EditorGUILayout.EnumPopup(easingType);\n            pivot = EditorGUILayout.Vector2Field(\"Pivot\", pivot);\n            EditorGUILayout.FloatField(\"Scale\", scale);\n            EditorGUILayout.RectField(GetScale(r, scale, pivot));\n            GUI.Box(GetScale(r, scale, pivot), \"\");\n\n        }\n\n        public static Rect GetScale(Rect _rect, float _scale, Vector2 _pivot)\n        {\n            Vector2 absPosition = _rect.position + _rect.size * _pivot;\n            _rect.size *= _scale;\n            _rect.position = absPosition - _rect.size * _pivot;\n            return _rect;\n        }\n    }\n}\n",
                "Assets/Editor/Examples/Example_56_EditorCoroutineAndAnimation",
                typeof(Example_EditorCoroutineAndAnimation),
                picPath : "",
                videoPath : "Assets/Editor/Examples/Example_56_EditorCoroutineAndAnimation/20210617_094416.mp4");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
