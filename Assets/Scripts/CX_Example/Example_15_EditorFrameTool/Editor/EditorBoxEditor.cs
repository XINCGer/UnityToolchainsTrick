using System;
using UnityEditor;
using UnityEngine;

namespace Example_15_EditorFrameTool
{
    [CustomEditor(typeof(EditorBox))]
    public class EditorBoxEditor : Editor
    {
        private void OnSceneGUI()
        {
            var editorBox = target as EditorBox;
            Handles.DrawWireCube(editorBox.Center, editorBox.Size);
            Handles.PositionHandle(editorBox.Center, editorBox.transform.rotation);
            
        }
    }
}