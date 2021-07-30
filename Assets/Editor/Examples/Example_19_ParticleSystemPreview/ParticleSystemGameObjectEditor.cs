using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ToolKits
{
    [CustomEditor(typeof(GameObject)), CanEditMultipleObjects]
    public class ParticleSystemGameObjectEditor : OverrideEditor
    {
        private class Styles
        {
            public GUIContent ps = new GUIContent("PS", "Show particle system preview");
            public GUIStyle preButton = "preButton";
        }

        private bool m_ShowParticlePreview;

        private int m_DefaultHasPreview;

        private ParticleSystemPreview m_Preview;

        private static Styles s_Styles;

        private ParticleSystemPreview preview
        {
            get
            {
                if (m_Preview == null)
                {
                    m_Preview = new ParticleSystemPreview();
                    m_Preview.SetEditor(this);
                    m_Preview.Initialize(targets);
                }

                return m_Preview;
            }
        }

        protected override Editor GetBaseEditor()
        {
            Editor editor = null;
            var baseType = typeof(Editor).Assembly.GetType("UnityEditor.GameObjectInspector");
            CreateCachedEditor(targets, baseType, ref editor);
            return editor;
        }

        void OnEnable()
        {
            m_ShowParticlePreview = true;
        }

        void OnDisable()
        {
            preview.OnDestroy();
            if (null != baseEditor&& IsPreviewCacheNotNull)
            {
                DestroyImmediate(baseEditor);
                baseEditor = null;
            }
        }

        private bool HasParticleSystemPreview()
        {
            return preview.HasPreviewGUI();
        }

        private bool HasBasePreview()
        {
            if (m_DefaultHasPreview == 0)
            {
                m_DefaultHasPreview = baseEditor.HasPreviewGUI() ? 1 : -1;
            }

            return m_DefaultHasPreview == 1;
        }

        private bool IsShowParticleSystemPreview()
        {
            return HasParticleSystemPreview() && m_ShowParticlePreview;
        }

        public override bool HasPreviewGUI()
        {
            return HasParticleSystemPreview() || HasBasePreview();
        }

        public override GUIContent GetPreviewTitle()
        {
            return IsShowParticleSystemPreview() ? preview.GetPreviewTitle() : baseEditor.GetPreviewTitle();
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            if (IsShowParticleSystemPreview())
            {
                preview.OnPreviewGUI(r, background);
            }
            else
            {
                baseEditor.OnPreviewGUI(r, background);
            }
        }

        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            if (IsShowParticleSystemPreview())
            {
                preview.OnInteractivePreviewGUI(r, background);
            }
            else
            {
                baseEditor.OnInteractivePreviewGUI(r, background);
            }
        }

        public override void OnPreviewSettings()
        {
            if (s_Styles == null)
            {
                s_Styles = new Styles();
            }

            if (HasBasePreview() && HasParticleSystemPreview())
            {
                m_ShowParticlePreview = GUILayout.Toggle(m_ShowParticlePreview, s_Styles.ps, s_Styles.preButton);
            }

            if (IsShowParticleSystemPreview())
            {
                preview.OnPreviewSettings();
            }
            else
            {
                baseEditor.OnPreviewSettings();
            }
        }

        public override string GetInfoString()
        {
            return IsShowParticleSystemPreview() ? preview.GetInfoString() : baseEditor.GetInfoString();
        }

        public override void ReloadPreviewInstances()
        {
            if (IsShowParticleSystemPreview())
            {
                preview.ReloadPreviewInstances();
            }
            else
            {
                baseEditor.ReloadPreviewInstances();
            }
        }

#if UNITY_2020_2_OR_NEWER        
        public void OnSceneDrag(SceneView sceneView, int index)
        {
            System.Type t = baseEditor.GetType();
            MethodInfo onSceneDragMi = t.GetMethod("OnSceneDrag",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (onSceneDragMi != null)
            {
                object[] param = new object[] {sceneView, index};
                onSceneDragMi.Invoke(baseEditor, param);
            }
        }
#else
        // /// <summary>
        // /// 需要调用 GameObjectInspector 的场景拖曳，否则无法拖动物体到 Scene 视图
        // /// </summary>
        // /// <param name="sceneView"></param>
        public void OnSceneDrag(SceneView sceneView)
        {
            System.Type t = baseEditor.GetType();
            MethodInfo onSceneDragMi = t.GetMethod("OnSceneDrag",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (onSceneDragMi != null)
            {
                onSceneDragMi.Invoke(baseEditor, new object[1] {sceneView});
            }
        }
#endif

        private bool IsPreviewCacheNotNull
        {
            get
            {
                System.Type t = baseEditor.GetType();
                var fieldInfo = t.GetField("m_PreviewCache", BindingFlags.NonPublic | BindingFlags.Instance);
                if (null != fieldInfo)
                {
                    var value = fieldInfo.GetValue(baseEditor);
                    return null != value;
                }

                return false;
            }
        }
    }
}
