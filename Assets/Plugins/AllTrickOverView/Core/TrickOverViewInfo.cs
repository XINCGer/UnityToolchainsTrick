//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 9:45:50
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.AllTrickOverView.Core
{
    /// <summary>
    /// 一个示例条目的信息
    /// </summary>
    public class TrickOverViewInfo
    {
        /// <summary>
        /// 显示在treeView的条目名称
        /// </summary>
        public string Name = "NoName";

        /// <summary>
        /// 分组
        /// </summary>
        public string Category = "Uncategorized";

        /// <summary>
        /// Preview object of the example.
        /// </summary>
        // Token: 0x170001E7 RID: 487
        // (get) Token: 0x06000A15 RID: 2581 RVA: 0x00030A04 File Offset: 0x0002EC04
        public object PreviewObject
        {
            get
            {
                if (this.previewObject == null)
                {
                    this.previewObject = Activator.CreateInstance(ExampleType);
                    (this.previewObject as AExample_Base).Init();
                }

                return this.previewObject;
            }
        }

        // Token: 0x0400041E RID: 1054
        private object previewObject;

        /// <summary>
        /// The type of the example object.
        /// </summary>
        // Token: 0x0400041F RID: 1055
        public Type ExampleType;

        /// <summary>
        /// The description of the example.
        /// </summary>
        public string Description;

        /// <summary>
        /// Raw code of the example.
        /// </summary>
        // Token: 0x04000422 RID: 1058
        public string Code;

        /// <summary>
        /// Raw code of the example.
        /// </summary>
        // Token: 0x04000422 RID: 1058
        public string CodePath;

        /// <summary>
        /// Sorting value of the example. Examples with lower order values should come before examples with higher order values.
        /// </summary>
        // Token: 0x04000423 RID: 1059
        public int Order;

        public TrickOverViewInfo(string name, string description, string category, string code, string codePath,
            Type type)
        {
            this.Name = name;
            this.Description = description;
            this.Category = category;

            this.Code =
                "using System;\nusing UnityEditor;\nusing UnityEngine;\n\n    public class ColorCodeWindow : EditorWindow\n    {\n        [MenuItem(\"Tools/ColorCodeWindow\")]\n        private static void ShowWindow()\n        {\n            var window = GetWindow<ColorCodeWindow>();\n            window.titleContent = new GUIContent(\"ColorCodeWindow\");\n            window.Show();\n        }\n\n        private string colorCode = \"\";\n        private void OnEnable()\n        {\n            SourceColorer sourceColorer = new SourceColorer();\n            sourceColorer.AddStyleDefinition = false;\n            sourceColorer.AddPreTags = false;\n            colorCode = System.IO.File.ReadAllText(System.IO.Path.Combine(Application.dataPath,\"8.ColorCode/Editor/ColorCodeWindow.cs\"));\n            string code = \"\npublic class Test{\n    public int a;\n}\";\n            code = code.Replace(\"\n\", \"\\n\");\n            UnityEngine.Debug.LogError(code);\n            colorCode = colorCode.Replace(\"\n\", \"\\n\");\n            //colorCode = sourceColorer.Highlight(colorCode);\n            UnityEngine.Debug.LogError(colorCode);\n        }\n\n        private void OnGUI()\n        {\n            GUIStyle sytle = new GUIStyle();\n            sytle.richText = true;\n            EditorGUILayout.LabelField(colorCode,sytle);\n        }\n    }";
            this.CodePath = codePath;
            this.ExampleType = type;
        }

        public TrickOverViewInfo()
        {
        }
    }
}