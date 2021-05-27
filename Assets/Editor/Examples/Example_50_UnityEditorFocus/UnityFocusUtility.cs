using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class UnityFocusUtility
{
    public static event Action<bool> OnUnityEditorFocus = (focus) => { };
    private static bool _appFocused;
    static UnityFocusUtility()
    {
        EditorApplication.update += Update;
    }
    
    private static void Update()
    {
        if (!_appFocused && UnityEditorInternal.InternalEditorUtility.isApplicationActive)
        {
            _appFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
            OnUnityEditorFocus(true);
            Debug.Log("On focus window!");
        }
        else if (_appFocused && !UnityEditorInternal.InternalEditorUtility.isApplicationActive)
        {
            _appFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
            OnUnityEditorFocus(false);
            Debug.Log("On lost focus");
        }
    }
}