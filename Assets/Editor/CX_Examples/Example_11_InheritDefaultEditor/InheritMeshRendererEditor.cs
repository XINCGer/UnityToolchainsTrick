using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshRenderer))]
public class InheritMeshRendererEditor : DecoratorEditor
{
    public InheritMeshRendererEditor() : base("MeshRendererEditor")
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Adding this button"))
        {
            Debug.Log("Adding this button");
        }
    }
}
