using UnityEngine;

public class SetupDepthTextureMode : MonoBehaviour
{
    public DepthTextureMode depthTextureMode = DepthTextureMode.None;

    private void Awake()
    {
        if(GetComponent<Camera>() != null)
        {
            GetComponent<Camera>().depthTextureMode = depthTextureMode;
        }
    }
}
