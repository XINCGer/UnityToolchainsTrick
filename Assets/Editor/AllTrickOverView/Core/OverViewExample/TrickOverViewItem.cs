//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 11:17:35
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace AllTrickOverView.Core
{
    /// <summary>
    /// 主要用于绘制与TreeViewItem数据本身关联不大的UI，更加核心的部分例如TreeViewItem数据，TreeViewItem的自定义绘制会在TrickOverViewPreview中进行
    /// TreeViewItem会调用TrickOverViewPreview的绘制函数
    /// </summary>
    public class TrickOverViewItem
    {
        public bool DrawCodeExample { get; set; }

        // Token: 0x04000427 RID: 1063
        private static GUIStyle headerGroupStyle;

        // Token: 0x04000428 RID: 1064
        private static GUIStyle tabGroupStyle;

        // Token: 0x04000429 RID: 1065
        private static Color backgroundColor = new Color32(195, 195, 195, byte.MaxValue);

        // Token: 0x0400042C RID: 1068
        private TrickOverViewPreview m_TrickOverViewPreviewDrawer;

        // Token: 0x0400042D RID: 1069
        private GUITabGroup tabGroup;

        private AExample_Base m_Example;
        
        public AExample_Base GetExample()
        {
            return this.m_Example;
        }
        
        public TrickOverViewItem(AExample_Base aExampleBase)
        {
            this.DrawCodeExample = true;
            if (aExampleBase == null)
            {
                Debug.LogError("AExampleBase数据为空，请检查类型");
                return;
            }
            m_Example = aExampleBase;

            this.m_TrickOverViewPreviewDrawer = new TrickOverViewPreview(m_Example);
            this.tabGroup = new GUITabGroup
            {
                ToolbarHeight = 30f
            };

            this.tabGroup.RegisterTab(m_Example.GetTrickOverViewInfo().Name);
        }

        // Token: 0x06000A23 RID: 2595 RVA: 0x00030DD4 File Offset: 0x0002EFD4
        [OnInspectorGUI]
        public void Draw()
        {
            GUIStyle guistyle;
            if ((guistyle = TrickOverViewItem.headerGroupStyle) == null)
            {
                (guistyle = new GUIStyle()).padding = new RectOffset(4, 6, 10, 4);
            }

            TrickOverViewItem.headerGroupStyle = guistyle;
            GUIStyle guistyle2;
            if ((guistyle2 = TrickOverViewItem.tabGroupStyle) == null)
            {
                (guistyle2 = new GUIStyle(SirenixGUIStyles.BoxContainer)).padding = new RectOffset(0, 0, 0, 0);
            }

            TrickOverViewItem.tabGroupStyle = guistyle2;
            GUILayout.BeginVertical(TrickOverViewItem.headerGroupStyle, new GUILayoutOption[0]);
            GUILayout.Label(this.m_Example.GetTrickOverViewInfo().Name, SirenixGUIStyles.SectionHeader, new GUILayoutOption[0]);

            SirenixEditorGUI.DrawThickHorizontalSeparator(4f, 10f);

            if (!string.IsNullOrEmpty(this.m_Example.GetTrickOverViewInfo().Description))
            {
                GUILayout.Label(this.m_Example.GetTrickOverViewInfo().Description, SirenixGUIStyles.MultiLineLabel, new GUILayoutOption[0]);
                SirenixEditorGUI.DrawThickHorizontalSeparator(10f, 10f);
            }
            
            if (this.m_TrickOverViewPreviewDrawer != null)
            {
                Color color = GUI.backgroundColor;
                GUI.backgroundColor = TrickOverViewItem.backgroundColor;
                this.tabGroup.BeginGroup(true, TrickOverViewItem.tabGroupStyle);
                GUI.backgroundColor = color;

                GUITabPage guitabPage = this.tabGroup.RegisterTab(this.m_Example.GetTrickOverViewInfo().Name);
                if (guitabPage.BeginPage())
                {
                    m_TrickOverViewPreviewDrawer.Draw(this.DrawCodeExample);
                }

                guitabPage.EndPage();

                this.tabGroup.EndGroup();
            }
            else
            {
                GUILayout.Label("No examples available.", new GUILayoutOption[0]);
            }

            GUILayout.EndVertical();
        }
    }
}