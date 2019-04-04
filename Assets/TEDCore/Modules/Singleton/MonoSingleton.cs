using UnityEngine;

namespace TEDCore
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        private static bool m_applicationIsQuiting;
        private static object m_lock = new object();

        public static T Instance
        {
            get
            {
                if (m_applicationIsQuiting)
                {
                    Debug.LogWarningFormat("[MonoSingleton] - Instance '{0}' already destroyed on application quit. Won't create again - returning null.", typeof(T).Name);

                    return null;
                }

                lock (m_lock)
                {
                    if (m_instance == null)
                    {
                        m_instance = FindObjectOfType<T>();

                        if (FindObjectsOfType<T>().Length > 1)
                        {
                            Debug.LogError("[MonoSingleton] Something went really wrong - there should never be more than 1 singleton! Reopenning the scene might fix it.");

                            return m_instance;
                        }

                        if (m_instance == null)
                        {
                            GameObject singleton = new GameObject();

                            m_instance = singleton.AddComponent<T>();
                            singleton.name = "[MonoSingleton] " + typeof(T).Name;

                            DontDestroyOnLoad(singleton);

                            Debug.LogFormat("[MonoSingleton] - {0} has setup.", typeof(T).Name);
                        }
                        else
                        {
                            Debug.LogFormat("[MonoSingleton] Using instance already created: {0}", m_instance.gameObject.name);
                        }
                    }
                }

                return m_instance;
            }

        }

        protected virtual void OnDestroy()
        {
            m_applicationIsQuiting = true;
            m_instance = null;
        }

        public static bool HasInstance()
        {
            return m_instance != null;
        }
    }
}
