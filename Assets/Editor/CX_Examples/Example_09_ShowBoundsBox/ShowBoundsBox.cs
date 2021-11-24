//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-24 13:55:26
// Name: ShowBoundsBox
//---------------------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;

public static class ShowBoundsBox
{
    /// <summary>
    /// 计算包围盒
    /// </summary>
    private static Bounds CalculateBound(Transform obj)
    {
        var res = new Bounds(obj.position, Vector3.zero);
        var renderList = obj.GetComponentsInChildren<Renderer>();
        if (renderList == null || renderList.Length <= 0)
            return res;
        bool isNew = true;
        for (int i = 0; i < renderList.Length; i++)
        {
            if (!renderList[i].enabled)
                continue;
            if (isNew)
            {
                res = renderList[i].bounds;
                isNew = false;
            }
            res.Encapsulate(renderList[i].bounds);
        }
    
        return res;
    }

    private static bool _isShow;

    [MenuItem("CX_Tools/ShowBoundsBox/Show",false ,priority = 6)]
    private static void ShowGizmos()
    {
        _isShow = true;
    }
    [MenuItem("CX_Tools/ShowBoundsBox/Show",true ,priority = 6)]
    private static bool CheckShowGizmos()
    {
        return !_isShow;
    }
    [MenuItem("CX_Tools/ShowBoundsBox/Hide",false ,priority = 6)]
    private static void HideGizmos()
    {
        _isShow = false;
    }
    [MenuItem("CX_Tools/ShowBoundsBox/Hide",true ,priority = 6)]
    private static bool CheckHideGizmos()
    {
        return _isShow;
    }
    
    //只能检测到能挂在物体上的组件，抽象类检测不到
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    public static void DrawInSceneDrawGizmos(Transform transform, GizmoType gizmoType)
    {
        if(!_isShow) return;
        var bound = CalculateBound(transform);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bound.center, bound.extents * 2);
    }
}
