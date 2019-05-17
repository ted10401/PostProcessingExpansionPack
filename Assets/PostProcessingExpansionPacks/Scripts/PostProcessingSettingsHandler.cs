
namespace UnityEngine.Rendering.PostProcessing
{
    public class PostProcessingSettingsHandler<T> where T : PostProcessEffectSettings
    {
        public Camera camera
        {
            get
            {
                if (m_camera == null)
                {
                    m_camera = Camera.main;
                }

                return m_camera;
            }
        }
        private Camera m_camera;

        public PostProcessVolume postProcessVolume
        {
            get
            {
                if (m_postProcessVolume == null)
                {
                    if (camera != null)
                    {
                        m_postProcessVolume = camera.GetComponent<PostProcessVolume>();
                    }
                    else
                    {
                        m_postProcessVolume = GameObject.FindObjectOfType<PostProcessVolume>();
                    }
                }

                return m_postProcessVolume;
            }
        }
        private PostProcessVolume m_postProcessVolume;
        private PostProcessProfile m_lastPostProcessProfile;

        public T settings
        {
            get
            {
                if (postProcessVolume == null)
                {
                    m_settings = null;
                    return m_settings;
                }

                if (m_settings == null || m_lastPostProcessProfile != postProcessVolume.sharedProfile)
                {
                    m_settings = null;
                    m_lastPostProcessProfile = postProcessVolume.sharedProfile;

                    if (m_lastPostProcessProfile != null)
                    {
                        m_settings = m_lastPostProcessProfile.GetSetting<T>();

                        if (m_settings == null)
                        {
                            m_settings = m_lastPostProcessProfile.AddSettings<T>();
                        }
                    }
                }

                return m_settings;
            }
        }
        private T m_settings;

        public void SetActive(bool active)
        {
            if(settings == null)
            {
                return;
            }

            settings.active = active;
        }
    }
}