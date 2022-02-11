using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Example_12_ReorderableList
{
    public class ReorderableListWindow : EditorWindow
    {
        [MenuItem("CX_Tools/ReorderableList")]
        private static void ShowWindow()
        {
            var window = GetWindow<ReorderableListWindow>();
            window.titleContent = new UnityEngine.GUIContent("ReorderableList");
            window.Show();
        }

        private List<TestItem> TestList;

        private void OnEnable()
        {
            if (TestList != null) return;
            TestList = new List<TestItem>
            {
                new TestItem{item = 1},
                new TestItem{item = 2},
                new TestItem{item = 3},
                new TestItem{item = 4},
            };
        }

        private ReorderableList _reorderableList;

        private void OnGUI()
        {
            if (_reorderableList == null)
                _reorderableList = new ReorderableList(TestList, typeof(TestItem));
            //是否能改变顺序
            _reorderableList.draggable = false;
            //title
            _reorderableList.drawHeaderCallback = rect => GUI.Label(rect, "标题");
            //设定整个列表元素的高度
            _reorderableList.elementHeightCallback = index => 2 * EditorGUIUtility.singleLineHeight;
            // 绘画元素时的callback
            _reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var testItem = TestList[index];
                rect.height = EditorGUIUtility.singleLineHeight;
                testItem.item = EditorGUI.IntField(rect, "intval", testItem.item);
                rect.y += EditorGUIUtility.singleLineHeight;
                if (GUI.Button(rect,"Test"))
                {
                    Debug.Log($"{index}:{TestList[index].item}");
                }
            };
        
            // +ボタンが押された時のコールバック
            _reorderableList.onAddCallback = list => Debug.Log("+ clicked.");

            // -ボタンが押された時のコールバック
            _reorderableList.onRemoveCallback = list => Debug.Log("- clicked : " + list.index + ".");
            //draw
            _reorderableList.DoLayoutList();
            
        }

        public class TestItem
        {
            public int item;
        }
    }
}