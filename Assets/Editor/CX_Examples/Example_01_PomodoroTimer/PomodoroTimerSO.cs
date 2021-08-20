//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-08-19 00:17:27
// Name: PomodoroTimerSO
//---------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PomodoroTimer
{
    internal class PomodoroTimerSO : ScriptableObject
    {
        public List<PomodoroItemData> ItemDatas;

        public void AddItem(string desc, int duration, ItemPriority priority)
        {
            var newItem = new PomodoroItemData(desc, duration, priority);
            ItemDatas ??= new List<PomodoroItemData>();
            ItemDatas.Add(newItem);
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
        [SerializeField] private string _createTime;
        public string CreateTime => _createTime;
        [SerializeField] private string _timer;
        [SerializeField] private int _duration;
        [SerializeField] private int _leftTime;
        [SerializeField] private ItemStatus _status;
        public ItemPriority Priority;
        
        public bool IsCompleted
        {
            get => _status == ItemStatus.Completed;
            set { _status = value ? ItemStatus.Completed : ItemStatus.Ready; }
        }

        public PomodoroItemData(string desc, int duration, ItemPriority priority)
        {
            Desc = desc;
            _createTime = DateTime.Now.ToString("MM/dd/yyyy");
            _duration = duration;
            _leftTime = duration;
            _status = ItemStatus.Ready;
            Priority = priority;
        }
    }
}