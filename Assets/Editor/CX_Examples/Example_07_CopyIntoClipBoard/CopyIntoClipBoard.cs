//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-18 23:02:44
// Name: CopyIntoClipBoard
//---------------------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;

namespace Example_07_CopyIntoClipBoard
{
    public class CopyAssetGuid
    {
        [MenuItem("Assets/CopyGuid")]
        private static void CopyGuid()
        {
            if(Selection.activeObject == null) return;
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if(string.IsNullOrEmpty(path)) return;
            var guid = AssetDatabase.GUIDFromAssetPath(path);
            //unity自带复制到剪贴板的api，win可用，不知其他平台如何。
            GUIUtility.systemCopyBuffer = guid.ToString();
            EditorUtility.DisplayDialog("Copy guid", $"已复制Guid：{guid.ToString()}", "ok");
        }

    }
}