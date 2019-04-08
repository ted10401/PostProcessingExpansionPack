using UnityEngine;

[ExecuteInEditMode]
public class DistortionComponent : MonoBehaviour
{
    private DistortionData m_distortionData;

    private void OnEnable()
    {
        if(m_distortionData == null)
        {
            m_distortionData = new DistortionData(gameObject);
        }

        DistortionManager.Instance.Register(m_distortionData);
    }

    private void OnDisable()
    {
        DistortionManager.Instance.Unregister(m_distortionData);
    }
}
