//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-08-18 22:46:44
// Name: PomodoroTimer
//---------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PomodoroTimer
{
    internal class PomodoroTimerWindow : EditorWindow
    {
        [MenuItem("CX_Tools/PomodoroTimer", priority = 0)]
        public static void ShowWindow()
        {
            var win = EditorWindow.GetWindow<PomodoroTimerWindow>("Pomodoro Timer");
            win.Show();
        }

        private PomodoroTimerSO _timerSO;

        private void OnEnable()
        {
            Initial();
            RefreshData(_timerSO.ItemDatas);
        }

        private void Initial()
        {
            var guids = AssetDatabase.FindAssets("t:PomodoroTimerSO");
            if (guids == null || guids.Length <= 0)
            {
                _timerSO = ScriptableObject.CreateInstance<PomodoroTimerSO>();
                var path = Application.dataPath + "/Editor/PomodoroTimer";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                AssetDatabase.CreateAsset(this._timerSO, "Assets/Editor/PomodoroTimer/PomodoroTimerSO.asset");
                AssetDatabase.Refresh();
            }
            else
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                _timerSO = AssetDatabase.LoadAssetAtPath<PomodoroTimerSO>(path);
            }
        }

        private List<PomodoroItem> _items;

        private void RefreshData(List<PomodoroItemData> datas)
        {
            if (datas == null || datas.Count <= 0) return;
            _items ??= new List<PomodoroItem>();
            var count = _items.Count;
            for (int i = 0; i < datas.Count; i++)
            {
                if (i < count)
                    _items[i].ItemData = datas[i];
                else
                    _items.Add(new PomodoroItem {ItemData = datas[i]});
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Create Item"))
            {
                _timerSO.AddItem("Test", 100, ItemPriority.High);
                RefreshData(_timerSO.ItemDatas);
                Repaint();
            }

            if (_items == null) return;
            foreach (var item in _items)
            {
                item.Show();
            }
        }
    }

    internal class PomodoroItem
    {
        public PomodoroItemData ItemData { get; set; }

        public void Show()
        {
            if (ItemData == null) return;
            GUILayout.Box(SetBackGround(ItemData.Priority)/*, "FrameBox"*/);
            GUILayout.Label(ItemData.CreateTime);
        }

        private Texture2D SetBackGround(ItemPriority priority)
        {
            return priority switch
            {
                ItemPriority.High => Texture2D.redTexture,
                ItemPriority.Normal => Texture2D.whiteTexture,
                _ => Texture2D.linearGrayTexture
            };
        }
    }
}