using CZToolKit.Core.Editors;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace CZToolKit.Examples
{
    public class DragDropAreaExample : EditorWindow
    {
        [MenuItem("Tools/DragDropArea")]
        public static void Open()
        {
            GetWindow<DragDropAreaExample>();
        }

        SerializedObject serializedObject;

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);
        }

        public List<UnityObject> objects = new List<UnityObject>();

        private void OnGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("objects"));

            UnityObject obj = EditorGUILayoutExtension.DragDropAreaSingle(GUILayout.Height(50));
            GUI.Box(GUILayoutUtility.GetLastRect(), "使用GUILayout方式绘制接收单个资源区域，无需指定Rect", "GroupBox");
            if (obj != null)
            {
                objects.Add(obj);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            Rect r = GUILayoutUtility.GetRect(50, 50);
            GUI.Box(r, "使用GUI方式绘制接收单个资源区域，需指定Rect", "GroupBox");
            obj = EditorGUIExtension.DragDropAreaSingle(r);
            if (obj != null)
            {
                objects.Add(obj);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            r = GUILayoutUtility.GetRect(50, 50);
            GUI.Box(r, "使用GUI方式绘制接收单个资源区域，需指定Rect，自定义高亮色", "GroupBox");
            obj = EditorGUIExtension.DragDropAreaSingle(r, Color.black);
            if (obj != null)
            {
                objects.Add(obj);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            r = GUILayoutUtility.GetRect(50, 50);
            GUI.Box(r, "使用GUI方式绘制接收单个资源区域，需指定Rect，自定义鼠标指针", "GroupBox");
            obj = EditorGUIExtension.DragDropAreaSingle(r, DragAndDropVisualMode.Link);
            if (obj != null)
            {
                objects.Add(obj);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            r = GUILayoutUtility.GetRect(50, 50);
            GUI.Box(r, "使用GUI方式绘制接收多个个资源区域，需指定Rect，自定义高亮色和鼠标指针", "GroupBox");
            UnityObject[] objs = EditorGUIExtension.DragDropAreaMulti(r, DragAndDropVisualMode.Link, new Color(0.3f, 0.58f, 0.7f, 0.3f));
            if (objs != null)
            {
                foreach (var item in objs)
                {
                    objects.Add(item);
                }
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        }

    }
}
