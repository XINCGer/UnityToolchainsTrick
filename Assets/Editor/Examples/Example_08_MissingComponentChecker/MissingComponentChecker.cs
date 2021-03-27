using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MissingComponentChecker
{
    private const string PATH = "";

    [MenuItem("Tools/MissingComponentCheck",priority = 8)]
    private static void MissingComponentCheck()
    {
        var guids = AssetDatabase.FindAssets("t:GameObject", new[] {PATH});
        var length = guids.Length;
        var index = 1;
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var cmps = prefab.GetComponentsInChildren<Component>(true);
            for (int i = 0; i < cmps.Length; i++)
            {
                if (null == cmps[i])
                {
                    Debug.LogError($"prefab:{prefab.name}has missing compoents!path is:{path}");
                }
            }
        }
    }
}
