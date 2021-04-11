//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 9:18:43
//------------------------------------------------------------

using System;
using AllTrickOverView.Core;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace AllTrickOverView
{
    public class AllTrickOverViewEditorWindow : OdinMenuEditorWindow
    {
        private TrickOverViewItem exampleItem;

        private Vector2 scrollPosition;
        
        [MenuItem("Tools/AllTrickOverView")]
        public static void PopUp()
        {
            bool flag = Resources.FindObjectsOfTypeAll<AttributesExampleWindow>().Length == 0;
            AllTrickOverViewEditorWindow window = EditorWindow.GetWindow<AllTrickOverViewEditorWindow>();
            if (flag)
            {
                window.MenuWidth = 250f;
                window.position = GUIHelper.GetEditorWindowRect().AlignCenterXY(850f, 700f);
            }
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree odinMenuTree = new OdinMenuTree();
            odinMenuTree.Selection.SupportsMultiSelect = false;
            odinMenuTree.Selection.SelectionChanged += this.SelectionChanged;
            odinMenuTree.Config.DrawSearchToolbar = true;
            odinMenuTree.Config.DefaultMenuStyle.Height = 22;
            AllTrickOverViewUtilities.BuildMenuTree(odinMenuTree);
            return odinMenuTree;
        }

        private void SelectionChanged(SelectionChangedType selectionChangedType)
        {
            exampleItem?.GetExample().Destroy();

            this.exampleItem = null;

            if (base.MenuTree.Selection.SelectedValue is Type type)
            {
                this.exampleItem = AllTrickOverViewUtilities.GetItemByType(type);
            }
        }

        protected override void DrawEditors()
        {
            GUILayout.BeginArea(new Rect(4f, 0f, Mathf.Max(300f, base.position.width - this.MenuWidth - 4f), base.position.height));
            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, GUILayoutOptions.ExpandWidth(false));
            GUILayout.Space(4f);
            if (this.exampleItem != null)
            {
                this.exampleItem.Draw();
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            exampleItem?.GetExample().Destroy();
            this.exampleItem = null;
        }
    }
}