using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_SceneViewExtension : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("SceneViewExtension",
                "Scene拓展",
                "Scene",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class SceneViewExtension\n    {\n        private static float RotateSpeed = 0.1f;\n\n        [InitializeOnLoadMethod]\n        public static void EditorInitialize()\n        {\n            SceneView.duringSceneGui -= Update;\n            if (EditorPrefs.GetBool(Constants.SCENE_VIEW_EXTENSITON_SWITH, false))\n            {\n                SceneView.duringSceneGui += Update;\n            }\n        }\n\n        private static void Update(SceneView sceneView)\n        {\n            var e = Event.current;\n            if (null != e)\n            {\n                var isFPS = Tools.viewTool == ViewTool.FPS;\n                if (isFPS && e.keyCode == KeyCode.F)\n                {\n                    //左旋\n                    var lastAngle = SceneView.lastActiveSceneView.rotation.eulerAngles;\n                    var newAngle = new Vector3(lastAngle.x,lastAngle.y,lastAngle.z - RotateSpeed);\n                    SceneView.lastActiveSceneView.rotation = Quaternion.Euler(newAngle);\n                }\n                else if (isFPS && e.keyCode == KeyCode.G)\n                {\n                    //右旋\n                    var lastAngle = SceneView.lastActiveSceneView.rotation.eulerAngles;\n                    var newAngle = new Vector3(lastAngle.x,lastAngle.y,lastAngle.z + RotateSpeed);\n                    SceneView.lastActiveSceneView.rotation = Quaternion.Euler(newAngle);\n                }\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_18_SceneViewExtension",
                typeof(Example_SceneViewExtension),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
