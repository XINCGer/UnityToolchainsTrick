//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-08-20 11:45:45
// Name: GUILayoutExt
//---------------------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace PomodoroTimer
{
    public class GUILayoutExt
    {
        /// <summary>
        /// set BackgroundColorScope in this scope
        /// </summary>
        public class BackgroundColorScope : GUI.Scope
        {
            private readonly Color color;

            public BackgroundColorScope(Color color)
            {
                this.color = GUI.backgroundColor;
                GUI.backgroundColor = color;
            }

            protected override void CloseScope()
            {
                GUI.backgroundColor = color;
            }
        }

        public static void Label(GUIContent guiContent, GUIStyle guiStyle = null, bool strikethrough = false,
            params GUILayoutOption[] options)
        {
            guiStyle ??= GUI.skin.label;
            var rect = GUILayoutUtility.GetRect(guiContent, guiStyle, options);
            GUI.Label(rect, guiContent, guiStyle);
            if (strikethrough)
            {
                Handles.BeginGUI();
                Handles.color = Color.white;
                var mid = rect.center.y;
                Handles.DrawAAPolyLine(3, new Vector3 {x = rect.xMin, y = mid},
                    new Vector3 {x = rect.xMax, y = mid});
                Handles.EndGUI();
            }
        }
    }
}