//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-08-21 11:20:44
// Name: FindEditorTex
//---------------------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;
namespace PomodoroTimer
{
    public class FindTextureExample : EditorWindow
    {
        string s;

        [MenuItem("Examples/Find editor texture")]
        static void findTextureExample()
        {
            FindTextureExample window = EditorWindow.GetWindow<FindTextureExample>();
            window.Show();
        }

        void OnGUI()
        {
            if (GUILayout.Button("Test"))
            {
                CreateOREditPopup.ShowWindow();
            }

            s = EditorGUILayout.TextField("Texture To Locate:", s);

            if (GUILayout.Button("Check"))
                if (EditorGUIUtility.FindTexture(s))
                {
                    Debug.Log("Texture found at: " + s);
                }
                else
                {
                    Debug.Log("No texture found at: " + s + ". Check your filename.");
                }
        }
    }
}

