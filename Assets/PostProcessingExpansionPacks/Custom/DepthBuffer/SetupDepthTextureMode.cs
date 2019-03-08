using UnityEngine;

public class SetupDepthTextureMode : MonoBehaviour
{
    [SerializeField] private DepthTextureMode m_depthTextureMode;

    private void Awake()
    {
        if(GetComponent<Camera>() != null)
        {
            GetComponent<Camera>().depthTextureMode = m_depthTextureMode;
        }
    }
}
