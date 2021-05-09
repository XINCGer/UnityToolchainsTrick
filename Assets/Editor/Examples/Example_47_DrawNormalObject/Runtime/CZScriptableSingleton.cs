using System;
using UnityEngine;

namespace CZToolKit.Core.Singletons
{
#if ODIN_INSPECTOR
    public abstract class CZScriptableSingleton<T> : Sirenix.OdinInspector.SerializedScriptableObject where T : CZScriptableSingleton<T>
#else
    public abstract class CZScriptableSingleton<T> : ScriptableObject where T : CZScriptableSingleton<T>
#endif
    {
        /// <summary> 线程锁 </summary>
        private static readonly object m_Lock = new object();

        private static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (m_Lock)
                    {
                        if (m_Instance == null)
                        {
                            m_Instance = ScriptableObject.CreateInstance<T>();

                            if (m_Instance != null)
                                m_Instance.OnInitialize();
                        }
                    }
                }
                return m_Instance;
            }
        }

        public static bool IsNull { get { return m_Instance == null; } }

        public static T Initialize() { return Instance; }

        public static void Clean()
        {
            if (m_Instance != null)
            {
                m_Instance.OnClean();
                m_Instance = null;
            }
        }

        protected virtual void OnInitialize() { }

        protected virtual void OnClean() { }

    }
}

