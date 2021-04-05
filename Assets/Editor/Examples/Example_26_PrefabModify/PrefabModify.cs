using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class PrefabModify
    {
        private const string PrefabPath = "Assets/GameAssets/Prefabs/Cube.prefab";

        [MenuItem("Tools/PrefabModify/AddComponentAndSave", priority = 26)]
        private static void AddComponentAndSave()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            var gameobject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (null == gameobject.GetComponent<ComponentA>())
            {
                gameobject.AddComponent<ComponentA>();
            }
            
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameobject, PrefabPath,InteractionMode.AutomatedAction);
            GameObject.DestroyImmediate(gameobject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/PrefabModify/DelComponentAndSave", priority = 26)]
        private static void DelComponentAndSave()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            var gameobject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            var cmps = gameobject.GetComponents<ComponentA>();
            foreach (var item in cmps)
            {
                Object.DestroyImmediate(item);
            }

            PrefabUtility.SaveAsPrefabAssetAndConnect(gameobject, PrefabPath,InteractionMode.AutomatedAction);
            GameObject.DestroyImmediate(gameobject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}