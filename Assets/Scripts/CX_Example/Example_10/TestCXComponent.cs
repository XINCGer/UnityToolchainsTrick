using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example_10
{
    [DisallowMultipleComponent]
    public class TestCXComponent : MonoBehaviour
    {
        private void Reset()
        {
            //这个会在第一次加组件,按Reset按钮后执行
            Debug.Log("Mono_Reset");
        }
    }
}

