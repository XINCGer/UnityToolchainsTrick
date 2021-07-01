using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKits
{
    [CreateAssetMenu(fileName = "Example_55_Scriptobj.asset", menuName = "CustomAssets/Example_55_Scriptobj")]
    public class Example_55_Scriptobj : ScriptableObject
    {
        public string Name;
        public int Age;
        public List<string> Skill;
    }   
}
