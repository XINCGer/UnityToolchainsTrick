//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月4日 12:48:55
//------------------------------------------------------------

using SetObjectIcon;
using UnityEditor;
using UnityEngine;
using UnityToolchinsTrick;

namespace ToolKits.Example_53_CustomAssetsIcon
{
    /// <summary>
    /// 目前两种做法
    /// 1：创建Gizmos文件夹，并将图标重命名为类名，放入与类命名空间一致的文件夹中，https://forum.unity.com/threads/custom-scriptableobject-icons-thumbnail.256246/#post-3654346
    /// 2：使用EditorGUIUtility.SetIconForObject，但会导致脚本文件图标也会跟着改变
    /// 自行取舍
    /// </summary>
    public class CreateExample_53_Assets
    {
        [MenuItem("Assets/Create/CreateExample_53_CustomSO_1", false, 10)]
        public static void CreateExample_53_CustomSO_1()
        {
            var so1 = ScriptableObject.CreateInstance<CustomSO_1>();
            ProjectWindowUtil.CreateAsset(so1, "CreateExample_53_CustomSO_1.asset");
        }
        
        [MenuItem("Assets/Create/CreateExample_53_CustomSO_2", false, 10)]
        public static void CreateExample_53_CustomSO_2()
        {
            var so2 = ScriptableObject.CreateInstance<CustomSO_2>();
            ProjectWindowUtil.CreateAsset(so2, "CreateExample_53_CustomSO_2.asset");
        }
        
        [MenuItem("Assets/Example_53/SetAssetsCustomIcon", false, 10)]
        public static void SetCustomIcon()
        {
            EditorGUIExtension.SetIcon(Selection.activeObject, "console.erroricon");
        }
    }
}