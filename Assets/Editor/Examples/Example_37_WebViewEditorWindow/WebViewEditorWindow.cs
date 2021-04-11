using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

#if !UNITY_2020
public class WebViewEditorWindow : EditorWindow
{
    static string Url = "https://github.com/XINCGer/UnityToolchainsTrick";

    [MenuItem("Tools/WebViewWindow", priority = 37)]
    static void Open()
    {
        string typeName = "UnityEditor.Web.WebViewEditorWindowTabs";
        Type type = Assembly.Load("UnityEditor.dll").GetType(typeName);
        BindingFlags Flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        var methodInfo = type.GetMethod("Create", Flags);
        methodInfo = methodInfo.MakeGenericMethod(type);
        methodInfo.Invoke(null, new object[] { "WebView", Url, 200, 530, 800, 600 });
    }
}
#endif