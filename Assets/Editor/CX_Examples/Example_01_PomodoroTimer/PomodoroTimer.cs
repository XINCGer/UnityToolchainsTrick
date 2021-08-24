//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-08-18 22:46:44
// Name: PomodoroTimer
//---------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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

        private void OnEnable()
        {
            RefreshData();
            PomodoroTimerSO.Instance.OnUpdate += RefreshData;
        }

        private void OnDisable()
        {
            PomodoroTimerSO.Instance.OnUpdate -= RefreshData;
        }

        private void OnDestroy()
        {
            PomodoroTimerSO.Instance.OnUpdate -= RefreshData;
        }

        private List<PomodoroItem> _items;

        private void RefreshData()
        {
            var datas = PomodoroTimerSO.Instance.ItemDatas;
            if (datas == null) return;
            _items = new List<PomodoroItem>();
            for (int i = 0; i < datas.Count; i++)
            {
                _items.Add(new PomodoroItem {ItemData = datas[i]});
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Create Item"))
            {
                CreateOREditPopup.ShowWindow();
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
        private PomodoroItemData _itemData;

        public PomodoroItemData ItemData
        {
            get => _itemData;
            set
            {
                _itemData = value;
                SetGUIStyle();
            }
        }

        public void Show()
        {
            if (ItemData == null) return;

            using (new GUILayoutExt.BackgroundColorScope(SetBackGround(ItemData.Priority)))
            {
                using (new GUILayout.VerticalScope("FrameBox"))
                {
                    using (new GUILayout.HorizontalScope("FrameBox"))
                    {
                        using (new GUILayoutExt.BackgroundColorScope(Color.white))
                        {
                            bool isComplete = ItemData.IsCompleted;
                            ItemData.IsCompleted =
                                GUILayout.Toggle(isComplete, string.Empty,
                                    _toggleStyle, GUILayout.Width(32f), GUILayout.Height(30f));
                        }

                        GUILayoutExt.Label(new GUIContent(ItemData.Desc), _descStyle, ItemData.IsCompleted);
                    }

                    GUILayout.Label(ItemData.CreateTime, _timeStyle);
                    using (new GUILayout.HorizontalScope())
                    {
                        // ItemData.IsCompleted = GUILayout.Toggle();
                        using (new GUILayout.HorizontalScope())
                        {
                            var (all, left) = ItemData.GetCountInfo();
                            var finishImg = EditorGUIUtility.FindTexture("sv_icon_dot0_pix16_gizmo");
                            var leftImg = EditorGUIUtility.FindTexture("sv_icon_dot6_pix16_gizmo");
                            for (int i = all; i > 0; i--)
                            {
                                GUILayout.Label(i <= left ? leftImg : finishImg, GUILayout.Height(32f),
                                    GUILayout.Width(32f));
                            }
                        }

                        GUILayout.Button("Start", "ButtonMid", GUILayout.MaxWidth(75f), GUILayout.MaxHeight(30f));
                        if (GUILayout.Button("Edit", "ButtonMid", GUILayout.MaxWidth(75f), GUILayout.MaxHeight(30f)))
                        {
                            CreateOREditPopup.ShowWindow(ItemData);
                        }

                        if (GUILayout.Button("Delete", "ButtonMid", GUILayout.MaxWidth(75f), GUILayout.MaxHeight(30f))
                            && EditorUtility.DisplayDialog("Delete Item", "Are you sure to delete this?", "Yes",
                                "Cancel"))
                        {
                            PomodoroTimerSO.Instance.DeleteItem(ItemData);
                        }
                    }
                }
            }
        }

        private GUIStyle _descStyle;
        private GUIStyle _timeStyle;
        private GUIStyle _toggleStyle;
        private GUIStyle _timerBoxStyle;

        private void SetGUIStyle()
        {
            _timeStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleRight,
                normal =
                {
                    textColor = Color.white
                },
                fontSize = 15
            };

            _toggleStyle = new GUIStyle();
            var togImg = EditorGUIUtility.IconContent("toggle@2x");
            var actImg = EditorGUIUtility.IconContent("toggle on act@2x");
            _toggleStyle.normal.background = togImg.image as Texture2D;
            _toggleStyle.onNormal.background = actImg.image as Texture2D;

            _descStyle = new GUIStyle();
            _descStyle.fontSize = 20;
            _descStyle.normal.textColor = Color.white;

            _timerBoxStyle = new GUIStyle();
            _timerBoxStyle.stretchWidth = true;
        }

        private Color SetBackGround(ItemPriority priority)
        {
            return priority switch
            {
                ItemPriority.High => Color.red,
                ItemPriority.Normal => Color.white,
                _ => Color.grey
            };
        }
    }

    internal class CreateOREditPopup : EditorWindow
    {
        public static void ShowWindow(PomodoroItemData data = null)
        {
            var title = data == null ? "Create" : "Editor";
            var win = EditorWindow.GetWindow<CreateOREditPopup>(true, title);
            win.Initial(data);
            win.Show();
        }

        private PomodoroItemData _originData;
        private string _desc = String.Empty;
        private int _clockCount;
        private ItemPriority _priority = ItemPriority.Normal;

        private void Initial(PomodoroItemData data)
        {
            _originData = data;
            if (data == null) return;
            _desc = data.Desc;
            _clockCount = data.Duration / PomodoroTimerSO.Instance.AllTime;
            _priority = data.Priority;
        }


        private void OnGUI()
        {
            GUILayout.Label("TODO:");
            _desc = GUILayout.TextArea(_desc);
            _priority = (ItemPriority) EditorGUILayout.EnumPopup("Priority:", _priority);
            _clockCount = EditorGUILayout.IntField("Clock Count:", _clockCount);
            if (GUILayout.Button(this.titleContent.text))
            {
                PomodoroTimerSO.Instance.UpdateItem(_originData, _desc, _clockCount * PomodoroTimerSO.Instance.AllTime,
                    _priority);
                Close();
            }
        }

        private void OnLostFocus()
        {
            this.Focus();
        }
    }

    internal class ActiveTimer
    {
        private PomodoroItemData _data;

        public ActiveTimer(PomodoroItemData data)
        {
            _data = data;
        }

        public void Show()
        {
            
        }
    }
}