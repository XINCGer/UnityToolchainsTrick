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
        private static float MIN_ZOOM = 0.4f;
        private static float MAX_ZOOM = 7f;
        private static float mGraphZoomScaler = 0.05f;

        private Color backgroundColor;
        private Color gridColor;
        private Vector2 drag;
        private Vector2 offset;

        private Rect mGraphRect;
        private Vector2 mGraphOffset = Vector2.zero;
        private float mGraphZoom = 1f;
        private Material mGridMaterial;

        private bool isDrawWithHandleAPI = true;

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

            if (mGridMaterial == null)
            {
                mGridMaterial = new Material(Shader.Find("Hidden/Behavior Designer/Grid"));
                mGridMaterial.hideFlags = HideFlags.HideAndDontSave;
                mGridMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
            }
        }

        private void OnGUI()
        {
            SetUpSize();
            ProcessEvents(Event.current);
            DrawBackground();
            if (isDrawWithHandleAPI)
            {
                DrawGrid(10 * mGraphZoom, 0.2f);
                DrawGrid(50 * mGraphZoom, 0.4f);
            }
            else
            {
                DrawGrid();
            }

            isDrawWithHandleAPI =
                GUILayout.Toggle(isDrawWithHandleAPI, isDrawWithHandleAPI ? "使用GL API绘制" : "使用Handle API绘制");
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
                case EventType.ScrollWheel:
                    OnMouseZoom();
                    break;
            }
        }

        private void SetUpSize()
        {
            mGraphRect = new Rect(Vector2.zero, new Vector2(position.width, position.height));
        }

        private void OnDrag(Vector2 delta)
        {
            drag = delta;
            GUI.changed = true;
        }

        private void OnMouseZoom()
        {
            mGraphZoom += mGraphZoomScaler * Event.current.delta.y;
            mGraphZoom = Mathf.Clamp(mGraphZoom, MIN_ZOOM, MAX_ZOOM);
            Event.current.Use();
        }

        private void DrawGrid()
        {
            mGridMaterial.SetPass((!EditorGUIUtility.isProSkin) ? 1 : 0);
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            DrawGridLines(10f * mGraphZoom,
                new Vector2(mGraphOffset.x % 10f * mGraphZoom, mGraphOffset.y % 10f * mGraphZoom));
            GL.End();
            GL.PopMatrix();
            mGridMaterial.SetPass((!EditorGUIUtility.isProSkin) ? 3 : 2);
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            DrawGridLines(50f * mGraphZoom,
                new Vector2(mGraphOffset.x % 50f * mGraphZoom, mGraphOffset.y % 50f * mGraphZoom));
            GL.End();
            GL.PopMatrix();
        }

        private void DrawGridLines(float gridSize, Vector2 offset)
        {
            float num = mGraphRect.x + offset.x;
            if (offset.x < 0f)
            {
                num += gridSize;
            }

            for (float num2 = num; num2 < mGraphRect.x + mGraphRect.width; num2 += gridSize)
            {
                DrawLine(new Vector2(num2, mGraphRect.y)
                    , new Vector2(num2, mGraphRect.y + mGraphRect.height));
            }

            float num3 = mGraphRect.y + offset.y;
            if (offset.y < 0f)
            {
                num3 += gridSize;
            }

            for (float num4 = num3; num4 < mGraphRect.y + mGraphRect.height; num4 += gridSize)
            {
                DrawLine(new Vector2(mGraphRect.x, num4), new Vector2(mGraphRect.x + mGraphRect.width, num4));
            }
        }

        private void DrawLine(Vector2 p1, Vector2 p2)
        {
            GL.Vertex(p1);
            GL.Vertex(p2);
        }
    }
}