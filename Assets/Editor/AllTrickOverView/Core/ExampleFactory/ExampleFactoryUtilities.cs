//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月12日 14:51:04
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AllTrickOverView.Core.ExampleFactory
{
    public static class ExampleFactoryUtilities
    {
        /// <summary>
        /// 模板文件所在目录
        /// </summary>
        private const string c_TemplatePath =
            "Assets/Editor/AllTrickOverView/OverViewExampleTemplate/OverViewExampleTemplate.txt";

        /// <summary>
        /// 输出目录
        /// </summary>
        private const string c_DesPath = "Assets/Editor/AllTrickOverView/Examples";

        private static Dictionary<string, string> ReplaceMap = new Dictionary<string, string>();

        public static void CreateOverViewExampleFromTemplate(ExampleTemplate exampleTemplate)
        {
            if (exampleTemplate != null)
            {
                string templateContent = AssetDatabase.LoadAssetAtPath<TextAsset>(c_TemplatePath).text;

                string temp = templateContent;
                string finalFileName = $"{c_DesPath}/Example_{exampleTemplate.Name}.cs";

                exampleTemplate.Code = CodeEscape(exampleTemplate.Code);

                Dictionary<string, string> replaceMap = GetReplaceMap(exampleTemplate);

                foreach (var kParam in replaceMap)
                {
                    temp = temp.Replace(kParam.Key, kParam.Value);
                }

                if (!Directory.Exists(c_DesPath))
                {
                    Directory.CreateDirectory(c_DesPath);
                }

                while (File.Exists(finalFileName))
                {
                    finalFileName = finalFileName.Replace(".cs", $"_1.cs");
                }

                //将文件信息读入流中
                //初始化System.IO.FileStream类的新实例与指定路径和创建模式
                using (var fs = new FileStream(finalFileName, FileMode.OpenOrCreate))
                {
                    if (!fs.CanWrite)
                    {
                        throw new System.Security.SecurityException("文件fileName=" + finalFileName + "是只读文件不能写入!");
                    }

                    var sw = new StreamWriter(fs);
                    sw.WriteLine(temp);
                    sw.Dispose();
                    sw.Close();
                }
            }
        }

        private static Dictionary<string, string> GetReplaceMap(ExampleTemplate exampleTemplate)
        {
            ReplaceMap["$EXAMPLE_NAME$"] = exampleTemplate.Name;
            ReplaceMap["$EXAMPLE_DESCRIPTION$"] = exampleTemplate.Description;
            ReplaceMap["$EXAMPLE_CATEGORY$"] = exampleTemplate.Category;
            ReplaceMap["$CODE$"] = exampleTemplate.Code;
            ReplaceMap["$CODE_PATH$"] = exampleTemplate.CodePath;
            ReplaceMap["$PIC_PATH$"] = exampleTemplate.PicPath;
            ReplaceMap["$VIDEO_PATH$"] = exampleTemplate.VideoPath;

            return ReplaceMap;
        }

        /// <summary>
        /// 将代码压缩在一行
        /// </summary>
        /// <returns></returns>
        private static string CodeEscape(string originCode)
        {
            //TODO 支持更多转义操作，目前会因为源代码内有转义内容而报错
            string finalCode = originCode.Replace(System.Environment.NewLine, "\\n");
            finalCode = finalCode.Replace("\"", "\\\"");
            return finalCode;
        }
    }
}