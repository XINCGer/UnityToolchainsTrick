//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-08-19 00:17:27
// Name: PomodoroTimerSO
//---------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PomodoroTimer
{
    internal class PomodoroTimerSO : ScriptableObject
    {
        public List<PomodoroItemData> ItemDatas;
        public int WorkTime = 25;
        public int BreakTime = 5;
        public string TempTimer;
        public int AllTime => WorkTime + BreakTime;

        private static PomodoroTimerSO _instance;

        public event Action OnUpdate;

        public static PomodoroTimerSO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var guids = AssetDatabase.FindAssets("t:PomodoroTimerSO");
                if (guids == null || guids.Length <= 0)
                {
                    _instance = ScriptableObject.CreateInstance<PomodoroTimerSO>();
                    var path = Application.dataPath + "/Editor/PomodoroTimer";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    AssetDatabase.CreateAsset(_instance, "Assets/Editor/PomodoroTimer/PomodoroTimerSO.asset");
                    AssetDatabase.Refresh();
                }
                else
                {
                    var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _instance = AssetDatabase.LoadAssetAtPath<PomodoroTimerSO>(path);
                }

                return _instance;
            }
        }

        public void UpdateItem(PomodoroItemData data, string desc, int duration, ItemPriority priority)
        {
            if (data == null)
            {
                var newItem = new PomodoroItemData(desc, duration, priority);
                ItemDatas ??= new List<PomodoroItemData>();
                ItemDatas.Add(newItem);
            }
            else
            {
                data.SetData(desc, duration, priority);
            }

            OnUpdate?.Invoke();
            SaveData();
        }

        public void DeleteItem(PomodoroItemData data)
        {
            if (ItemDatas.Contains(data))
            {
                var res = ItemDatas.Remove(data);
                if (res)
                {
                    OnUpdate?.Invoke();
                    SaveData();
                }
                else
                {
                    Debug.LogError("Delete Error!");
                }
                return;
            }
            Debug.LogError("Not found Data");
        }

        public void SaveData()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }

    internal enum ItemStatus
    {
        Ready,
        Start,
        Suspended,
        OverTime,
        Completed
    }

    internal enum ItemPriority
    {
        High,
        Normal,
        Low
    }

    [Serializable]
    internal class PomodoroItemData
    {
        public string Desc;
        public int Duration;
        public ItemPriority Priority;
        [SerializeField] private string _createTime;
        public string CreateTime => _createTime;
        [SerializeField] private int _leftTime;
        [SerializeField] private ItemStatus _status;

        public bool IsCompleted
        {
            get => _status == ItemStatus.Completed;
            set { _status = value ? ItemStatus.Completed : ItemStatus.Ready; }
        }

        public (int all, int left) GetCountInfo()
        {
            var allTime = PomodoroTimerSO.Instance.AllTime;
            return (Duration / allTime, Duration / allTime);
        }

        public PomodoroItemData(string desc, int duration, ItemPriority priority)
        {
            var now = DateTime.Now;
            _createTime = now.ToString("MM/dd/yyyy");
            Duration = duration;
            _status = ItemStatus.Ready;
            SetData(desc, duration, priority);
        }

        public void SetData(string desc, int duration, ItemPriority priority)
        {
            Desc = desc;
            var diff = duration - Duration;
            Duration = duration;
            _leftTime += diff;
            _leftTime = _leftTime < 0 ? 0 : _leftTime;
            Priority = priority;
        }
    }
}