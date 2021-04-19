using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_ShaderVariantCount : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("ShaderVariantCount",
                "Shader变体统计",
                "Shader",
                "using System.Collections;\nusing System.Collections.Generic;\nusing System.IO;\nusing System.Reflection;\nusing System.Text;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class ShaderKit\n    {\n        private const string ShaderPath = \"Assets/GameAssets/Shaders\";\n        private static string OutputPath = \"Shader\";\n\n        [MenuItem(\"Tools/变体统计\", priority = 21)]\n        public static void CalcAllShaderVariantCount()\n        {\n            var asm = typeof(SceneView).Assembly;\n            System.Type type = asm.GetType(\"UnityEditor.ShaderUtil\");\n            MethodInfo method = type.GetMethod(\"GetVariantCount\",\n                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);\n            var shaderList = AssetDatabase.FindAssets(\"t:Shader\", new[] {ShaderPath});\n            string date = System.DateTime.Now.ToString(\"yyyy-MM-dd-hh-mm-ss\");\n            string pathF = string.Format(\"{0}/ShaderVariantCount{1}.csv\", OutputPath, date);\n            FileStream fs = new FileStream(pathF, FileMode.Create, FileAccess.Write);\n            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);\n\n            EditorUtility.DisplayProgressBar(\"Shader统计文件\", \"正在写入统计文件中...\", 0f);\n            int ix = 0;\n            sw.WriteLine(\"Shader 数量：\" + shaderList.Length);\n            sw.WriteLine(\"ShaderFile, VariantCount\");\n            int totalCount = 0;\n            foreach (var i in shaderList)\n            {\n                EditorUtility.DisplayProgressBar(\"Shader统计文件\", \"正在写入统计文件中...\", ix / shaderList.Length);\n                var path = AssetDatabase.GUIDToAssetPath(i);\n                Shader s = AssetDatabase.LoadAssetAtPath(path, typeof(Shader)) as Shader;\n                var variantCount = method.Invoke(null, new System.Object[] {s, true});\n                sw.WriteLine(path + \",\" + variantCount.ToString());\n                totalCount += int.Parse(variantCount.ToString());\n                ++ix;\n            }\n\n            sw.WriteLine(\"Shader Variant Total Amount: \" + totalCount);\n            EditorUtility.ClearProgressBar();\n            sw.Close();\n            fs.Close();\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_21_ShaderKit",
                typeof(Example_ShaderVariantCount),
                picPath : "Assets/Editor/Examples/Example_21_ShaderKit/QQ截图20210419160107.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
