using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    [System.Flags]
    public enum CompositeEnum
    {
        Red = 1 << 1,
        Blue = 1 << 2,
        Yellow = 1 << 3,
        All = Red | Blue | Yellow,
    }

    public class CompositeEnumWindow : EditorWindow
    {
        private static CompositeEnumWindow _window;
        private static readonly Vector2 MIN_SIE = new Vector2(400, 300);

        private CompositeEnum _compositeEnum;

        [MenuItem("Tools/复合枚举", priority = 30)]
        private static void PopUp()
        {
            _window = GetWindow<CompositeEnumWindow>("复合枚举");
            _window.minSize = MIN_SIE;
            _window.Show();
        }

        private void OnGUI()
        {
            _compositeEnum = (CompositeEnum) EditorGUILayout.EnumFlagsField("复合枚举", _compositeEnum);
            if (GUILayout.Button("是否包含Red"))
            {
                var contain = (_compositeEnum & CompositeEnum.Red) > 0;
                Debug.Log($"是否包含Red？{contain}");
            }
        }
    }
}