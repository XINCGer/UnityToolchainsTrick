using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DrawArrowWindow : EditorWindow
{
    private static DrawArrowWindow _window;
    private static readonly Vector2 MIN_SIZE = new Vector2(300,200);
    private const int LABEL_SIZE = 25;

    [MenuItem("Tools/在窗口上绘制一条带有箭头的线",priority = 6)]
    public static void PopUp()
    {
        _window = GetWindow<DrawArrowWindow>("箭头线窗口");
        _window.minSize = _window.maxSize = MIN_SIZE;
        _window.Show();
    }

    private void OnGUI()
    {
        DrawArrow(new Vector2(10, 10), new Vector2(150, 10), Color.white);
        DrawArrow(new Vector2(10, 10), new Vector2(10, 150), Color.white);
        
        GUI.Label(new Rect(160,10,LABEL_SIZE,LABEL_SIZE),"X轴");
        GUI.Label(new Rect(10,160,LABEL_SIZE,LABEL_SIZE),"Y轴");
    }
    
    private void DrawArrow(Vector2 from, Vector2 to, Color color)
    {
        Handles.BeginGUI();
        Handles.color = color;
        Handles.DrawAAPolyLine(3, from, to);
        Vector2 v0 = from - to;
        v0 *= 10 / v0.magnitude;
        Vector2 v1 = new Vector2(v0.x * 0.866f - v0.y * 0.5f, v0.x * 0.5f + v0.y * 0.866f);
        Vector2 v2 = new Vector2(v0.x * 0.866f + v0.y * 0.5f, v0.x * -0.5f + v0.y * 0.866f); ;
        Handles.DrawAAPolyLine(3, to + v1, to, to + v2);
        Handles.EndGUI();
    }
}
