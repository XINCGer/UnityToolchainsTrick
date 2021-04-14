//------------------------------------------------------------
// Author: 静风霁
// Mail: 1778139321@qq.com
// Data: 2021年4月14日 10:58:24
//------------------------------------------------------------

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(CapsuleBoundsExample))]
public class CapsuleBoundsExampleEditor : Editor
{
    private CapsuleBoundsHandle m_CapsuleBoundsHandle = new CapsuleBoundsHandle(typeof(CapsuleBoundsHandle).GetHashCode());
    
    protected virtual void OnSceneGUI()
    {
        CapsuleBoundsExample boundsExample = (CapsuleBoundsExample)target;

        m_CapsuleBoundsHandle.center = boundsExample.transform.position + boundsExample.Center;
        m_CapsuleBoundsHandle.radius = boundsExample.Radius;
        m_CapsuleBoundsHandle.height = boundsExample.Height;
        m_CapsuleBoundsHandle.handleColor = Color.green;
        m_CapsuleBoundsHandle.wireframeColor = Color.green;
        
        EditorGUI.BeginChangeCheck();
        m_CapsuleBoundsHandle.DrawHandle();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(boundsExample, "Change Bounds");
            boundsExample.Center = m_CapsuleBoundsHandle.center;
            boundsExample.Radius = m_CapsuleBoundsHandle.radius;
            boundsExample.Height = m_CapsuleBoundsHandle.height;
        }
    }
}