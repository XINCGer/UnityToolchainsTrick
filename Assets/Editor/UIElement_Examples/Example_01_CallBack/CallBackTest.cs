//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-19 10:45:59
// Name: EnumFieldTest
//---------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace UIElement_Example_01
{
    internal enum EnumType
    {
        TypeA,
        TypeB,
        TypeC
    }
    
    
    public class CallBackTest : EditorWindow
    {
        [MenuItem("Element_Examples/Example_01_CallBackTest")]
        public static void ShowExample()
        {
            CallBackTest wnd = GetWindow<CallBackTest>();
            wnd.titleContent = new GUIContent("CallBackTest");
            wnd.Initial();
            wnd.Show();
        }

        public void Initial()
        {
            VisualElement root = rootVisualElement;
            var enumGUi = new EnumGUI();
            root.Add(enumGUi.Element);
        }
    }

    public class EnumGUI
    {
        private static readonly Dictionary<EnumType, string> EnumDict = new Dictionary<EnumType, string>()
        {
            {EnumType.TypeA, "This A"},
            {EnumType.TypeB, "This B"},
            {EnumType.TypeC, "This C"}
        };
        
        public VisualElement Element { get; set; }

        private EnumField _enumField;
        private Label _label;
        
        public EnumGUI()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UIElement_Examples/Example_01_CallBack/CallBackTest.uxml");
            Element = visualTree.Instantiate();
            //binding
            _enumField = Element.Q<EnumField>("EnumField");
            _label = Element.Q<Label>("Label");
            //register
            //注意在构造函数里先注册回调事件，再赋值也不会触发回调事件。
            _enumField.RegisterValueChangedCallback(OnValueChange);
            _enumField.value = EnumType.TypeC;
            _enumField.value = EnumType.TypeA;
        }

        private void OnValueChange(ChangeEvent<Enum> evn)
        {
            Debug.Log("CallBack");
            _label.text = EnumDict[(EnumType)evn.newValue];
        }
        
    }
}


