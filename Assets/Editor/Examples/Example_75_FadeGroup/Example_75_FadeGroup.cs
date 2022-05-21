using Sirenix.Utilities.Editor;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace UnityToolchinsTrick
{
    /// <summary>
    /// 
    /// https://blog.csdn.net/qq_42139931/article/details/120686951
    /// https://blog.csdn.net/tom_221x/article/details/78475300
    /// https://docs.unity3d.com/cn/2017.4/ScriptReference/EditorGUILayout.BeginFadeGroup.html
    /// </summary>
    public class Example_75_FadeGroupWindow : EditorWindow
    {
        private AnimBool _showExtraFields;
        private string _string;
        private Color _color = Color.white;
        private int _number = 0;
        private bool _foldout;

        [MenuItem("Tools/Example_75_FadeGroup", priority = 75)]
        static void Init()
        {
            Example_75_FadeGroupWindow window =
                (Example_75_FadeGroupWindow) EditorWindow.GetWindow(typeof(Example_75_FadeGroupWindow));
            window.titleContent = new GUIContent("Example_75_FadeGroup");
        }

        void OnEnable()
        {
            _showExtraFields = new AnimBool(true);
            _showExtraFields.valueChanged.AddListener(Repaint);
        }

        void OnGUI()
        {
            _showExtraFields.target =
                EditorGUILayout.Foldout(_showExtraFields.target, _showExtraFields.target ? "折叠" : "展开", true);

            //Extra block that can be toggled on and off.
            if (EditorGUILayout.BeginFadeGroup(_showExtraFields.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PrefixLabel("Color");
                _color = EditorGUILayout.ColorField(_color);
                EditorGUILayout.PrefixLabel("Text");
                _string = EditorGUILayout.TextField(_string);
                EditorGUILayout.PrefixLabel("Number");
                _number = EditorGUILayout.IntSlider(_number, 0, 10);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFadeGroup();
        }
    }
}