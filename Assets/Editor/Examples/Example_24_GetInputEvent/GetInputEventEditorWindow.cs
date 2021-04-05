using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class GetInputEventWindow : EditorWindow
    {
        private bool isSpaceDown = false;
        private static GetInputEventWindow _window;

        [MenuItem("Tools/GetInputEventWindow", priority = 24)]
        private static void PopUp()
        {
            _window = GetWindow<GetInputEventWindow>("GetInputEventWindow");
            _window.Show();
        }

        private void OnGUI()
        {
            Event e = Event.current;
            if (e.isKey)
            {
                if (e.keyCode == KeyCode.Space)
                {
                    isSpaceDown = e.type != EventType.KeyUp;
                }

                if (e.type == EventType.KeyUp)
                {
                    if (e.keyCode == KeyCode.A && isSpaceDown)
                    {
                        Debug.LogError("组合键：空格 + A");
                    }
                    else if (e.keyCode == KeyCode.S && isSpaceDown)
                    {
                        Debug.LogError("组合键：空格 + S");
                    }
                }
                else if (e.type == EventType.KeyDown)
                {
                    if (e.keyCode == KeyCode.UpArrow)
                    {
                        Debug.LogError("组合键：空格+上箭头");
                    }
                    else if (e.keyCode == KeyCode.DownArrow)
                    {
                        Debug.LogError("组合键：空格+下箭头");
                    }
                }
            }
        }
    }
}