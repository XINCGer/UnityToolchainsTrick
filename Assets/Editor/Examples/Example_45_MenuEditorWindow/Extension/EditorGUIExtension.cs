using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.Core.Editors
{
    public static partial class EditorGUIExtension
    {
        static Dictionary<string, GUIContent> GUIContentsCache = new Dictionary<string, GUIContent>();
        public static GUIContent GetGUIContent(string _name)
        {
            GUIContent content;
            if (!GUIContentsCache.TryGetValue(_name, out content))
                content = new GUIContent(_name);
            content.tooltip = string.Empty;
            content.image = null;
            return content;
        }
        public static GUIContent GetGUIContent(string _name, Texture2D _image)
        {
            GUIContent content = GetGUIContent(_name);
            content.image = _image;
            return content;
        }

        public static GUIContent GetGUIContent(string _name, string _tooltip)
        {
            GUIContent content = GetGUIContent(_name);
            content.tooltip = _tooltip;
            return content;
        }

        public static GUIContent GetGUIContent(string _name, string _tooltip, Texture2D _image)
        {
            GUIContent content = GetGUIContent(_name);
            content.tooltip = _tooltip;
            content.image = _image;
            return content;
        }

        static Dictionary<string, bool> FoldoutCache = new Dictionary<string, bool>();
        public static bool GetFoldoutBool(string _key, bool _fallback = false)
        {
            bool result;
            if (!FoldoutCache.TryGetValue(_key, out result))
                result = _fallback;
            return result;
        }

        public static void SetFoldoutBool(string _key, bool _value)
        {
            FoldoutCache[_key] = _value;
        }

        /// <summary> 绘制一个ProgressBar </summary>
        public static float ProgressBar(Rect _rect, float _value, float _minLimit, float _maxLimit, string _text, bool _dragable = true, bool _drawMinMax = false)
        {
            float progress = (_value - _minLimit) / (_maxLimit - _minLimit);

            Rect r = _rect;
            GUI.Box(r, string.Empty);
            r.width = _rect.width * progress;
            EditorGUI.DrawRect(r, new Color(0.07f, 0.56f, 0.9f, 1));

            if (_drawMinMax)
            {
                EditorGUI.LabelField(_rect, _minLimit.ToString());
                EditorGUI.LabelField(_rect, _maxLimit.ToString(), EditorStylesExtension.RightLabelStyle);
            }
            EditorGUI.LabelField(_rect, _text, EditorStylesExtension.MiddleLabelStyle);

            if (_dragable)
#if UNITY_2019_1_OR_NEWER
                return GUI.HorizontalSlider(_rect, _value, _minLimit, _maxLimit, EditorStylesExtension.Transparent, EditorStylesExtension.Transparent, EditorStylesExtension.Transparent);
#else
                return GUI.HorizontalSlider(_rect, _value, _minLimit, _maxLimit, EditorStylesExtension.Transparent, EditorStylesExtension.Transparent);
#endif
            return _value;
        }
    }
}
