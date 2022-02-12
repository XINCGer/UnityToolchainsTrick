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

        public void Draw(params GUILayoutOption[] tabOption)
        {
            using (new GUILayout.HorizontalScope())
            {
                if (_startIndex > 0)
                {
                    GUILayout.Button("<",GUILayout.Width(25));
                }
                var maxWid = EditorGUIUtility.currentViewWidth;
                var width = 0f;
                var rect = new Rect();
                for (var i = _startIndex; i < _datas.Count; i++)
                {
                    if (width > maxWid)
                    {
                        var left = rect.width - (width - maxWid);
                        rect.x += left - 25;
                        rect.width = 25;
                        GUI.Button(rect, ">");
                        if (Event.current.isMouse)
                        {
                            _startIndex++;
                            Debug.Log("aaa");
                        }
                        
                        break;
                    }
                    
                    GUIStyle style = i == 0 ? _dragtabFirst : _dragTab;
                    rect = GUILayoutUtility.GetRect(new GUIContent(_datas[i]),style, tabOption);
                    width += rect.width;
                    var val = GUI.Toggle(rect,i == SelectedIndex,_datas[i], style);
                    if (val)
                    {
                        SelectedIndex = i;
                    }
                }
            }
        }
    }
}