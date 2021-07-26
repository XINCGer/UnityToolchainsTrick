using UnityEditor;
using UnityEngine;

namespace CZToolKit.Examples
{
    public class ObjectPreviewWindow : EditorWindow
    {
        [MenuItem("Tools/Object Preview Window")]
        public static void Open()
        {
            GetWindow<ObjectPreviewWindow>();
        }

        public Editor modelEditor, animEditor;

        private void OnEnable()
        {
            modelEditor = Editor.CreateEditor(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/Editor/Examples/Example_62_ObjectPreview/SF_Animal_Boar.fbx"));
        }

        private void OnGUI()
        {
            modelEditor.OnPreviewGUI(new Rect(100, 100, 300, 300), GUIStyle.none);
        }
    }
}
