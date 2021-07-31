using CZToolKit.Core.Editors;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CZToolKit.Examples
{
    public class ScrollListExample : EditorWindow
    {
        [MenuItem("Tools/Scroll List")]
        public static void Open()
        {
            GetWindow<ScrollListExample>();
        }

        public List<int> nums = new List<int>(10);

        public List<GameObject> objects = new List<GameObject>(10);

        public Sprite sprite;

        SerializedObject serializedObject;

        private void OnEnable()
        {
            for (int i = 0; i < 100; i++)
            {
                nums.Add(i);
            }

            for (int i = 0; i < 100; i++)
            {
                objects.Add(null);
            }

            serializedObject = new SerializedObject(this);
        }

        bool numsFoldout = true;
        float numsScroll = 0;

        bool objectsFoldout = true;
        float objectsScroll = 0;

        private void OnGUI()
        {
            numsScroll = EditorGUILayoutExtension.ScrollList(serializedObject.FindProperty("nums"), numsScroll, ref numsFoldout, 15);

            objectsScroll = EditorGUILayoutExtension.ScrollList(serializedObject.FindProperty("objects"), objectsScroll, ref objectsFoldout, 20);
        }
    }

}
