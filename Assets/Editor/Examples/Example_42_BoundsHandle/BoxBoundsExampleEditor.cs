//------------------------------------------------------------
// Author: 静风霁
// Mail: 1778139321@qq.com
// Data: 2021年4月14日 10:55:04
//------------------------------------------------------------

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(BoxBoundsExample))]
public class BoundsExampleEditor : Editor
{
    private BoxBoundsHandle m_BoxBoundsHandle = new BoxBoundsHandle(typeof(BoundsExampleEditor).GetHashCode());
    
    protected virtual void OnSceneGUI()
    {
        BoxBoundsExample boundsExample = (BoxBoundsExample)target;

        m_BoxBoundsHandle.center = boundsExample.transform.position + boundsExample.BoxBounds.center;
        m_BoxBoundsHandle.size = boundsExample.BoxBounds.size;
        m_BoxBoundsHandle.handleColor = Color.green;
        m_BoxBoundsHandle.wireframeColor = Color.green;
        
        EditorGUI.BeginChangeCheck();
        m_BoxBoundsHandle.DrawHandle();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(boundsExample, "Change Bounds");
            Bounds newBounds = new Bounds();
            newBounds.center = m_BoxBoundsHandle.center;
            newBounds.size = m_BoxBoundsHandle.size;
            boundsExample.BoxBounds = newBounds;
        }
    }
}