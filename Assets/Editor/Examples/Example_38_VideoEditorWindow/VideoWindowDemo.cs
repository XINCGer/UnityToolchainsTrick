using UnityEditor;

public class VideoWindowDemo
{
    [MenuItem("Tools/VideoEditorWindow", priority = 38)]
    private static void ShowWindow()
    {
        VideoWindow.ShowVideo("Assets/Editor/Examples/Example_15_SubWindowDock/SubWindowDockVideo.mp4");
    }
}