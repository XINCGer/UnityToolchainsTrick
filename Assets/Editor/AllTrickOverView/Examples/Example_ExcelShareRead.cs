using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_ExcelShareRead : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("ExcelShareRead",
                "Excel公共读示例",
                "Excel",
                "using System.Collections;\nusing System.Collections.Generic;\nusing System.IO;\nusing OfficeOpenXml;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class Example_14_ExcelShareRead\n    {\n        private const string PATH = \"Excels/Example.xlsx\";\n\n        [MenuItem(\"Tools/Excel/NormalRead\",priority = 14)]\n        private static void NormalRead()\n        {\n            using (ExcelPackage excelPackage = new ExcelPackage(new FileStream(PATH,FileMode.Open,FileAccess.Read,FileShare.Read)))\n            {\n                var workbook = excelPackage.Workbook.Worksheets[1];\n                for (int i = 1; i <= workbook.Dimension.End.Row; i++)\n                {\n                    var key = workbook.GetValue(i, 1);\n                    Debug.Log(\"Key: {key}\");\n                    for (int j = 2; j <= workbook.Dimension.End.Column; j++)\n                    {\n                        var value = workbook.GetValue(i, j) as string;\n                        Debug.Log(\"value{j} : {value}\");\n                    }\n                }\n            }\n        }\n\n        [MenuItem(\"Tools/Excel/ShareRead\",priority = 14)]\n        private static void ShareRead()\n        {\n            using (ExcelPackage excelPackage = new ExcelPackage(new FileStream(PATH,FileMode.Open,FileAccess.Read,FileShare.ReadWrite)))\n            {\n                var workbook = excelPackage.Workbook.Worksheets[1];\n                for (int i = 1; i <= workbook.Dimension.End.Row; i++)\n                {\n                    var key = workbook.GetValue(i, 1);\n                    Debug.Log(\"Key: {key}\");\n                    for (int j = 2; j <= workbook.Dimension.End.Column; j++)\n                    {\n                        var value = workbook.GetValue(i, j) as string;\n                        Debug.Log(\"value{j} : {value}\");\n                    }\n                }\n            }\n        }\n    }\n\n}\n",
                "Assets/Editor/Examples/Example_14_ExcelShareRead",
                typeof(Example_ExcelShareRead),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
