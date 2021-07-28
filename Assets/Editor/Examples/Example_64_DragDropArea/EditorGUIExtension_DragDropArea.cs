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
    public static partial class EditorGUIExtension
    {
        static readonly DragAndDropVisualMode dropVisualMode = DragAndDropVisualMode.Copy;
        static readonly Color dragDropHighlightColor = new Color(0f, 1f, 1f, 0.3f);

        public static DragAndDropVisualMode DropVisualMode { get { return dropVisualMode; } }
        public static Color DragDropHighlightColor { get { return dragDropHighlightColor; } }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static Object[] DragDropAreaMulti(Rect _rect)
        {
            return DragDropAreaMulti(_rect, DropVisualMode, DragDropHighlightColor);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static Object[] DragDropAreaMulti(Rect _rect, DragAndDropVisualMode _dropVisualMode)
        {
            return DragDropAreaMulti(_rect, _dropVisualMode, DragDropHighlightColor);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static Object[] DragDropAreaMulti(Rect _rect, Color _hightlightColor)
        {
            return DragDropAreaMulti(_rect, DropVisualMode, _hightlightColor);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static Object[] DragDropAreaMulti(Rect _rect, DragAndDropVisualMode _dropVisualMode, Color _hightlightColor)
        {
            Event evt = Event.current;

            if (!_rect.Contains(evt.mousePosition)) return null;

            Object[] temp = null;

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    DragAndDrop.visualMode = _dropVisualMode;
                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        temp = DragAndDrop.objectReferences;
                    }
                    Event.current.Use();
                    break;
                case EventType.Repaint:
                    if (DragAndDrop.visualMode == _dropVisualMode)
                        EditorGUI.DrawRect(_rect, _hightlightColor);
                    break;
                default:
                    break;
            }

            return temp;
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static Object DragDropAreaSingle(Rect _rect)
        {
            return DragDropAreaSingle(_rect, DropVisualMode, DragDropHighlightColor);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static Object DragDropAreaSingle(Rect _rect, DragAndDropVisualMode _dropVisualMode)
        {
            return DragDropAreaSingle(_rect, _dropVisualMode, DragDropHighlightColor);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static Object DragDropAreaSingle(Rect _rect, Color _hightlightColor)
        {
            return DragDropAreaSingle(_rect, DropVisualMode, _hightlightColor);
        }

        /// <summary> 绘制一个可接收拖拽资源的区域 </summary>
        public static Object DragDropAreaSingle(Rect _rect, DragAndDropVisualMode _dropVisualMode, Color _hightlightColor)
        {
            Object[] temp = DragDropAreaMulti(_rect, _dropVisualMode, _hightlightColor);
            if (temp == null || temp.Length == 0) return null;

            for (int i = temp.Length - 1; i >= 0; i--)
            {
                if (temp[i] != null) return temp[i];
            }

            return null;
        }
    }
}
