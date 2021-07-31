#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using UnityEditor;
using UnityEngine;

namespace CZToolKit.Core.Editors
{
    public static partial class EditorGUILayoutExtension
    {
        public static float ScrollList(SerializedProperty _list, float _scroll, ref bool _foldout, int _count = 10)
        {
            _foldout = EditorGUILayout.Foldout(_foldout, _list.displayName, true);

            if (_foldout)
            {
                EditorGUI.indentLevel++;

                GUILayout.BeginHorizontal();
                int size = EditorGUILayout.DelayedIntField("Count", _list.arraySize);
                EditorGUI.indentLevel--;
                int targetIndex = -1;
                targetIndex = EditorGUILayout.DelayedIntField(targetIndex, GUILayout.Width(40));
                GUILayout.EndHorizontal();

                EditorGUI.indentLevel++;

                if (size != _list.arraySize)
                    _list.arraySize = size;

                GUILayout.BeginHorizontal();
                Rect r = EditorGUILayout.BeginVertical();

                if (_list.arraySize > _count)
                {
                    int startIndex = Mathf.CeilToInt(_list.arraySize * _scroll);
                    startIndex = Mathf.Max(0, startIndex);
                    for (int i = startIndex; i < startIndex + _count; i++)
                    {
                        EditorGUILayout.PropertyField(_list.GetArrayElementAtIndex(i));
                    }
                }
                else
                {
                    for (int i = 0; i < _list.arraySize; i++)
                    {
                        EditorGUILayout.PropertyField(_list.GetArrayElementAtIndex(i));
                    }
                }

                EditorGUILayout.EndVertical();
                if (_list.arraySize > _count)
                {
                    GUILayout.Space(20);
                    if (_list.arraySize > _count)
                    {
                        if (Event.current.type == EventType.ScrollWheel && r.Contains(Event.current.mousePosition))
                        {
                            _scroll += Event.current.delta.y * 0.01f;
                            Event.current.Use();
                        }
                        if (targetIndex != -1)
                        {
                            _scroll = Mathf.Clamp01((float)targetIndex / _list.arraySize);
                        }

                        r.xMin += r.width + 5;
                        r.width = 20;
                        _scroll = GUI.VerticalScrollbar(r, _scroll, (float)_count / _list.arraySize, 0, 1);
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }

            return _scroll;
        }
    }
}
