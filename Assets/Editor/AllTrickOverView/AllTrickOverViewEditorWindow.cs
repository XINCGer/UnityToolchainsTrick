//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 9:18:43
//------------------------------------------------------------

using System;
using AllTrickOverView.Core;
using AllTrickOverView.Core.ExampleFactory;
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

        private PropertyTree m_ExamplePropertyTree;
        private ExampleTemplate m_ExampleTemplate = new ExampleTemplate();

        private Vector2 scrollPosition;

        private bool m_ShouldDrawExampleCreatorUI;

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

        protected override void OnGUI()
        {
            GUILayout.BeginHorizontal();

            GUI.color = Color.HSVToRGB(
                Mathf.Cos((float) UnityEditor.EditorApplication.timeSinceStartup + 1f) * 0.125f + 0.325f, 1, 1);
            if (GUILayout.Button(
                new GUIContent("Create a Example OverView", EditorGUIUtility.FindTexture("Toolbar Plus")),
                "toolbarbutton", GUILayout.Width(200)))
            {
                m_ShouldDrawExampleCreatorUI = true;
            }

            GUI.color = Color.white;
            GUILayout.EndHorizontal();
            base.OnGUI();
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
            m_ShouldDrawExampleCreatorUI = false;
            this.exampleItem = null;
            if (base.MenuTree.Selection.SelectedValue is Type type)
            {
                this.exampleItem = AllTrickOverViewUtilities.GetItemByType(type);
                //每次选择的TreeView变化时都要进行Init
                this.exampleItem.GetExample().Init();
            }
        }

        /// <summary>
        /// 绘制用于Example创建的UI
        /// </summary>
        private void DrawExampleCreatorEditor()
        {
            if (m_ExamplePropertyTree == null)
            {
                if (m_ExampleTemplate == null)
                {
                    m_ExampleTemplate = new ExampleTemplate();
                }

                m_ExamplePropertyTree = PropertyTree.Create(m_ExampleTemplate);
            }

            GUILayout.Label("自动生成OverViewExample代码", SirenixGUIStyles.SectionHeader, new GUILayoutOption[0]);

            SirenixEditorGUI.DrawThickHorizontalSeparator(4f, 10f);
            m_ExamplePropertyTree.Draw(false);
        }

        protected override void DrawEditors()
        {
            GUILayout.BeginArea(new Rect(4f, 0f, Mathf.Max(300f, base.position.width - this.MenuWidth - 4f),
                base.position.height));
            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, GUILayoutOptions.ExpandWidth(false));
            GUILayout.Space(4f);
            if (m_ShouldDrawExampleCreatorUI)
            {
                DrawExampleCreatorEditor();
            }
            else if (this.exampleItem != null)
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