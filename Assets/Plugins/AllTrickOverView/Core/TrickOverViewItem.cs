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

namespace Plugins.AllTrickOverView.Core
{
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

        // Token: 0x0400042E RID: 1070
        public readonly string Name;
        public readonly string Description;

        public TrickOverViewItem(Type type)
        {
            this.DrawCodeExample = true;
            AExample_Base aExampleBase =
                AllTrickOverViewUtilities.GetExampleByType(type);
            if (aExampleBase == null)
            {
                return;
            }
            
            TrickOverViewInfo trickOverViewInfo = aExampleBase.GetTrickOverViewInfo();
            this.Name = trickOverViewInfo.Name;
            this.Description = trickOverViewInfo.Description;
            
            this.m_TrickOverViewPreviewDrawer = new TrickOverViewPreview(trickOverViewInfo);
            this.tabGroup = new GUITabGroup
            {
                ToolbarHeight = 30f
            };

            this.tabGroup.RegisterTab(trickOverViewInfo.Name);
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
            GUILayout.Label(this.Name, SirenixGUIStyles.SectionHeader, new GUILayoutOption[0]);

            SirenixEditorGUI.DrawThickHorizontalSeparator(4f, 10f);

            if (!string.IsNullOrEmpty(this.Description))
            {
                GUILayout.Label(this.Description, SirenixGUIStyles.MultiLineLabel, new GUILayoutOption[0]);
                SirenixEditorGUI.DrawThickHorizontalSeparator(10f, 10f);
            }
            
            if (this.m_TrickOverViewPreviewDrawer != null)
            {
                Color color = GUI.backgroundColor;
                GUI.backgroundColor = TrickOverViewItem.backgroundColor;
                this.tabGroup.BeginGroup(true, TrickOverViewItem.tabGroupStyle);
                GUI.backgroundColor = color;

                GUITabPage guitabPage = this.tabGroup.RegisterTab(this.m_TrickOverViewPreviewDrawer.ExampleInfo.Name);
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