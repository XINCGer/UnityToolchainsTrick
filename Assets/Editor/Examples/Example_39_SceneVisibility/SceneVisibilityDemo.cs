using UnityEditor;
using UnityEngine;

public class SceneVisibilityDemo : Editor
{
    [MenuItem("Tools/SceneVisibility/Show", priority = 39)]
    public static void Show()
    {
        GameObject demoGo = GameObject.Find("SceneVisibilityDemo");
        SceneVisibilityManager.instance.Show(demoGo, true);
    }

    [MenuItem("Tools/SceneVisibility/Hide", priority = 39)]
    public static void Hide()
    {
        GameObject demoGo = GameObject.Find("SceneVisibilityDemo");
        SceneVisibilityManager.instance.Hide(demoGo, true);
    }
    
    [MenuItem("Tools/SceneVisibility/ShowAll", priority = 39)]
    public static void ShowAll()
    {
        SceneVisibilityManager.instance.ShowAll();
    }
    
    [MenuItem("Tools/SceneVisibility/HideAll", priority = 39)]
    public static void HideAll()
    {
        SceneVisibilityManager.instance.HideAll();
    }
    
    [MenuItem("Tools/SceneVisibility/DisablePicking", priority = 39)]
    public static void DisablePicking()
    {
        GameObject demoGo = GameObject.Find("SceneVisibilityDemo");
        SceneVisibilityManager.instance.DisablePicking(demoGo, true);
    }
    
    [MenuItem("Tools/SceneVisibility/EnablePicking", priority = 39)]
    public static void EnablePicking()
    {
        GameObject demoGo = GameObject.Find("SceneVisibilityDemo");
        SceneVisibilityManager.instance.EnablePicking(demoGo, true);
    }
    
    [MenuItem("Tools/SceneVisibility/TogglePicking", priority = 39)]
    public static void TogglePicking()
    {
        GameObject demoGo = GameObject.Find("SceneVisibilityDemo");
        SceneVisibilityManager.instance.TogglePicking(demoGo, true);
    }
    
    [MenuItem("Tools/SceneVisibility/ToggleVisibility", priority = 39)]
    public static void ToggleVisibility()
    {
        GameObject demoGo = GameObject.Find("SceneVisibilityDemo");
        SceneVisibilityManager.instance.ToggleVisibility(demoGo, true);
    }
    
    [MenuItem("Tools/SceneVisibility/IsHidden", priority = 39)]
    public static void IsHidden()
    {
        GameObject demoGo = GameObject.Find("SceneVisibilityDemo");
        Debug.Log(SceneVisibilityManager.instance.IsHidden(demoGo, true));
    }
    
    [MenuItem("Tools/SceneVisibility/IsPickingDisabled", priority = 39)]
    public static void IsPickingDisabled()
    {
        GameObject demoGo = GameObject.Find("SceneVisibilityDemo");
        Debug.Log(SceneVisibilityManager.instance.IsPickingDisabled(demoGo, true));
    }
}