#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 
 *
 */
#endregion
using System;
using System.Collections.Generic;
using UnityEditor;

using UnityObject = UnityEngine.Object;

namespace CZToolKit.Core.Editors
{
    public abstract class UnityObjectMenuEditorWindow : BasicMenuEditorWindow
    {
        readonly static Dictionary<Type, Editor> EditorCache = new Dictionary<Type, Editor>();

        protected override void OnRightGUI(CZMenuTreeViewItem _selectedItem)
        {
            if (_selectedItem.userData is UnityObject unityObject)
            {
                if (!EditorCache.TryGetValue(unityObject.GetType(), out Editor editor))
                    EditorCache[unityObject.GetType()] = editor = Editor.CreateEditor(unityObject);
                editor.OnInspectorGUI();
            }
        }
    }
}
