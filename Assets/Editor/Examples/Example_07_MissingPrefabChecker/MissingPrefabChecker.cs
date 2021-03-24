using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MissingPrefabChecker
{
    private const string PATH = "Assets/Editor/Examples/Example_07_MissingPrefabChecker/Prefabs/";

    private static void CheckMissingPrefab()
    {
        var guids = AssetDatabase.FindAssets("t:GameObjbect", new[] {PATH});
        var length = guids.Length;
        var index = 1;
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }
    }
    
    static void FindMissingPrefabRecursive(GameObject gameObject, string prefabName, bool isRoot)
    {
        if (gameObject.name.Contains("Missing Prefab"))
        {
            Debug.LogError($"{prefabName} has missing prefab {gameObject.name}");
            return;
        }
 
        if (PrefabUtility.IsPrefabAssetMissing(gameObject))
        {
            Debug.LogError($"{prefabName} has missing prefab {gameObject.name}");
            return;
        }
 
        if (PrefabUtility.IsDisconnectedFromPrefabAsset(gameObject))
        {
            Debug.LogError($"{prefabName} has missing prefab {gameObject.name}");
            return;
        }
 
        if (!isRoot)
        {
            if (PrefabUtility.IsAnyPrefabInstanceRoot(gameObject))
            {
                return;
            }
            GameObject root = PrefabUtility.GetNearestPrefabInstanceRoot(gameObject);
            if (root == gameObject)
            {
                return;
            }
        }
        
        foreach (Transform childT in gameObject.transform)
        {
            FindMissingPrefabRecursive(childT.gameObject, prefabName, false);
        }
    }
}
