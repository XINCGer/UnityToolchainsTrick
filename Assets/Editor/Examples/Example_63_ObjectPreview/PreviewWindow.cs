using CZToolKit.Core;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.Examples
{
    public class ObjectPreviewWindow : EditorWindow
    {
        [MenuItem("Tools/Object Preview Window")]
        public static void Open()
        {
            GetWindow<ObjectPreviewWindow>();
        }

        public Editor modelEditor, animEditor, meshEditor, matEditor, textureEditor, audioEditor;

        private void OnEnable()
        {
            UnityEngine.Object model =
                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(
                    "Assets/Editor/Examples/Example_63_ObjectPreview/SF_Animal_Boar.fbx");
            modelEditor = Editor.CreateEditor(model);
            AnimationClip _clip = null;

            foreach (var item in AssetDatabase.LoadAllAssetRepresentationsAtPath(
                         "Assets/Editor/Examples/Example_63_ObjectPreview/SF_Animal_Boar.fbx"))
            {
                switch (item)
                {
                    case AnimationClip animationClip:
                        if (animEditor == null)
                            animEditor = Editor.CreateEditor(animationClip);
                        _clip = animationClip;
                        continue;
                    case Mesh mesh:
                        if (meshEditor == null)
                            meshEditor = Editor.CreateEditor(mesh);
                        continue;
                    case Material mat:
                        if (matEditor == null)
                            matEditor = Editor.CreateEditor(mat);
                        continue;
                    default:
                        break;
                }
            }

            Type animationClipEditorType =
                Assembly.Load("UnityEditor.CoreModule").GetType("UnityEditor.AnimationClipEditor");
            MethodInfo initMethod =
                animationClipEditorType.GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic);
            initMethod.Invoke(animEditor, null);
            
            var avatarPreviewFieldInfo =
                animationClipEditorType.GetField("m_AvatarPreview", BindingFlags.Instance | BindingFlags.NonPublic);
            var avatarPreviewValue = avatarPreviewFieldInfo.GetValue(animEditor);
            
            Type avatarPreviewType = Type.GetType("UnityEditor.AvatarPreview,UnityEditor.CoreModule");
            var timeControlFieldInfo =
                avatarPreviewType.GetField("timeControl", BindingFlags.Instance | BindingFlags.Public);
            var timeControlValue = timeControlFieldInfo.GetValue(avatarPreviewValue);
            
            var timeControlType = Type.GetType("UnityEditor.TimeControl,UnityEditor.CoreModule");
            var stopTimeFieldInfo = timeControlType.GetField("stopTime", BindingFlags.Instance | BindingFlags.Public);
            stopTimeFieldInfo.SetValue(timeControlValue, _clip.length);

            UnityEngine.Object texture =
                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(
                    "Assets/Editor/Examples/Example_63_ObjectPreview/Wallpaper 5.png");
            textureEditor = Editor.CreateEditor(texture);

            UnityEngine.Object audioClip =
                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(
                    "Assets/Editor/Examples/Example_63_ObjectPreview/Click.wav");
            audioEditor = Editor.CreateEditor(audioClip);
        }

        private void OnGUI()
        {
            modelEditor.DrawPreview(new Rect(50, 50, 300, 300));
            animEditor.DrawPreview(new Rect(400, 50, 300, 300));

            meshEditor.DrawPreview(new Rect(50, 400, 300, 300));
            matEditor.DrawPreview(new Rect(400, 400, 300, 300));

            textureEditor.DrawPreview(new Rect(750, 50, 300, 300));

            GUILayout.BeginArea(new Rect(750, 375, 300, 50));
            GUILayout.BeginHorizontal();
            audioEditor.OnPreviewSettings();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            audioEditor.DrawPreview(new Rect(750, 400, 300, 300));

            Repaint();
        }
    }
}