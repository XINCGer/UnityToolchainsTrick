using AllTrickOverView.Core;
using System;
using System.Reflection;
using AllTrickOverView.Core;
using UnityEditor;
using UnityEngine;

namespace AllTrickOverView.Examples
{
    public class Example_AnimationCurvePreview : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("AnimationCurvePreview",
                "Animation曲线预览",
                "Animation",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing System.IO;\nusing System.Reflection;\nusing UnityEditor;\nusing UnityEngine;\nusing Object = System.Object;\n\nnamespace ToolKits\n{\n    public class AnimationCurvePreview : EditorWindow\n    {\n        private static AnimationCurvePreview _window;\n        private static readonly Vector2 MIN_SIZE = new Vector2(500, 600);\n        private static readonly Rect TEXT_RECT = new Rect(100, 100, 200, 200);\n\n        private AnimationCurve _curve = new AnimationCurve();\n        private MethodInfo _methodInfo;\n        private Texture2D _texture;\n\n        [MenuItem(\"Tools/AnimationCurvePreview\", priority = 29)]\n        private static void PopUp()\n        {\n            _window = GetWindow<AnimationCurvePreview>(\"动画曲线预览\");\n            _window.minSize = MIN_SIZE;\n            _window.Init();\n            _window.Show();\n        }\n\n        private void Init()\n        {\n            var type = Type.GetType(\"UnityEditorInternal.AnimationCurvePreviewCache,UnityEditor\");\n            _methodInfo = type.GetMethod(\"GetPreview\",\n                new[]\n                {\n                    typeof(int), typeof(int), typeof(AnimationCurve), typeof(Color),\n                    typeof(Color), typeof(Color)\n                });\n        }\n\n        private void OnGUI()\n        {\n            _curve = EditorGUILayout.CurveField(\"调整动画曲线\", _curve);\n            if (GUILayout.Button(\"预览\"))\n            {\n                _texture = null;\n                if (null != _methodInfo)\n                {\n                    _texture =\n                        _methodInfo.Invoke(null,\n                                new Object[] {200, 200, _curve, Color.green, Color.clear, Color.clear}) as\n                            Texture2D;\n\n                    // byte[] rawBytes = tempTexture.GetRawTextureData();\n                    //\n                    // _texture = new Texture2D(tempTexture.width, tempTexture.height, tempTexture.format, false,\n                    //     false);\n                    // _texture.LoadRawTextureData(rawBytes);\n                    // _texture.Apply();\n                }\n            }\n\n            if (null != _texture)\n            {\n                GUI.DrawTexture(TEXT_RECT, _texture, ScaleMode.ScaleToFit);\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_29_AnimationCurvePreview",
                typeof(Example_AnimationCurvePreview),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
        
        private AnimationCurve _curve = new AnimationCurve();
        private MethodInfo _methodInfo;
        private Texture2D _texture;
        private static readonly Rect TEXT_RECT = new Rect(100, 100, 200, 200);
        

        public override void Init()
        {
            var type = Type.GetType("UnityEditorInternal.AnimationCurvePreviewCache,UnityEditor");
            _methodInfo = type.GetMethod("GetPreview",
                new[]
                {
                    typeof(int), typeof(int), typeof(AnimationCurve), typeof(Color),
                    typeof(Color), typeof(Color)
                });
        }

        public override void DrawUI(Rect rect)
        {
            _curve = EditorGUILayout.CurveField("调整动画曲线", _curve);
            if (GUILayout.Button("预览"))
            {
                _texture = null;
                if (null != _methodInfo)
                {
                    _texture =
                        _methodInfo.Invoke(null,
                                new System.Object[] {200, 200, _curve, Color.green, Color.clear, Color.clear}) as
                            Texture2D;

                    // byte[] rawBytes = tempTexture.GetRawTextureData();
                    //
                    // _texture = new Texture2D(tempTexture.width, tempTexture.height, tempTexture.format, false,
                    //     false);
                    // _texture.LoadRawTextureData(rawBytes);
                    // _texture.Apply();
                }
            }

            if (null != _texture)
            {
                GUILayout.Label("", GUILayout.Height(300), GUILayout.Width(300));
                GUI.DrawTexture(TEXT_RECT, _texture, ScaleMode.ScaleToFit);
            }
        }
    }
}
