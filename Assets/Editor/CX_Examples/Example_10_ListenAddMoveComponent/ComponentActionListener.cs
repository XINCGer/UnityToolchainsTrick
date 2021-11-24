//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-24 17:32:50
// Name: ComponentActionListener
//---------------------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Example_10
{
    [InitializeOnLoad]
    public static class ComponentActionListener
    {
        static ComponentActionListener()
        {
            ObjectFactory.componentWasAdded += AddComponentCallback;
        }
        
        static void AddComponentCallback(Component newComponent)
        {
            Debug.Log("AddComponentCallback:" + newComponent.gameObject.name);
        }

        //移除组件的监听只要重写Remove按钮就行
        [MenuItem("CONTEXT/TestCXComponent/Remove Component")]
        private static void RemoveTextCXComponent(MenuCommand menuCommand)
        {
            var component = menuCommand.context as TestCXComponent;
            var obj = component.gameObject;
            Debug.Log("RemoveTextCXComponent:" + obj.name);
            var path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path))
            {
                //说明是场景里的，直接删就行
                UnityEngine.Object.DestroyImmediate(component);
                EditorUtility.SetDirty(obj);
            }
            else
            {
                //说明是prefab
                GameObject instance = PrefabUtility.LoadPrefabContents(path);
                component = instance.GetComponent<TestCXComponent>();
                UnityEngine.Object.DestroyImmediate(component);
                PrefabUtility.SaveAsPrefabAsset(instance, path);
            }

        }
    }
}