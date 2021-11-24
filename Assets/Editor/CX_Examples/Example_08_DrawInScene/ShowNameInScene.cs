using UnityEditor;
using UnityEngine;

namespace Example_08_DrawInScene
{
    public static class ShowNameInScene
    {
        private static bool _isShowName;

        [MenuItem("CX_Tools/DrawInScene/ShowName",false ,priority = 5)]
        private static void ShowGizmos()
        {
            _isShowName = true;
        }
        [MenuItem("CX_Tools/DrawInScene/ShowName",true ,priority = 5)]
        private static bool CheckShowGizmos()
        {
            return !_isShowName;
        }
        [MenuItem("CX_Tools/DrawInScene/HideName",false ,priority = 5)]
        private static void HideGizmos()
        {
            _isShowName = false;
        }
        [MenuItem("CX_Tools/DrawInScene/HideName",true ,priority = 5)]
        private static bool CheckHideGizmos()
        {
            return _isShowName;
        }
        
        //只能检测到能挂在物体上的组件，抽象类检测不到
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        public static void DrawInSceneDrawGizmos(Transform transform, GizmoType gizmoType)
        {
            if(!_isShowName) return;
            Handles.Label(transform.position, transform.name);
        }
    }
}