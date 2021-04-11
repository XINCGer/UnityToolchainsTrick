//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 11:13:56
//------------------------------------------------------------

using System;
using System.Reflection;
using Plugins.AllTrickOverView.Core;
using UnityEditor;
using UnityEngine;

namespace Plugins.AllTrickOverView.Examples
{
    public class Example_AnimationCurvePreview : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo = new TrickOverViewInfo(
            "AnimationCurvePreview", 
            "直观的动画曲线预览。",
            "EditorWindow",
            "",
            "Assets/Editor/Examples/Example_29_AnimationCurvePreview", 
            typeof(Example_AnimationCurvePreview));

        private AnimationCurve _curve = new AnimationCurve();
        private MethodInfo _methodInfo;
        private Texture2D _texture;
        private static readonly Rect TEXT_RECT = new Rect(100, 100, 200, 200);
        
        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }

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