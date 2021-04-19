using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_SceneViewLock : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("SceneViewLock",
                "锁定Scene中指定物体，使其不可被操作",
                "Scene",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class SceneViewLock\n    {\n        [MenuItem(\"Tools/SceneViewLock/LockDefault\", priority = 28)]\n        private static void LockDefault()\n        {\n            var layer = LayerMask.NameToLayer(\"Default\");\n            LockLayer(layer);\n        }\n\n        [MenuItem(\"Tools/SceneViewLock/UnLockDefault\", priority = 28)]\n        private static void UnLocakDefault()\n        {\n            var layer = LayerMask.NameToLayer(\"Default\");\n            UnLockLayer(layer);\n        }\n\n        /// <summary>\n        /// 设置锁定\n        /// </summary>\n        public static void LockLayer(int layer)\n        {\n            Tools.lockedLayers |= 1 << layer;\n        }\n\n        /// <summary>\n        /// 取消锁定\n        /// </summary>\n        public static void UnLockLayer(int layer)\n        {\n            Tools.lockedLayers &= ~(1 << layer);\n        }\n\n        /// <summary>\n        /// 切换锁定\n        /// </summary>\n        public static void SwichLockLayer(int layer)\n        {\n            Tools.lockedLayers ^= 1 << layer;\n        }\n\n        /// <summary>\n        /// 判断是否锁定\n        /// </summary>\n        public static bool IsLayerLocked(int layer)\n        {\n            return (Tools.lockedLayers & 1 << layer) == 1 << layer;\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_28_SceneViewLock",
                typeof(Example_SceneViewLock),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
