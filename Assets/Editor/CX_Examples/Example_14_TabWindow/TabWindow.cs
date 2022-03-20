using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Example_12_TabWindow
{
    public class TabWindow : EditorWindow
    {
        [MenuItem("CX_Tools/TabWindow", priority = 6)]
        private static void ShowWindow()
        {
            var window = GetWindow<TabWindow>();
            window.titleContent = new UnityEngine.GUIContent("TabWindow");
            window.Initial();
            window.Show();
        }

        private List<DataSetting> _originDatas;
        private Dictionary<string, List<DataSetting>> _fullDataDict;
        private List<string> _tabNameList;
        private string _activeTabName;
        private TabBar _tabBar;

        private List<DataSetting> _activeDatas
        {
            get
            {
                if (!string.IsNullOrEmpty(_activeTabName) && _fullDataDict.TryGetValue(_activeTabName, out var datas))
                {
                    return datas;
                }
                return new List<DataSetting>();
            }
        }

        private void Initial()
        {
            _originDatas = new List<DataSetting>
            {
                new DataSetting{Name = "AA", TabName = null},
                new DataSetting{Name = "BB", TabName = "01"},
                new DataSetting{Name = "CC", TabName = "02"},
                new DataSetting{Name = "sss", TabName = "02"},
                new DataSetting{Name = "dsds", TabName = "02"},
                new DataSetting{Name = "CC", TabName = "ddddd"}
            };
            _fullDataDict = new Dictionary<string, List<DataSetting>>();
            //add default
            _fullDataDict["Default"] = new List<DataSetting>();
            foreach (var data in _originDatas)
            {
                if (string.IsNullOrEmpty(data.TabName))
                {
                    _fullDataDict["Default"].Add(data);
                }
                else
                {
                    if (_fullDataDict.TryGetValue(data.TabName, out var list))
                    {
                        list.Add(data);
                    }
                    else
                    {
                        _fullDataDict[data.TabName] = new List<DataSetting> {data};
                    }
                }
            }
            _tabNameList = new List<string>(_fullDataDict.Keys);
            _tabBar = new TabBar(_tabNameList, 0);
        }

        private void OnGUI()
        {
            _tabBar.Draw();
            var datas = _fullDataDict[_tabNameList[_tabBar.SelectedIndex]];
            foreach (var data in datas)
            {
                GUILayout.Label(data.Name);
            }
        }
    }
}

