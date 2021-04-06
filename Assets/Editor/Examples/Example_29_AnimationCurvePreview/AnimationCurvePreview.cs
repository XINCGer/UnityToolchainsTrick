using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace ToolKits
{
    public class AnimationCurvePreview : EditorWindow
    {
        private static AnimationCurvePreview _window;
        private static readonly Vector2 MIN_SIZE = new Vector2(500, 600);
        private static readonly Rect TEXT_RECT = new Rect(100, 100, 200, 200);

        private AnimationCurve _curve = new AnimationCurve();
        private MethodInfo _methodInfo;
        private Texture2D _texture;

        [MenuItem("Tools/AnimationCurvePreview", priority = 29)]
        private static void PopUp()
        {
            _window = GetWindow<AnimationCurvePreview>("动画曲线预览");
            _window.minSize = MIN_SIZE;
            _window.Init();
            _window.Show();
        }

        private void Init()
        {
            var type = Type.GetType("UnityEditorInternal.AnimationCurvePreviewCache,UnityEditor");
            _methodInfo = type.GetMethod("GetPreview",
                new[]
                {
                    typeof(int), typeof(int), typeof(AnimationCurve), typeof(Color),
                    typeof(Color), typeof(Color)
                });
        }

        private void OnGUI()
        {
            _curve = EditorGUILayout.CurveField("调整动画曲线", _curve);
            if (GUILayout.Button("预览"))
            {
                _texture = null;
                if (null != _methodInfo)
                {
                    _texture =
                        _methodInfo.Invoke(null,
                                new Object[] {200, 200, _curve, Color.green, Color.clear, Color.clear}) as
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
                GUI.DrawTexture(TEXT_RECT, _texture, ScaleMode.ScaleToFit);
            }
        }
    }
}