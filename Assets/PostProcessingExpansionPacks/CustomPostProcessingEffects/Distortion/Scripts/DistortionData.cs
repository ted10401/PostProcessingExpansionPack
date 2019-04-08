using UnityEngine;

public class DistortionData
{
    public GameObject parent;
    public Renderer renderer;
    public Material material { get; private set; }

    public DistortionData(GameObject gameObject)
    {
        parent = gameObject;
        renderer = gameObject.GetComponent<Renderer>();
        material = renderer.sharedMaterial;
    }
}
