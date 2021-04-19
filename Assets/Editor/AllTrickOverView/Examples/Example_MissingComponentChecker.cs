using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_MissingComponentChecker : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("MissingComponentChecker",
                "丢失引用Component检查器",
                "Assets",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class MissingComponentChecker\n    {\n        private const string PATH = \"Assets/Editor/Examples/Example_08_MissingComponentChecker/Prefabs\";\n        private const string PrefabNameColor = \"#00FF00\";\n        private const string PathColor = \"#0000FF\";\n\n        [MenuItem(\"Tools/CheckMissingComponent\", priority = 8)]\n        private static void MissingComponentCheck()\n        {\n            var guids = AssetDatabase.FindAssets(\"t:GameObject\", new[] {PATH});\n            var length = guids.Length;\n            var index = 1;\n            foreach (var guid in guids)\n            {\n                var path = AssetDatabase.GUIDToAssetPath(guid);\n                EditorUtility.DisplayProgressBar(\"玩命检查中\", \"玩命检查中...\" + path, (float) index / length);\n                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);\n                FindMissingComponentRecursive(prefab, prefab, path);\n                index++;\n            }\n\n            EditorUtility.ClearProgressBar();\n            EditorUtility.DisplayDialog(\"提示\", \"检查结束\", \"OK\");\n        }\n\n        private static void FindMissingComponentRecursive(GameObject gameObject, GameObject prefab, string path)\n        {\n            var cmps = gameObject.GetComponents<Component>();\n            for (int i = 0; i < cmps.Length; i++)\n            {\n                if (null == cmps[i])\n                {\n                    Debug.LogError(\n                        \"<color={PrefabNameColor}>{GetRelativePath(gameObject, prefab)}</color> has missing components! path is: <color={PathColor}>{path}</color>\"\n                    );\n                }\n            }\n\n            foreach (Transform trans in gameObject.transform)\n            {\n                FindMissingComponentRecursive(trans.gameObject, prefab, path);\n            }\n        }\n\n        private static string GetRelativePath(GameObject gameObject, GameObject prefab)\n        {\n            if (null == gameObject.transform.parent)\n            {\n                return gameObject.name;\n            }\n            else if (gameObject == prefab)\n            {\n                return gameObject.name;\n            }\n            else\n            {\n                return GetRelativePath(gameObject.transform.parent.gameObject, prefab) + \"/\" + gameObject.name;\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_08_MissingComponentChecker",
                typeof(Example_MissingComponentChecker),
                picPath : "Assets/Editor/Examples/Example_08_MissingComponentChecker/QQ截图20210419153834.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
