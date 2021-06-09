#if AssetModificationProcessor
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ToolKits
{

    /// <summary>
    /// AssetModificationProcessor
    /// 官方文档：https://docs.unity3d.com/2019.4/Documentation/ScriptReference/AssetModificationProcessor.html
    /// </summary>
    public class MyAssetModificationProcessor :UnityEditor.AssetModificationProcessor
    {
        // [InitializeOnLoadMethod]
        static void InitializeOnLoadMethod()
        {
            //全局监听Project视图下的资源是否发生变化(添加，删除和移动)
            EditorApplication.projectChanged += delegate ()
            {
                Debug.Log("projectChanged");
            };
        }
        
        //监听"双击鼠标左键，打开资源"事件
        public static bool IsOpenForEdit(string assetPath, out string message)
        {
            message = null;
            Debug.LogFormat("Open AssetPath:{0}", assetPath);
            //true标识该资源可以打开，false标识不允许在Unity中打开该资源
            return true;
        }
        
        //监听"资源即将被创建"事件
        public static void OnWillCreateAsset(string path)
        {
            Debug.LogFormat("Create AssetPath:{0}", path);
        }
        
        //监听"资源即将被保存"事件
        public static string[] OnWillSaveAsset(string[] paths)
        {
            if (paths != null)
            {
                Debug.LogFormat("Save AssetPath:{0}", string.Join(",", paths));
            }
            return paths;
        }
        
        //监听"资源即将被移动"事件
        public static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
        {
            Debug.LogFormat("Move from:{0} to:{1}", oldPath, newPath);
            //AsserMoveResult.DidMove
            return AssetMoveResult.DidMove;
        }
        
        //监听"资源即将被删除"事件
        public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions opint)
        {
            Debug.LogFormat("Delete delete:{0}", assetPath);
            //AssetDeleteResult.DidNotDelete表示该资源可以被删除
            return AssetDeleteResult.DidNotDelete;
        }
    }
}
#endif