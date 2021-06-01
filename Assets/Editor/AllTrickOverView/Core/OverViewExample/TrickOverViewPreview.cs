//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 11:23:36
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using ToolKits;
using UnityEditor;
using UnityEngine;

namespace AllTrickOverView.Core
{
    internal class TrickOverViewPreview
    {
        // Token: 0x04000430 RID: 1072
        private static GUIStyle exampleGroupStyle;

        // Token: 0x04000431 RID: 1073
        private static GUIStyle previewStyle;

        // Token: 0x04000432 RID: 1074
        private static GUIStyle codeTextStyle;

        // Token: 0x04000433 RID: 1075
        private static Color previewBackgroundColorDark = new Color32(56, 56, 56, byte.MaxValue);

        // Token: 0x04000434 RID: 1076
        private static Color previewBackgroundColorLight = new Color32(194, 194, 194, byte.MaxValue);

        // Token: 0x04000436 RID: 1078
        private PropertyTree tree;

        // Token: 0x04000437 RID: 1079
        private string highlightedCode;

        // Token: 0x04000438 RID: 1080
        private Vector2 scrollPosition;

        private Action<Rect> m_DrawCallbaclAction;

        // Token: 0x04000439 RID: 1081
        private bool showRaw;

        private AExample_Base m_Example;

        public TrickOverViewPreview(AExample_Base aExampleBase)
        {
	        m_Example = aExampleBase;
	        this.m_DrawCallbaclAction = m_Example.DrawUI;
	        try
            {
                this.highlightedCode = SyntaxHighlighter.Parse(m_Example.GetTrickOverViewInfo().Code);
            }
            catch (Exception exception)
            {
                Debug.LogError($"条目{aExampleBase.GetTrickOverViewInfo().Name}的代码块高亮失败，原因是：{exception}");
                this.highlightedCode = m_Example.GetTrickOverViewInfo().Code;
                this.showRaw = true;
            }
        }

        // Token: 0x06000A26 RID: 2598 RVA: 0x00031018 File Offset: 0x0002F218
		public void Draw(bool drawCodeExample)
		{
			if (TrickOverViewPreview.exampleGroupStyle == null)
			{
				TrickOverViewPreview.exampleGroupStyle = new GUIStyle(GUIStyle.none)
				{
					padding = new RectOffset(1, 1, 10, 0)
				};
			}
			if (TrickOverViewPreview.previewStyle == null)
			{
				TrickOverViewPreview.previewStyle = new GUIStyle(GUIStyle.none)
				{
					padding = new RectOffset(0, 0, 0, 0)
				};
			}

			GUILayout.BeginVertical(TrickOverViewPreview.exampleGroupStyle, new GUILayoutOption[0]);
			GUILayout.Label("Preview：", SirenixGUIStyles.BoldTitle, new GUILayoutOption[0]);
			GUILayout.BeginVertical(TrickOverViewPreview.previewStyle, GUILayoutOptions.ExpandWidth(true));
			Rect rect = GUIHelper.GetCurrentLayoutRect().Expand(4f, 0f);
			SirenixEditorGUI.DrawSolidRect(rect, EditorGUIUtility.isProSkin ? TrickOverViewPreview.previewBackgroundColorDark : TrickOverViewPreview.previewBackgroundColorLight, true);
			SirenixEditorGUI.DrawBorders(rect, 1, true);
			GUILayout.Space(8f);
			
			m_DrawCallbaclAction.Invoke(rect);
			this.tree = (this.tree ?? PropertyTree.Create(m_Example));
			this.tree.Draw(false);
			
			GUILayout.Space(8f);
			GUILayout.EndVertical();
			if (drawCodeExample && m_Example.GetTrickOverViewInfo().Code != null)
			{
				GUILayout.Space(12f);
				GUILayout.Label("Code", SirenixGUIStyles.BoldTitle, new GUILayoutOption[0]);
				Rect rect2 = SirenixEditorGUI.BeginToolbarBox(new GUILayoutOption[0]);
				SirenixEditorGUI.DrawSolidRect(rect2.HorizontalPadding(1f), SyntaxHighlighter.BackgroundColor, true);
				SirenixEditorGUI.BeginToolbarBoxHeader(22f);
				if (SirenixEditorGUI.ToolbarButton(this.showRaw ? "Highlighted" : "Raw", false))
				{
					this.showRaw = !this.showRaw;
				}
				GUILayout.FlexibleSpace();
				EditorGUILayoutExtension.LinkFileLabelField("点击此处定位到脚本目录", this.m_Example.GetTrickOverViewInfo().CodePath);
				GUILayout.FlexibleSpace();
				if (SirenixEditorGUI.ToolbarButton("Copy", false))
				{
					Clipboard.Copy<string>(this.m_Example.GetTrickOverViewInfo().Code);
				}
				SirenixEditorGUI.EndToolbarBoxHeader();
				if (TrickOverViewPreview.codeTextStyle == null)
				{
					TrickOverViewPreview.codeTextStyle = new GUIStyle(SirenixGUIStyles.MultiLineLabel);
					TrickOverViewPreview.codeTextStyle.normal.textColor = SyntaxHighlighter.TextColor;
					TrickOverViewPreview.codeTextStyle.active.textColor = SyntaxHighlighter.TextColor;
					TrickOverViewPreview.codeTextStyle.focused.textColor = SyntaxHighlighter.TextColor;
					TrickOverViewPreview.codeTextStyle.wordWrap = false;
				}
				GUIContent content = GUIHelper.TempContent(this.showRaw ? this.m_Example.GetTrickOverViewInfo().Code.TrimEnd(new char[]
				{
					'\n',
					'\r'
				}) : this.highlightedCode);
				Vector2 vector = TrickOverViewPreview.codeTextStyle.CalcSize(content);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Space(-3f);
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUIHelper.PushEventType((Event.current.type == EventType.ScrollWheel) ? EventType.Used : Event.current.type);
				this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, true, false, GUI.skin.horizontalScrollbar, GUIStyle.none, new GUILayoutOption[]
				{
					GUILayout.MinHeight(vector.y + 20f)
				});
				Rect rect3 = GUILayoutUtility.GetRect(vector.x + 50f, vector.y).AddXMin(4f).AddY(2f);
				if (this.showRaw)
				{
					EditorGUI.SelectableLabel(rect3, this.m_Example.GetTrickOverViewInfo().Code, TrickOverViewPreview.codeTextStyle);
					GUILayout.Space(-14f);
				}
				else
				{
					GUI.Label(rect3, content, TrickOverViewPreview.codeTextStyle);
				}
				GUILayout.EndScrollView();
				GUIHelper.PopEventType();
				GUILayout.EndVertical();
				GUILayout.Space(-3f);
				GUILayout.EndHorizontal();
				GUILayout.Space(-3f);
				SirenixEditorGUI.EndToolbarBox();
			}
			GUILayout.EndVertical();
		}
    }
}