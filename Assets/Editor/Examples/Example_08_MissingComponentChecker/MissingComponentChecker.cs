using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class MissingComponentChecker
    {
        private const string PATH = "Assets/Editor/Examples/Example_08_MissingComponentChecker/Prefabs";
        private const string PrefabNameColor = "#00FF00";
        private const string PathColor = "#0000FF";

        [MenuItem("Tools/CheckMissingComponent", priority = 8)]
        private static void MissingComponentCheck()
        {
            var guids = AssetDatabase.FindAssets("t:GameObject", new[] {PATH});
            var length = guids.Length;
            var index = 1;
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                EditorUtility.DisplayProgressBar("玩命检查中", "玩命检查中..." + path, (float) index / length);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                FindMissingComponentRecursive(prefab, prefab, path);
                index++;
            }

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("提示", "检查结束", "OK");
        }

        private static void FindMissingComponentRecursive(GameObject gameObject, GameObject prefab, string path)
        {
            var cmps = gameObject.GetComponents<Component>();
            for (int i = 0; i < cmps.Length; i++)
            {
                if (null == cmps[i])
                {
                    Debug.LogError(
                        $"<color={PrefabNameColor}>{GetRelativePath(gameObject, prefab)}</color> has missing components! path is: <color={PathColor}>{path}</color>"
                    );
                }
            }

            foreach (Transform trans in gameObject.transform)
            {
                FindMissingComponentRecursive(trans.gameObject, prefab, path);
            }
        }

        private static string GetRelativePath(GameObject gameObject, GameObject prefab)
        {
            if (null == gameObject.transform.parent)
            {
                return gameObject.name;
            }
            else if (gameObject == prefab)
            {
                return gameObject.name;
            }
            else
            {
                return GetRelativePath(gameObject.transform.parent.gameObject, prefab) + "/" + gameObject.name;
            }
        }
    }
}