#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ToolKits
{
    public class ReflectionHelper
    {
        public ReflectAnimator Animator;
        public RefHandleUtility HandleUtility;
        public RefBlendTree BlendTree;
        public RefModelImporter ModelImporter;
        public RefCamera Camera;
        public RefEditorUtility EditorUtility;
        public RefAvatarPreviewSelection AvatarPreviewSelection;

        private static ReflectionHelper _instance;

        public static ReflectionHelper Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new ReflectionHelper();
                }

                return _instance;
            }
        }

        private ReflectionHelper()
        {
            Animator = new ReflectAnimator();
            HandleUtility = new RefHandleUtility();
            BlendTree = new RefBlendTree();
            ModelImporter = new RefModelImporter();
            Camera = new RefCamera();
            EditorUtility = new RefEditorUtility();
            AvatarPreviewSelection = new RefAvatarPreviewSelection();
        }
    }

    public class ReflectAnimator
    {
        private Type _type;

        private PropertyInfo _bodyPositionInternal;

        public ReflectAnimator()
        {
            _type = typeof(Animator);
            _bodyPositionInternal =
                _type.GetProperty("bodyPositionInternal", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public Vector3 GetBodyPosition(Animator animator)
        {
            if (null != _bodyPositionInternal)
            {
                return (Vector3) _bodyPositionInternal.GetValue(animator, null);
            }

            return Vector3.zero;
        }
    }

    public class RefHandleUtility
    {
        private Type _type;

        private MethodInfo _applyWireMaterial;

        public RefHandleUtility()
        {
            _type = typeof(HandleUtility);
            _applyWireMaterial = _type.GetMethod("ApplyWireMaterial", new Type[]{});
        }

        public void ApplyWireMaterial()
        {
            if (null != _applyWireMaterial)
            {
                _applyWireMaterial.Invoke(null, null);
            }
        }
    }

    public class RefBlendTree
    {
        private Type _type;
        private MethodInfo _getAnimationClipsFlattened;

        public RefBlendTree()
        {
            _type = typeof(BlendTree);
            _getAnimationClipsFlattened = _type.GetMethod("GetAnimationClipsFlattened",
                BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public AnimationClip[] GetAnimationClipsFlattened(BlendTree blendTree)
        {
            if (null != _getAnimationClipsFlattened)
            {
                return _getAnimationClipsFlattened.Invoke(blendTree, null) as AnimationClip[];
            }

            return null;
        }
    }

    public class RefModelImporter
    {
        private Type _type;
        private MethodInfo _calculateBestFittingPreviewGameObject;

        public RefModelImporter()
        {
            _type = typeof(ModelImporter);
            _calculateBestFittingPreviewGameObject = _type.GetMethod("CalculateBestFittingPreviewGameObject",
                BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public string CalculateBestFittingPreviewGameObject()
        {
            if (null != _calculateBestFittingPreviewGameObject)
            {
                return _calculateBestFittingPreviewGameObject.Invoke(null, null) as string;
            }

            return string.Empty;
        }
    }

    public class RefCamera
    {
        private Type _type;
        private FieldInfo _fieldInfo;
        private const int kPreviewCullingLayer = 31;

        public RefCamera()
        {
            _type = typeof(Camera);
            _fieldInfo = _type.GetField("PreviewCullingLayer", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public int PreviewCullingLayer
        {
            get
            {
                if (null != _fieldInfo)
                {
                    return (int) _fieldInfo.GetValue(null);
                }

                return kPreviewCullingLayer;
            }
        }
    }

    public class RefEditorUtility
    {
        private Type _type;
        private MethodInfo _methodInfo;

        public RefEditorUtility()
        {
            _type = typeof(EditorUtility);
            _methodInfo = _type.GetMethod("InstantiateForAnimatorPreview",
                BindingFlags.Static | BindingFlags.NonPublic);
        }

        public GameObject InstantiateForAnimatorPreview(Object original)
        {
            if (null != _methodInfo)
            {
                return _methodInfo.Invoke(null, new[] {original}) as GameObject;
            }

            return null;
        }
    }

    public class RefAvatarPreviewSelection
    {
        private Type _type;
        private MethodInfo _methodInfo;

        public RefAvatarPreviewSelection()
        {
            _type = Type.GetType("UnityEditor.AvatarPreviewSelection,UnityEditor");
            _methodInfo = _type.GetMethod("SetPreview", BindingFlags.Static | BindingFlags.Public);
        }

        public void SetPreview(ModelImporterAnimationType type, GameObject go)
        {
            if (null != _methodInfo)
            {
                _methodInfo.Invoke(null, new object[] {type, go});
            }
        }
    }
}
#endif