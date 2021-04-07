using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class TimeControlWindow : EditorWindow
    {
        private static TimeControlWindow _window;
        private static readonly Vector2 MIN_SIZE = new Vector2(400, 300);
        private TimeControl _timeControl;
        private static readonly Rect TIME_RECR = new Rect(20, 20, 300, 50);

        [MenuItem("Tools/TimeControlWindow", priority = 10)]
        private static void PopUp()
        {
            _window = GetWindow<TimeControlWindow>("TimeControlWindow");
            _window.minSize = _window.maxSize = MIN_SIZE;
            _window.Init();
            _window.Show();
        }

        private void Init()
        {
            _timeControl = new TimeControl();
        }

        private void OnGUI()
        {
            _timeControl.DoTimeControl(TIME_RECR);
            if (Event.current.type == EventType.Repaint)
            {
                _timeControl.Update();
            }
            Repaint();
        }
    }
}