using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Example_12_TabWindow
{
    public class TabWindow : EditorWindow
    {
        [MenuItem("CX_Tools/TabWindow")]
        private static void ShowWindow()
        {
            var window = GetWindow<TabWindow>();
            window.titleContent = new UnityEngine.GUIContent("TabWindow");
            window.Initial();
            window.Show();
        }

        private List<DataSetting> _originDatas;
        private Dictionary<string, List<DataSetting>> _fullDataDict;
        private string _activeTabName;

        // private List<DataSetting> _activeDatas
        // {
        //     get
        //     {
        //         
        //     }
        // }

        private void Initial()
        {
            _originDatas = new List<DataSetting>();
            //_activeDatas = new List<DataSetting>();
            //_fullDatas = 
        }
    }
}

