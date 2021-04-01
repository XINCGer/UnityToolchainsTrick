using System.Collections;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class Example_14_ExcelShareRead
    {
        private const string PATH = "Excels/Example.xlsx";

        [MenuItem("Tools/Excel/NormalRead",priority = 14)]
        private static void NormalRead()
        {
            using (ExcelPackage excelPackage = new ExcelPackage(new FileStream(PATH,FileMode.Open,FileAccess.Read,FileShare.Read)))
            {
                var workbook = excelPackage.Workbook.Worksheets[1];
                for (int i = 1; i <= workbook.Dimension.End.Row; i++)
                {
                    var key = workbook.GetValue(i, 1);
                    Debug.Log($"Key: {key}");
                    for (int j = 2; j <= workbook.Dimension.End.Column; j++)
                    {
                        var value = workbook.GetValue(i, j) as string;
                        Debug.Log($"value{j} : {value}");
                    }
                }
            }
        }

        [MenuItem("Tools/Excel/ShareRead",priority = 14)]
        private static void ShareRead()
        {
            using (ExcelPackage excelPackage = new ExcelPackage(new FileStream(PATH,FileMode.Open,FileAccess.Read,FileShare.ReadWrite)))
            {
                var workbook = excelPackage.Workbook.Worksheets[1];
                for (int i = 1; i <= workbook.Dimension.End.Row; i++)
                {
                    var key = workbook.GetValue(i, 1);
                    Debug.Log($"Key: {key}");
                    for (int j = 2; j <= workbook.Dimension.End.Column; j++)
                    {
                        var value = workbook.GetValue(i, j) as string;
                        Debug.Log($"value{j} : {value}");
                    }
                }
            }
        }
    }

}
