using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    [CustomEditor(typeof(Example_55_Scriptobj))]
    public class Example_55_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("自定义按钮"))
            {
                Debug.Log("点击了自定义按钮");
            }
        }
    }   
}
