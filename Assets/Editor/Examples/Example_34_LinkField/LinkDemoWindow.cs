using UnityEditor;
using UnityEngine;

namespace UnityEditor
{
    public class LinkDemoWindow : EditorWindow
    {
        [MenuItem("Tools/LinkDemoWindow", priority = 34)]
        private static void ShowWindow()
        {
            var window = GetWindow<LinkDemoWindow>();
            window.titleContent = new GUIContent("LinkDemo");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayoutExtension.LinkFileLabelField("EditorGUILayoutExtension.cs", "Assets/Editor/Examples/Example_34_LinkField/EditorGUILayoutExtension.cs");
            EditorGUILayoutExtension.LinkUrlLabelField("UnityToolchainsTrick仓库","https://github.com/XINCGer/UnityToolchainsTrick");
            
            EditorGUIExtension.LinkFileLabelField(new Rect(0,100,120,16), "EditorGUIExtension.cs","Assets/Editor/Examples/Example_34_LinkField/EditorGUIExtension.cs");
            EditorGUIExtension.LinkUrlLabelField(new Rect(0,120,150,16),"UnityToolchainsTrick仓库","https://github.com/XINCGer/UnityToolchainsTrick");
        }
    }
}