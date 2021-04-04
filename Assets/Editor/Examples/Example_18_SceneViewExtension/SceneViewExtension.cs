using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class SceneViewExtension
    {
        private static float RotateSpeed = 0.1f;

        [InitializeOnLoadMethod]
        public static void EditorInitialize()
        {
            SceneView.duringSceneGui -= Update;
            if (EditorPrefs.GetBool(Constants.SCENE_VIEW_EXTENSITON_SWITH, false))
            {
                SceneView.duringSceneGui += Update;
            }
        }

        private static void Update(SceneView sceneView)
        {
            var e = Event.current;
            if (null != e)
            {
                var isFPS = Tools.viewTool == ViewTool.FPS;
                if (isFPS && e.keyCode == KeyCode.F)
                {
                    //左旋
                    var lastAngle = SceneView.lastActiveSceneView.rotation.eulerAngles;
                    var newAngle = new Vector3(lastAngle.x,lastAngle.y,lastAngle.z - RotateSpeed);
                    SceneView.lastActiveSceneView.rotation = Quaternion.Euler(newAngle);
                }
                else if (isFPS && e.keyCode == KeyCode.G)
                {
                    //右旋
                    var lastAngle = SceneView.lastActiveSceneView.rotation.eulerAngles;
                    var newAngle = new Vector3(lastAngle.x,lastAngle.y,lastAngle.z + RotateSpeed);
                    SceneView.lastActiveSceneView.rotation = Quaternion.Euler(newAngle);
                }
            }
        }
    }
}