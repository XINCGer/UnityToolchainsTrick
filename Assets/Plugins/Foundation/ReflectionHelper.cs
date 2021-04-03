using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ToolKits
{
    public class ReflectionHelper
    {
        public ReflectAnimator Animator;
        public RefHandleUtility HandleUtility;
        public RefBlendTree BlendTree;

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
            _applyWireMaterial = _type.GetMethod("ApplyWireMaterial", BindingFlags.NonPublic | BindingFlags.Static);
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
}