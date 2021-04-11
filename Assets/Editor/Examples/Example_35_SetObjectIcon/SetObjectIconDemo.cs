using UnityEditor;
using UnityEngine;

namespace SetObjectIcon
{
    public class SetObjectIconDemo : Editor
    {
        [MenuItem("Tools/SetObjectIconDemo", priority = 35)]
        private static void TestMenu()
        {
            //console.erroricon
            EditorGUIExtension.SetIcon(GameObject.Find("SetObjectIcon"), "console.erroricon");
        }
    }
}