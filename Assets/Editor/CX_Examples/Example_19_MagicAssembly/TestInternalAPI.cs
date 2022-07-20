using System;
using UnityEditor;
using UnityEngine;

namespace Example_19_InternalBridge
{
    public class TestInternalAPI : EditorWindow
    {
        [MenuItem("CX_Tools/TestInternalAPI")]
        public static void OpenWindow()
        {
            var window = GetWindow<TestInternalAPI>();
            window.titleContent = new GUIContent("TestInternalAPI");
            window.Show();
        }

        private Shader _shader = null;
        private void OnGUI()
        {
            _shader = (Shader)EditorGUILayout.ObjectField(_shader, typeof(Shader), false);
            if (_shader != null && GUILayout.Button("Test OpenShaderCombinations"))
            {
                InternalShaderUtilBridge.OpenShaderCombinations(_shader,false);
            }
        }
    }
}