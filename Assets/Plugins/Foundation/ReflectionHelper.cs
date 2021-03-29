using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ToolKits
{
    public class ReflectionHelper
    {
        public ReflectAnimator Animator;

        public ReflectionHelper()
        {
            Animator = new ReflectAnimator();
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
                return (Vector3)_bodyPositionInternal.GetValue(animator, null);
            }
            return Vector3.zero;
        }
    }
}