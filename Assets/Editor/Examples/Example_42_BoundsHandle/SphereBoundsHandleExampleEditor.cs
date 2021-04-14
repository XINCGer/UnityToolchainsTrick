//------------------------------------------------------------
// Author: 静风霁
// Mail: 1778139321@qq.com
// Data: 2021年4月14日 11:07:24
//------------------------------------------------------------


using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(SphereBoundsHandleExample))]
public class SphereBoundsHandleExampleEditor : Editor
{
    private SphereBoundsHandle m_SphereBoundsHandle = new SphereBoundsHandle(typeof(SphereBoundsHandle).GetHashCode());
    
    protected virtual void OnSceneGUI()
    {
        SphereBoundsHandleExample boundsExample = (SphereBoundsHandleExample)target;

        m_SphereBoundsHandle.center = boundsExample.transform.position + boundsExample.Center;
        m_SphereBoundsHandle.radius = boundsExample.Radius;
        m_SphereBoundsHandle.handleColor = Color.green;
        m_SphereBoundsHandle.wireframeColor = Color.green;
        
        EditorGUI.BeginChangeCheck();
        m_SphereBoundsHandle.DrawHandle();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(boundsExample, "Change Bounds");
            boundsExample.Center = m_SphereBoundsHandle.center;
            boundsExample.Radius = m_SphereBoundsHandle.radius;
        }
    }
}