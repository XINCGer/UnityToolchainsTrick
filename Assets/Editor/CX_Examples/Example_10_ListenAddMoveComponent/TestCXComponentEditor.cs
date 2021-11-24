
using System;
using UnityEditor;
using UnityEngine;

namespace Example_10
{
    [CustomEditor(typeof(TestCXComponent))]
    public class TestCXComponentEditor : Editor
    {
        private void Reset()
        {
            //这个会在第一次加组件, 退出PlayMode，脚本编译后会调用
            Debug.Log("Editor_Reset");
        }

        private void OnDisable()
        {
            //在移除脚本，退出playmode时会调用
            if (target == null)
            {
                Debug.Log("Editor_OnDisable");
            }
        }
    }
}

