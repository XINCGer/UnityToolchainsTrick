using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_SceneVisibility : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("SceneVisibility",
                "设置Scene中物体的可见性",
                "Scene",
                "using UnityEditor;\nusing UnityEngine;\n\npublic class SceneVisibilityDemo : Editor\n{\n    [MenuItem(\"Tools/SceneVisibility/Show\", priority = 39)]\n    public static void Show()\n    {\n        GameObject demoGo = GameObject.Find(\"SceneVisibilityDemo\");\n        SceneVisibilityManager.instance.Show(demoGo, true);\n    }\n\n    [MenuItem(\"Tools/SceneVisibility/Hide\", priority = 39)]\n    public static void Hide()\n    {\n        GameObject demoGo = GameObject.Find(\"SceneVisibilityDemo\");\n        SceneVisibilityManager.instance.Hide(demoGo, true);\n    }\n    \n    [MenuItem(\"Tools/SceneVisibility/ShowAll\", priority = 39)]\n    public static void ShowAll()\n    {\n        SceneVisibilityManager.instance.ShowAll();\n    }\n    \n    [MenuItem(\"Tools/SceneVisibility/HideAll\", priority = 39)]\n    public static void HideAll()\n    {\n        SceneVisibilityManager.instance.HideAll();\n    }\n    \n    [MenuItem(\"Tools/SceneVisibility/DisablePicking\", priority = 39)]\n    public static void DisablePicking()\n    {\n        GameObject demoGo = GameObject.Find(\"SceneVisibilityDemo\");\n        SceneVisibilityManager.instance.DisablePicking(demoGo, true);\n    }\n    \n    [MenuItem(\"Tools/SceneVisibility/EnablePicking\", priority = 39)]\n    public static void EnablePicking()\n    {\n        GameObject demoGo = GameObject.Find(\"SceneVisibilityDemo\");\n        SceneVisibilityManager.instance.EnablePicking(demoGo, true);\n    }\n    \n    [MenuItem(\"Tools/SceneVisibility/TogglePicking\", priority = 39)]\n    public static void TogglePicking()\n    {\n        GameObject demoGo = GameObject.Find(\"SceneVisibilityDemo\");\n        SceneVisibilityManager.instance.TogglePicking(demoGo, true);\n    }\n    \n    [MenuItem(\"Tools/SceneVisibility/ToggleVisibility\", priority = 39)]\n    public static void ToggleVisibility()\n    {\n        GameObject demoGo = GameObject.Find(\"SceneVisibilityDemo\");\n        SceneVisibilityManager.instance.ToggleVisibility(demoGo, true);\n    }\n    \n    [MenuItem(\"Tools/SceneVisibility/IsHidden\", priority = 39)]\n    public static void IsHidden()\n    {\n        GameObject demoGo = GameObject.Find(\"SceneVisibilityDemo\");\n        Debug.Log(SceneVisibilityManager.instance.IsHidden(demoGo, true));\n    }\n    \n    [MenuItem(\"Tools/SceneVisibility/IsPickingDisabled\", priority = 39)]\n    public static void IsPickingDisabled()\n    {\n        GameObject demoGo = GameObject.Find(\"SceneVisibilityDemo\");\n        Debug.Log(SceneVisibilityManager.instance.IsPickingDisabled(demoGo, true));\n    }\n}",
                "Assets/Editor/Examples/Example_39_SceneVisibility",
                typeof(Example_SceneVisibility),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
