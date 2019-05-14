using UnityEngine;
using System.Collections.Generic;

public class RimData
{
    public GameObject parent;
    public Renderer[] renderers;
    public Color color;
    public float power;
    public float intensity;
    public Material prepassMaterial { get; private set; }

    private Color m_lastColor;
    private float m_lastPower;
    private float m_lastIntensity;

    public RimData(GameObject parent, Color color, float power, float intensity)
    {
        this.parent = parent;

        List<Renderer> cacheRenderers = new List<Renderer>();
        cacheRenderers.AddRange(parent.GetComponentsInChildren<SkinnedMeshRenderer>());
        cacheRenderers.AddRange(parent.GetComponentsInChildren<MeshRenderer>());
        cacheRenderers.AddRange(parent.GetComponentsInChildren<SpriteRenderer>());
        renderers = cacheRenderers.ToArray();

        this.color = color;
        this.power = power;
        this.intensity = intensity;

        UpdatePrepassMaterial();
    }

    public void SetColor(Color color)
    {
        this.color = color;
        UpdatePrepassMaterial();
    }

    public void SetPower(float power)
    {
        this.power = power;
        UpdatePrepassMaterial();
    }

    public void SetIntensity(float intensity)
    {
        this.intensity = intensity;
        UpdatePrepassMaterial();
    }

    private void UpdatePrepassMaterial()
    {
        if (prepassMaterial == null)
        {
            prepassMaterial = new Material(RimManager.Instance.GetPrepassShader());

            m_lastColor = color;
            prepassMaterial.SetColor(RimManager.Instance.RIM_COLOR_ID, m_lastColor);

            m_lastPower = power;
            prepassMaterial.SetFloat(RimManager.Instance.RIM_POWER_ID, m_lastPower);

            m_lastIntensity = intensity;
            prepassMaterial.SetFloat(RimManager.Instance.RIM_INTENSITY_ID, m_lastIntensity);
        }
        else
        {
            if(m_lastColor != color)
            {
                m_lastColor = color;
                prepassMaterial.SetColor(RimManager.Instance.RIM_COLOR_ID, m_lastColor);
            }

            if (m_lastPower != power)
            {
                m_lastPower = power;
                prepassMaterial.SetFloat(RimManager.Instance.RIM_POWER_ID, m_lastPower);
            }

            if (m_lastIntensity != intensity)
            {
                m_lastIntensity = intensity;
                prepassMaterial.SetFloat(RimManager.Instance.RIM_INTENSITY_ID, m_lastIntensity);
            }
        }
    }
}
