using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    [CreateAssetMenu(fileName = "ExampleAssets.asset", menuName = "CustomAssets/ExampleAsstes")]
    public class ExampleScriptableObject : ScriptableObject
    {
        [Tooltip("姓名")] public string Name;

        [Tooltip("年龄")] public int Age;
    }
}