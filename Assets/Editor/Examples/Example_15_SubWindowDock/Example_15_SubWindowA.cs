using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class Example_15_SubWindowA :EditorWindow
    {
        public static Example_15_SubWindowA PopUp()
        {
            var window = GetWindow<Example_15_SubWindowA>("子窗口A");
            window.Show();
            return window;
        }
    }   
}
