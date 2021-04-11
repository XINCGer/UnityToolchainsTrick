using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeveloperMode : Editor
{
    [MenuItem("Tools/DeveloperMode/EnableDeveloperMode", priority = 36)]
    private static void Enable()
    {
        UnityEditor.EditorPrefs.SetBool("DeveloperMode", true);
    }
    
    [MenuItem("Tools/DeveloperMode/DisableDeveloperMode", priority = 36)]
    private static void Disable()
    {
        UnityEditor.EditorPrefs.SetBool("DeveloperMode", false);
    }
}
