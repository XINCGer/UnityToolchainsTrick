//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 9:28:36
//------------------------------------------------------------

using System.Collections.Generic;
using AllTrickOverView.Core;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;
using UnityEditor;


namespace AllTrickOverView.Examples
{
    public class Example_DrawArrowLine : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("DrawArrowLine", 
                "绘制两条箭头。", 
                "EditorWindow", 
                "", 
                "Assets/Editor/Examples/Example_06_DrawArrowLine",
                typeof(Example_DrawArrowLine));

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
        
        private const int LABEL_SIZE = 25;

        public override void DrawUI(Rect rect)
        {
            //使用GUILayout使布局自动改变（根据设置的参数做占位）
            GUILayout.Label("", GUILayout.Height(170), GUILayout.Width(160));
            DrawArrow(new Vector2(rect.x + 10, rect.y + 10), new Vector2(rect.x + 150, rect.y + 10), Color.white);
            DrawArrow(new Vector2(rect.x + 10, rect.y + 10), new Vector2(rect.x + 10, rect.y + 150), Color.white);

            GUI.Label(new Rect(rect.x + 160, rect.y + 10, LABEL_SIZE, LABEL_SIZE), "X轴");
            GUI.Label(new Rect(rect.x + 10, rect.y + 160, LABEL_SIZE, LABEL_SIZE), "Y轴");
        }

        private void DrawArrow(Vector2 from, Vector2 to, Color color)
        {
            Handles.BeginGUI();
            Handles.color = color;
            Handles.DrawAAPolyLine(3, from, to);
            Vector2 v0 = from - to;
            v0 *= 10 / v0.magnitude;
            Vector2 v1 = new Vector2(v0.x * 0.866f - v0.y * 0.5f, v0.x * 0.5f + v0.y * 0.866f);
            Vector2 v2 = new Vector2(v0.x * 0.866f + v0.y * 0.5f, v0.x * -0.5f + v0.y * 0.866f);
            ;
            Handles.DrawAAPolyLine(3, to + v1, to, to + v2);
            Handles.EndGUI();
        }
    }
}