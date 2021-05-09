#region 注 释
/***
 *
 *  Title:
 *  ObjectEditor
 *  Description:
 *  绘制一个普通对象，可通过继承自定义绘制方式
 *  Date:
 *  Version:
 *  Writer: 
 *
 */
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CZToolKit.Core.Editors
{

    public class ObjectEditor
    {
        static Dictionary<Type, Type> ObjectEditorTypeCache;

        static ObjectEditor()
        {
            if (ObjectEditorTypeCache == null)
                BuildCache();
        }

        static void BuildCache()
        {
            ObjectEditorTypeCache = new Dictionary<Type, Type>();

            foreach (var type in TypeCache.GetTypesDerivedFrom<ObjectEditor>())
            {
                if (Utility.TryGetTypeAttribute(type, out CustomObjectEditorAttribute att))
                    ObjectEditorTypeCache[att.targetType] = type;
            }
        }

        public static ObjectEditor CreateEditor(object _targetObject)
        {
            Type objectEditorType;
            ObjectEditor objectEditor;
            if (ObjectEditorTypeCache.TryGetValue(ObjectInspector.Instance.targetObject.GetType(), out objectEditorType))
                objectEditor = Activator.CreateInstance(objectEditorType, true) as ObjectEditor;
            else if ((objectEditorType = ObjectEditorTypeCache.FirstOrDefault(kv => kv.Key.IsAssignableFrom(_targetObject.GetType())).Value) != null)
                objectEditor = Activator.CreateInstance(objectEditorType, true) as ObjectEditor;
            else
                objectEditor = new ObjectEditor();
            objectEditor.Initialize(_targetObject);
            return objectEditor;
        }

        public object Target { get; set; }

        public Editor Editor { get; set; }

        protected ObjectEditor() { }

        public void Initialize(object _target)
        {
            Target = _target;
        }

        public string GetTitle() { return string.Empty; }

        public virtual void OnEnable() { }

        public virtual void OnHeaderGUI() { }

        public virtual void OnInspectorGUI()
        {
            EditorGUILayoutExtension.DrawFields(ObjectInspector.Instance.targetObject);
        }

        public virtual bool HasPreviewGUI() { return false; }

        public virtual GUIContent GetPreviewTitle() { return null; }

        public virtual void OnPreviewSettings() { }

        public virtual void DrawPreview(Rect previewArea) { }

        public virtual void OnPreviewGUI(Rect _r, GUIStyle _background) { }

        public virtual void OnInteractivePreviewGUI(Rect _r, GUIStyle _background) { }

        public virtual void OnValidate() { }

        public virtual void OnSceneGUI() { }

        public virtual void OnDisable() { }

        public VisualElement CreateInspectorGUI() { return null; }
    }
}
