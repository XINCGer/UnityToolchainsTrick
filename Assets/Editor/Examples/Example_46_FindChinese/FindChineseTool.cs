using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;

public class FindChineseTool : MonoBehaviour
{
    [MenuItem("Tools/查找代码中文")]
    public static void Pack()
    {
        Rect wr = new Rect(300, 400, 400, 100);
        FindChineseWindow window = (FindChineseWindow)EditorWindow.GetWindowWithRect(typeof(FindChineseWindow), wr, true, "查找项目中的中文字符");
        window.Show();
    }
}

public class FindChineseWindow : EditorWindow
{
    private ArrayList csList = new ArrayList();
    private int eachFrameFind = 4;
    private int currentIndex = 0;
    private bool isBeginUpdate = false;
    private string outputText;
    public string filePath = "/Scripts";
    private void GetAllFIle(DirectoryInfo dir)
    {
        FileInfo[] allFile = dir.GetFiles();
        foreach (FileInfo fi in allFile)
        {
            if (fi.DirectoryName.Contains("FindChineseTool"))//排除指定名称的代码
                continue;
            if (fi.FullName.IndexOf(".meta") == -1 && fi.FullName.IndexOf(".cs") != -1)
            {
                csList.Add(fi.DirectoryName + "/" + fi.Name);
            }
        }
        DirectoryInfo[] allDir = dir.GetDirectories();
        foreach (DirectoryInfo d in allDir)
        {
            GetAllFIle(d);
        }
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        filePath = EditorGUILayout.TextField("Path", filePath);
        //if (GUILayout.Button("粘贴", GUILayout.Width(100)))
        //{
        //    TextEditor te = new TextEditor();
        //    te.Paste();
        //}
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(outputText, EditorStyles.boldLabel);

        if (GUILayout.Button("开始遍历项目"))
        {
            csList.Clear();
            DirectoryInfo d = new DirectoryInfo(Application.dataPath + filePath);
            GetAllFIle(d);
            outputText = "游戏内代码文件的数量：" + csList.Count;
            isBeginUpdate = true;
            outputText = "开始遍历项目";

            string name = "ChineseTexts.csv";
            string powerCsv = Application.dataPath + "/../Temp/" + name;
            if (File.Exists(powerCsv))
            {
                File.Delete(powerCsv);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    void Update()
    {
        if (isBeginUpdate && currentIndex < csList.Count)
        {
            int count = (csList.Count - currentIndex) > eachFrameFind ? eachFrameFind : (csList.Count - currentIndex);
            for (int i = 0; i < count; i++)
            {
                string url = csList[currentIndex].ToString();
                currentIndex = currentIndex + 1;
                url = url.Replace("\\", "/");
                printChinese(url);
            }
            if (currentIndex >= csList.Count)
            {
                isBeginUpdate = false;
                currentIndex = 0;
                outputText = "遍历结束，总共" + csList.Count;
            }
        }
    }

    private bool HasChinese(string str)
    {
        return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
    }

    private Regex regex = new Regex("\"[^\"]*\"");
    private void printChinese(string path)
    {
        if (File.Exists(path))
        {
            string[] fileContents = File.ReadAllLines(path, Encoding.Default);
            int count = fileContents.Length;
            string name = "ChineseTexts.csv";
            string powerCsv = Application.dataPath + "/../Temp/" + name;
            StreamWriter file = new StreamWriter(powerCsv, true);

            for (int i = 0; i < count; i++)
            {
                string printStr = fileContents[i].Trim();
                if (printStr.IndexOf("//") == 0)  //说明是注释
                    continue;
                if (printStr.IndexOf("#") == 0)
                {
                    continue;
                }
                if (printStr.IndexOf("Debug.Log") == 0)  //说明是注释
                    continue;
                if (printStr.Contains("//"))  //说明是注释
                    continue;


                MatchCollection matches = regex.Matches(printStr);
                Regex regexContent = new Regex("\"(.*?)\"");



                foreach (Match match in matches)
                {
                    if (HasChinese(match.Value))
                    {
                        Debug.Log("路径:" + path + " 行数:" + i + " 代码:" + printStr);

                        MatchCollection mc = regexContent.Matches(printStr);
                        foreach (var item in mc)
                        {
                            //Debug.Log("路径:" + path + " 行数:" + i + " 代码:" + printStr + " 内容:" + item.ToString());
                            StringBuilder sb = new StringBuilder();
                            sb.Append(item.ToString());
                            sb.Append(",");
                            sb.Append(path.Substring(path.IndexOf(filePath) + 7));
                            sb.Append(",");
                            sb.Append(i);
                            sb.Append(",");
                            sb.Append("\"" + printStr.Replace("\"", "\"\"") + "\"");
                            file.WriteLine(sb.ToString());
                        }
                    }
                }
            }
            file.Close();
            fileContents = null;
        }
    }
}