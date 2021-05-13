using AllTrickOverView.Core;
using ToolKits;
using UnityEngine;

namespace AllTrickOverView.Examples
{
    public class Example_LinkField : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("LinkField",
                "带链接的文本，可链接到工程内文件，可链接到网址",
                "Field",
                "using UnityEditor;\nusing UnityEngine;\n\nnamespace UnityEditor\n{\n    public class LinkDemoWindow : EditorWindow\n    {\n        [MenuItem(\"Tools/LinkDemoWindow\", priority = 34)]\n        private static void ShowWindow()\n        {\n            var window = GetWindow<LinkDemoWindow>();\n            window.titleContent = new GUIContent(\"LinkDemo\");\n            window.Show();\n        }\n\n        private void OnGUI()\n        {\n            EditorGUILayoutExtension.LinkFileLabelField(\"EditorGUILayoutExtension.cs\", \"Assets/Editor/Examples/Example_34_LinkField/EditorGUILayoutExtension.cs\");\n            EditorGUILayoutExtension.LinkUrlLabelField(\"UnityToolchainsTrick仓库\",\"https://github.com/XINCGer/UnityToolchainsTrick\");\n            \n            GUIExtension.LinkFileLabelField(new Rect(200,40,120,16), \"EditorGUIExtension.cs\",\"Assets/Editor/Examples/Example_34_LinkField/EditorGUIExtension.cs\");\n            GUIExtension.LinkUrlLabelField(new Rect(200,60,150,16),\"UnityToolchainsTrick仓库\",\"https://github.com/XINCGer/UnityToolchainsTrick\");\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_34_LinkField",
                typeof(Example_LinkField),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
        
        
        public override void DrawUI(Rect rect)
        {
            EditorGUILayoutExtension.LinkFileLabelField("EditorGUILayoutExtension.cs", "Assets/Editor/Examples/Example_34_LinkField/EditorGUILayoutExtension.cs");
            EditorGUILayoutExtension.LinkUrlLabelField("UnityToolchainsTrick仓库","https://github.com/XINCGer/UnityToolchainsTrick");
            
            GUIExtension.LinkFileLabelField(new Rect(200,40,120,16), "EditorGUIExtension.cs","Assets/Editor/Examples/Example_34_LinkField/EditorGUIExtension.cs");
            GUIExtension.LinkUrlLabelField(new Rect(200,60,150,16),"UnityToolchainsTrick仓库","https://github.com/XINCGer/UnityToolchainsTrick");
        }
    }
}
