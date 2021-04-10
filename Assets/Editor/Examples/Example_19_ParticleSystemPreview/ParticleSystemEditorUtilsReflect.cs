using System;
using UnityEngine;
using System.Reflection;
using UnityEditor;

namespace ToolKits
{
    public class ParticleSystemEditorUtilsReflect
    {
        private static Type realType;
        private static Type realType2;
        private static PropertyInfo property_editorResimulation;
        private static PropertyInfo property_editorPlaybackTime;
        private static Func<float> getFunc_editorPlaybackTime;
        private static PropertyInfo property_editorIsScrubbing;
        private static PropertyInfo property_lockedParticleSystem;
        private static MethodInfo method_StopEffect;

        public static void InitType()
        {
            if (realType == null)
            {
                var assembly = Assembly.GetAssembly(typeof(Editor));
                realType = assembly.GetType("UnityEditor.ParticleSystemEditorUtils");

#if UNITY_2018_1_OR_NEWER
                property_editorResimulation =
                    realType.GetProperty("resimulation", BindingFlags.Static | BindingFlags.NonPublic);
                property_editorPlaybackTime =
                    realType.GetProperty("playbackTime", BindingFlags.Static | BindingFlags.NonPublic);

                getFunc_editorPlaybackTime = (Func<float>) Delegate.CreateDelegate(typeof(Func<float>),
                    property_editorPlaybackTime.GetGetMethod(true));
                property_editorIsScrubbing =
                    realType.GetProperty("playbackIsScrubbing", BindingFlags.Static | BindingFlags.NonPublic);
                property_lockedParticleSystem = realType.GetProperty("lockedParticleSystem",
                    BindingFlags.Static | BindingFlags.NonPublic);

                realType2 = assembly.GetType("UnityEditor.ParticleSystemEffectUtils");
                method_StopEffect = realType2.GetMethod("StopEffect", BindingFlags.Static | BindingFlags.NonPublic,
                    null, new Type[] { }, new ParameterModifier[] { });
#else
            property_editorResimulation =
 realType.GetProperty("editorResimulation", BindingFlags.Static | BindingFlags.NonPublic);
            property_editorPlaybackTime =
 realType.GetProperty("editorPlaybackTime", BindingFlags.Static | BindingFlags.NonPublic);

            getFunc_editorPlaybackTime =
 (Func<float>)Delegate.CreateDelegate(typeof(Func<float>), property_editorPlaybackTime.GetGetMethod(true));
            property_editorIsScrubbing =
 realType.GetProperty("editorIsScrubbing", BindingFlags.Static | BindingFlags.NonPublic);
            property_lockedParticleSystem =
 realType.GetProperty("lockedParticleSystem", BindingFlags.Static | BindingFlags.NonPublic);
            method_StopEffect =
 realType.GetMethod("StopEffect", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { }, new ParameterModifier[] { });
#endif
            }
        }

        public static bool editorResimulation
        {
            set
            {
                InitType();
                property_editorResimulation.SetValue(null, value, null);
            }
        }

        public static float editorPlaybackTime
        {
            get
            {
                InitType();
                return getFunc_editorPlaybackTime();
            }
            set
            {
                InitType();
                property_editorPlaybackTime.SetValue(null, value, null);
            }
        }

        public static bool editorIsScrubbing
        {
            set
            {
                InitType();
                property_editorIsScrubbing.SetValue(null, value, null);
            }
        }

        public static ParticleSystem lockedParticleSystem
        {
            get
            {
                InitType();
                return (ParticleSystem) property_lockedParticleSystem.GetValue(null, null);
            }
            set
            {
                InitType();
                property_lockedParticleSystem.SetValue(null, value, null);
            }
        }

        public static void StopEffect()
        {
            InitType();
            method_StopEffect.Invoke(null, null);
        }

        public static ParticleSystem GetRoot(ParticleSystem ps)
        {
            if (ps == null)
            {
                return null;
            }

            Transform transform = ps.transform;
            while (transform.parent && transform.parent.gameObject.GetComponent<ParticleSystem>() != null)
            {
                transform = transform.parent;
            }

            return transform.gameObject.GetComponent<ParticleSystem>();
        }
    }
}