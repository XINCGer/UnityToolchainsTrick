using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_PrefabModify : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("PrefabModify",
                "Prefab修改示例",
                "Assets",
                "using System.Collections;\nusing System.Collections.Generic;\nusing System.Threading.Tasks;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class PrefabModify\n    {\n        private const string PrefabPath = \"Assets/GameAssets/Prefabs/Cube.prefab\";\n\n        [MenuItem(\"Tools/PrefabModify/AddComponentAndSave\", priority = 26)]\n        private static void AddComponentAndSave()\n        {\n            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);\n            var gameobject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;\n            if (null == gameobject.GetComponent<ComponentA>())\n            {\n                gameobject.AddComponent<ComponentA>();\n            }\n            \n            PrefabUtility.SaveAsPrefabAssetAndConnect(gameobject, PrefabPath,InteractionMode.AutomatedAction);\n            GameObject.DestroyImmediate(gameobject);\n            AssetDatabase.SaveAssets();\n            AssetDatabase.Refresh();\n        }\n\n        [MenuItem(\"Tools/PrefabModify/DelComponentAndSave\", priority = 26)]\n        private static void DelComponentAndSave()\n        {\n            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);\n            var gameobject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;\n            var cmps = gameobject.GetComponents<ComponentA>();\n            foreach (var item in cmps)\n            {\n                Object.DestroyImmediate(item);\n            }\n\n            PrefabUtility.SaveAsPrefabAssetAndConnect(gameobject, PrefabPath,InteractionMode.AutomatedAction);\n            GameObject.DestroyImmediate(gameobject);\n            AssetDatabase.SaveAssets();\n            AssetDatabase.Refresh();\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_26_PrefabModify",
                typeof(Example_PrefabModify),
                picPath : "Assets/Editor/Examples/Example_26_PrefabModify/QQ截图20210419161125.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
