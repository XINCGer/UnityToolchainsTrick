using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_UnityEditorFocus : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("UnityEditorFocus",
                "Unity编辑器Focus监听示例",
                "Project",
                "using System;\nusing UnityEditor;\nusing UnityEngine;\n\n[InitializeOnLoad]\npublic class UnityFocusUtility\n{\n    public static event Action<bool> OnUnityEditorFocus = (focus) => { };\n    private static bool _appFocused;\n    static UnityFocusUtility()\n    {\n        EditorApplication.update += Update;\n    }\n    \n    private static void Update()\n    {\n        if (!_appFocused && UnityEditorInternal.InternalEditorUtility.isApplicationActive)\n        {\n            _appFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;\n            OnUnityEditorFocus(true);\n            Debug.Log(\"On focus window!\");\n        }\n        else if (_appFocused && !UnityEditorInternal.InternalEditorUtility.isApplicationActive)\n        {\n            _appFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;\n            OnUnityEditorFocus(false);\n            Debug.Log(\"On lost focus\");\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_50_UnityEditorFocus",
                typeof(Example_UnityEditorFocus),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
