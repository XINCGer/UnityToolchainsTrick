using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_ParticleSystemPreview : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("ParticleSystemPreview",
                "Particle Inspector面板拓展预览",
                "Particle",
                "using System.Reflection;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n// thanks to Sprite Animation Preview\n    public abstract class OverrideEditor : Editor\n    {\n        readonly MethodInfo methodInfo = typeof(Editor).GetMethod(\"OnHeaderGUI\",\n            BindingFlags.NonPublic | BindingFlags.Instance);\n\n        private Editor m_BaseEditor;\n\n        protected Editor baseEditor\n        {\n            get { return m_BaseEditor ?? (m_BaseEditor = GetBaseEditor()); }\n            set { m_BaseEditor = value; }\n        }\n\n        protected abstract Editor GetBaseEditor();\n\n        public override void OnInspectorGUI()\n        {\n            baseEditor.OnInspectorGUI();\n        }\n\n        public override bool HasPreviewGUI()\n        {\n            return baseEditor.HasPreviewGUI();\n        }\n\n        public override GUIContent GetPreviewTitle()\n        {\n            return baseEditor.GetPreviewTitle();\n        }\n\n        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)\n        {\n            return baseEditor.RenderStaticPreview(assetPath, subAssets, width, height);\n        }\n\n        public override void OnPreviewGUI(Rect r, GUIStyle background)\n        {\n            baseEditor.OnPreviewGUI(r, background);\n        }\n\n        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)\n        {\n            baseEditor.OnInteractivePreviewGUI(r, background);\n        }\n\n        public override void OnPreviewSettings()\n        {\n            baseEditor.OnPreviewSettings();\n        }\n\n        public override string GetInfoString()\n        {\n            return baseEditor.GetInfoString();\n        }\n\n        public override void ReloadPreviewInstances()\n        {\n            baseEditor.ReloadPreviewInstances();\n        }\n\n        protected override void OnHeaderGUI()\n        {\n            methodInfo.Invoke(baseEditor, new object[0]);\n        }\n\n        public override bool RequiresConstantRepaint()\n        {\n            return baseEditor.RequiresConstantRepaint();\n        }\n\n        public override bool UseDefaultMargins()\n        {\n            return baseEditor.UseDefaultMargins();\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_19_ParticleSystemPreview",
                typeof(Example_ParticleSystemPreview),
                picPath : "Assets/Editor/Examples/Example_19_ParticleSystemPreview/ParticleSystemPreview.cs",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
