//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-09-22 11:50:50
// Name: EditorProgressBar
//---------------------------------------------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;

namespace CX_Example_02
{
    public class EditorProgressBar : EditorWindow
    {
        [MenuItem("CX_Tools/EditorProgessBar", priority = 1)]
        public static void OpenWindows()
        {
            var win = EditorWindow.GetWindow<EditorProgressBar>("EditorProgressBar");
            win.Show();
        }

        private bool startProgress;
        private float val;
        private float speed = 0.001f;
        private void OnGUI()
        {
            EditorGUILayout.LabelField($"上传进度:");
            var rect = GUILayoutUtility.GetRect(6, 20);//获取当前自动排布下，{Width，height}的实际Rect实例
            var precent = ((int)(val * 10000)) / 100f;
            precent = precent > 100 ? 100 : precent;
            EditorGUI.ProgressBar(rect, val, $"{precent}%");
            if (GUILayout.Button("Start"))
            {
                startProgress = true;
                val = 0;
            }
        }

        private void OnEnable()
        {
            startProgress = false;
            val = 0;
        }

        private void Update()
        {
            if(!startProgress) return;
            val += speed;
            if (val >= 1)
            {
                startProgress = false;
            }
        }
    }
}