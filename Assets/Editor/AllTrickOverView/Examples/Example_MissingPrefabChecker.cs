using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_MissingPrefabChecker : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("MissingPrefabChecker",
                "丢失引用Prefab检测器",
                "Assets",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class MissingPrefabChecker\n    {\n        private const string PATH = \"Assets/Editor/Examples/Example_07_MissingPrefabChecker/Prefabs\";\n        private const string PrefabNameColor = \"#00FF00\";\n        private const string SubPrefabNameColor = \"#FF0000\";\n\n        [MenuItem(\"Tools/CheckMissingPrefab\", priority = 7)]\n        private static void CheckMissingPrefab()\n        {\n            var guids = AssetDatabase.FindAssets(\"t:GameObject\", new[] {PATH});\n            var length = guids.Length;\n            var index = 1;\n            foreach (var guid in guids)\n            {\n                var path = AssetDatabase.GUIDToAssetPath(guid);\n                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);\n                EditorUtility.DisplayProgressBar(\"玩命检查中\", \"玩命检查中...\" + path, (float) index / length);\n                FindMissingPrefabRecursive(prefab, prefab.name, true);\n            }\n\n            EditorUtility.ClearProgressBar();\n        }\n\n        static void FindMissingPrefabRecursive(GameObject gameObject, string prefabName, bool isRoot)\n        {\n            if (gameObject.name.Contains(\"Missing Prefab\"))\n            {\n                Debug.LogError(\n                    \"<color={PrefabNameColor}>{prefabName}</color> has missing prefab <color={SubPrefabNameColor}>{gameObject.name}</color>\");\n                return;\n            }\n\n            if (PrefabUtility.IsPrefabAssetMissing(gameObject))\n            {\n                Debug.LogError(\n                    \"<color={PrefabNameColor}>{prefabName}</color> has missing prefab <color={SubPrefabNameColor}>{gameObject.name}</color>\");\n                return;\n            }\n\n            if (PrefabUtility.IsDisconnectedFromPrefabAsset(gameObject))\n            {\n                Debug.LogError(\n                    \"<color={PrefabNameColor}>{prefabName}</color> has missing prefab <color={SubPrefabNameColor}>{gameObject.name}</color>\");\n                return;\n            }\n\n            if (!isRoot)\n            {\n                if (PrefabUtility.IsAnyPrefabInstanceRoot(gameObject))\n                {\n                    return;\n                }\n\n                GameObject root = PrefabUtility.GetNearestPrefabInstanceRoot(gameObject);\n                if (root == gameObject)\n                {\n                    return;\n                }\n            }\n\n            foreach (Transform childT in gameObject.transform)\n            {\n                FindMissingPrefabRecursive(childT.gameObject, prefabName, false);\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_07_MissingPrefabChecker",
                typeof(Example_MissingPrefabChecker),
                picPath : "Assets/Editor/Examples/Example_07_MissingPrefabChecker/QQ截图20210419153500.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
