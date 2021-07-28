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

using UnityObject = UnityEngine.Object;

namespace CZToolKit.Core.Editors
{
    public static partial class EditorGUILayoutExtension
    {
        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static UnityObject[] DragDropAreaMulti(params GUILayoutOption[] _options)
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(), GUI.skin.label, _options);
            return EditorGUIExtension.DragDropAreaMulti(rect);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static UnityObject[] DragDropAreaMulti(DragAndDropVisualMode _dropVisualMode, params GUILayoutOption[] _options)
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(), GUI.skin.label, _options);
            return EditorGUIExtension.DragDropAreaMulti(rect, _dropVisualMode);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static UnityObject[] DragDropAreaMulti(Color _hightlightColor, params GUILayoutOption[] _options)
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(), GUI.skin.label, _options);
            return EditorGUIExtension.DragDropAreaMulti(rect, _hightlightColor);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static UnityObject[] DragDropAreaMulti(DragAndDropVisualMode _dropVisualMode, Color _hightlightColor, params GUILayoutOption[] _options)
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(), GUI.skin.label, _options);
            return EditorGUIExtension.DragDropAreaMulti(rect, _dropVisualMode, _hightlightColor);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static UnityObject DragDropAreaSingle(params GUILayoutOption[] _options)
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(), GUI.skin.label, _options);
            return EditorGUIExtension.DragDropAreaSingle(rect);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static UnityObject DragDropAreaSingle(DragAndDropVisualMode _dropVisualMode, params GUILayoutOption[] _options)
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(), GUI.skin.label, _options);
            return EditorGUIExtension.DragDropAreaSingle(rect, _dropVisualMode);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static UnityObject DragDropAreaSingle(Color _hightlightColor, params GUILayoutOption[] _options)
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(), GUI.skin.label, _options);
            return EditorGUIExtension.DragDropAreaSingle(rect, _hightlightColor);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static UnityObject DragDropAreaSingle(Rect _rect, DragAndDropVisualMode _dropVisualMode, Color _hightlightColor, params GUILayoutOption[] _options)
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(), GUI.skin.label, _options);
            return EditorGUIExtension.DragDropAreaSingle(rect, _dropVisualMode, _hightlightColor);
        }
    }
}
