using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_BoundsHandle : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("BoundsHandle",
                "在Scene绘制常见碰撞体的编辑样式",
                "Scene",
                "//------------------------------------------------------------\n// Author: 静风霁\n// Mail: 1778139321@qq.com\n// Data: 2021年4月14日 10:55:04\n//------------------------------------------------------------\n\nusing UnityEditor;\nusing UnityEditor.IMGUI.Controls;\nusing UnityEngine;\n\n[CustomEditor(typeof(BoxBoundsExample))]\npublic class BoundsExampleEditor : Editor\n{\n    private BoxBoundsHandle m_BoxBoundsHandle = new BoxBoundsHandle(typeof(BoundsExampleEditor).GetHashCode());\n    \n    protected virtual void OnSceneGUI()\n    {\n        BoxBoundsExample boundsExample = (BoxBoundsExample)target;\n\n        m_BoxBoundsHandle.center = boundsExample.transform.position + boundsExample.BoxBounds.center;\n        m_BoxBoundsHandle.size = boundsExample.BoxBounds.size;\n        m_BoxBoundsHandle.handleColor = Color.green;\n        m_BoxBoundsHandle.wireframeColor = Color.green;\n        \n        EditorGUI.BeginChangeCheck();\n        m_BoxBoundsHandle.DrawHandle();\n        if (EditorGUI.EndChangeCheck())\n        {\n            Undo.RecordObject(boundsExample, \"Change Bounds\");\n            Bounds newBounds = new Bounds();\n            newBounds.center = m_BoxBoundsHandle.center;\n            newBounds.size = m_BoxBoundsHandle.size;\n            boundsExample.BoxBounds = newBounds;\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_42_BoundsHandle",
                typeof(Example_BoundsHandle),
                picPath : "Assets/Editor/Examples/Example_42_BoundsHandle/QQ截图20210419165726.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
