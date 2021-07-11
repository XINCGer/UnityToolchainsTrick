using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class DrawGridWindow : EditorWindow
    {
        private static DrawGridWindow _window;
        private static Vector2 MIN_SIZE = new Vector2(400, 300);

        private Color backgroundColor;
        private Color gridColor;
        private Vector2 drag;
        private Vector2 offset;

        [MenuItem("Tools/绘制网格的示例窗口", priority = 61)]
        private static void PopUp()
        {
            _window = GetWindow<DrawGridWindow>("绘制网格的示例窗口");
            _window.minSize = MIN_SIZE;
            _window.Init();
            _window.Show();
        }

        private void Init()
        {
            backgroundColor = new Color(0.4f, 0.4f, 0.4f);
            gridColor = new Color(0.1f, 0.1f, 0.1f);
        }

        private void OnGUI()
        {
            ProcessEvents(Event.current);
            DrawBackground();
            DrawGrid(10, 0.2f);
            DrawGrid(50, 0.4f);
            if (GUI.changed) Repaint();
        }

        private void DrawBackground()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), backgroundColor);
        }

        private void DrawGrid(float gridSpacing, float gridOpacity)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);
            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);
            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset,
                    new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset,
                    new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }

                    break;
            }
        }

        private void OnDrag(Vector2 delta)
        {
            drag = delta;
            GUI.changed = true;
        }
    }
}