using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Example_12_TabWindow
{
    public class TabBar
    {
        public int SelectedIndex { get; private set; }
        private List<string> _datas;
        private readonly GUIStyle _dragtabFirst = "dragtab first";
        private readonly GUIStyle _dragTab = "dragtab";
        private int _startIndex;



        public TabBar(List<string> tabData, int defaultSelectedIndex)
        {
            _datas = tabData;
            SelectedIndex = defaultSelectedIndex;
        }

        private Vector2 _scroll = Vector2.zero;
        public void Draw()
        {
            using (var scroll = new GUILayout.ScrollViewScope(_scroll,(GUIStyle)"horizontalscrollbar", GUILayout.Height(30f)))
            {
                _scroll = scroll.scrollPosition;
                using (new GUILayout.HorizontalScope())
                {
                    var maxWid = EditorGUIUtility.currentViewWidth;
                    var width = 0f;
                    var rect = new Rect();
                    for (var i = _startIndex; i < _datas.Count; i++)
                    {
                        GUIStyle style = i == 0 ? _dragtabFirst : _dragTab;
                        rect = GUILayoutUtility.GetRect(new GUIContent(_datas[i]), style);
                        width += rect.width;
                        var val = GUI.Toggle(rect, i == SelectedIndex, _datas[i], style);
                        if (val)
                        {
                            SelectedIndex = i;
                        }
                    }
                }

            }
        }
    }
}