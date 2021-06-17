using UnityEngine;

namespace CZToolKit.Core.ReactiveX
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        /// <summary> 线程锁 </summary>
        private static readonly object m_Lock = new object();

        public static GameObject mgrParent;

        /// <summary> 单例对象 </summary>
        private static MainThreadDispatcher m_Instance;

        public virtual bool DontDestoryOnLoad { get { return true; } }

        /// <summary> 单例对象属性 </summary>
        public static MainThreadDispatcher Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (m_Lock)
                    {
                        if (m_Instance == null)
                        {
                            mgrParent = GetMgrParent();

                            Transform mgrTrans = mgrParent.transform.Find(typeof(MainThreadDispatcher).ToString());

                            if (mgrTrans == null)
                            {
                                mgrTrans = new GameObject(typeof(MainThreadDispatcher).Name).transform;
                                m_Instance = mgrTrans.gameObject.AddComponent<MainThreadDispatcher>();
                                mgrTrans.transform.SetParent(mgrParent.transform);
                            }
                            else
                            {
                                m_Instance = mgrTrans.gameObject.GetComponent<MainThreadDispatcher>();
                                if (m_Instance == null)
                                    m_Instance = mgrTrans.gameObject.AddComponent<MainThreadDispatcher>();
                            }
                            if (m_Instance != null)
                                m_Instance.OnInitialize();
                        }
                    }
                }
                return m_Instance;
            }
        }

        public static bool IsNull { get { return m_Instance == null; } }

        protected virtual void Awake()
        {
            m_Instance = this;
            if (DontDestoryOnLoad)
                DontDestroyOnLoad(m_Instance.gameObject);
        }

        public static GameObject GetMgrParent()
        {
            if (mgrParent == null)
            {
                mgrParent = GameObject.Find("CZManagers");
                if (mgrParent == null)
                {
                    mgrParent = new GameObject("CZManagers");
                    DontDestroyOnLoad(mgrParent);
                }
            }
            return mgrParent;
        }

        public static MainThreadDispatcher Initialize()
        {
            return Instance;
        }

        public static void Clean()
        {
            if (m_Instance != null)
            {
                m_Instance.OnClean();
                Destroy(m_Instance.gameObject);
                m_Instance = null;
            }
        }

        protected virtual void OnInitialize() { }

        protected virtual void OnClean()
        {
            StopAllCoroutines();
        }
    }
}
