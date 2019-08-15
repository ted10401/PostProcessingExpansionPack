using System.Collections.Generic;

namespace UnityEngine.Rendering.PostProcessing
{
    public class RimData
    {
        private const float DEFAULT_POWER = 2f;
        private const float DEFAULT_INTENSITY = 1.5f;

        public GameObject parent;
        public Renderer[] renderers;
        public RimType rimType;
        public Color color;
        public float power;
        public float intensity;
        public Material material { get; private set; }

        private RimType m_lastRimType;
        private Color m_lastColor;
        private float m_lastPower;
        private float m_lastIntensity;

        public RimData(GameObject parent, RimType rimType, Color color, float power = DEFAULT_POWER, float intensity = DEFAULT_INTENSITY)
        {
            this.parent = parent;

            List<Renderer> cacheRenderers = new List<Renderer>();
            cacheRenderers.AddRange(parent.GetComponentsInChildren<SkinnedMeshRenderer>());
            cacheRenderers.AddRange(parent.GetComponentsInChildren<MeshRenderer>());
            cacheRenderers.AddRange(parent.GetComponentsInChildren<SpriteRenderer>());
            renderers = cacheRenderers.ToArray();

            this.rimType = rimType;
            this.color = color;
            this.power = power;
            this.intensity = intensity;

            UpdateMaterial();
        }

        public RimData(GameObject parent, Material material, Renderer[] renderers, RimType rimType, Color color, float power = DEFAULT_POWER, float intensity = DEFAULT_INTENSITY)
        {
            this.parent = parent;
            this.material = GameObject.Instantiate(material);
            this.m_lastRimType = rimType;
            this.renderers = renderers;
            this.rimType = rimType;
            this.color = color;
            this.power = power;
            this.intensity = intensity;

            UpdateMaterial();
        }

        public void SetRimType(RimType rimType)
        {
            this.rimType = rimType;
            UpdateMaterial();
        }

        public void SetColor(Color color)
        {
            this.color = color;
            UpdateMaterial();
        }

        public void SetAlpha(float alpha)
        {
            color.a = alpha;
            UpdateMaterial();
        }

        public void SetPower(float power)
        {
            this.power = power;
            UpdateMaterial();
        }

        public void SetIntensity(float intensity)
        {
            this.intensity = intensity;
            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            if (material == null || m_lastRimType != rimType)
            {
                m_lastRimType = rimType;

                if (material == null)
                {
                    material = new Material(RimManager.Instance.GetShader(m_lastRimType));
                }
                else
                {
                    material.shader = RimManager.Instance.GetShader(m_lastRimType);
                }

                m_lastColor = color;
                material.SetColor(RimManager.Instance.RIM_COLOR_ID, m_lastColor);

                m_lastPower = power;
                material.SetFloat(RimManager.Instance.RIM_POWER_ID, m_lastPower);

                m_lastIntensity = intensity;
                material.SetFloat(RimManager.Instance.RIM_INTENSITY_ID, m_lastIntensity);
            }
            else
            {
                if (m_lastColor != color)
                {
                    m_lastColor = color;
                    material.SetColor(RimManager.Instance.RIM_COLOR_ID, m_lastColor);
                }

                if (m_lastPower != power)
                {
                    m_lastPower = power;
                    material.SetFloat(RimManager.Instance.RIM_POWER_ID, m_lastPower);
                }

                if (m_lastIntensity != intensity)
                {
                    m_lastIntensity = intensity;
                    material.SetFloat(RimManager.Instance.RIM_INTENSITY_ID, m_lastIntensity);
                }
            }
        }
    }
}