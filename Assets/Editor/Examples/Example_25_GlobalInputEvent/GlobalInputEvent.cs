using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class GlobalInputEvent
    {
        private static bool isSpaceDown = false;

        [InitializeOnLoadMethod]
        public static void EditorInitialize()
        {
            // globalEventHandler
            FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler",
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            EditorApplication.CallbackFunction functions = (EditorApplication.CallbackFunction) info.GetValue(null);
            functions -= OnGlobalEventHandler;

            if (EditorPrefs.GetBool(Constants.GLOBAL_INPUT_ENEVT_ENABLE, false))
            {
                functions += OnGlobalEventHandler;
                info.SetValue(null, (object) functions);
            }

            info.SetValue(null, (object) functions);
        }

        private static void OnGlobalEventHandler()
        {
            var e = Event.current;
            if (null != e)
            {
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
}