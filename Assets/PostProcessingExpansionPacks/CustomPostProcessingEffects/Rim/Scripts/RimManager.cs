using TEDCore;
using System.Collections.Generic;

namespace UnityEngine.Rendering.PostProcessing
{
    public class RimManager : Singleton<RimManager>
    {
        public readonly int RIM_COLOR_ID = Shader.PropertyToID("_RimColor");
        public readonly int RIM_POWER_ID = Shader.PropertyToID("_RimPower");
        public readonly int RIM_INTENSITY_ID = Shader.PropertyToID("_RimIntensity");

        private readonly Dictionary<RimType, Shader> m_shaders = new Dictionary<RimType, Shader>()
        {
            { RimType.Normal, Shader.Find("Rim/Prepass/Normal") },
            { RimType.Invert, Shader.Find("Rim/Prepass/Invert") },
        };

        private List<RimData> m_rimDatas = new List<RimData>();
        private PostProcessingSettingsHandler<Rim> m_rimSettings = new PostProcessingSettingsHandler<Rim>();

        public Shader GetShader(RimType rimType)
        {
            return m_shaders[rimType];
        }

        public void Register(RimData rimData)
        {
            for (int i = 0, count = m_rimDatas.Count; i < count; i++)
            {
                if (m_rimDatas[i].parent == rimData.parent)
                {
                    m_rimDatas[i] = rimData;
                    return;
                }
            }

            if (rimData.renderers == null || rimData.renderers.Length == 0)
            {
                return;
            }

            m_rimDatas.Add(rimData);
            UpdateActive();
        }

        public void Unregister(RimData rimData)
        {
            Unregister(rimData.parent);
        }

        public void Unregister(GameObject parent)
        {
            for (int i = 0, count = m_rimDatas.Count; i < count; i++)
            {
                if (m_rimDatas[i].parent == parent)
                {
                    m_rimDatas.RemoveAt(i);
                    UpdateActive();
                    break;
                }
            }
        }

        public void Clear()
        {
            while (m_rimDatas.Count > 0)
            {
                m_rimDatas.RemoveAt(0);
            }

            UpdateActive();
        }

        public void ExecuteCommandBuffer(CommandBuffer commandBuffer)
        {
            for (int i = 0, count = m_rimDatas.Count; i < count; i++)
            {
                for (int j = 0; j < m_rimDatas[i].renderers.Length; j++)
                {
                    commandBuffer.DrawRenderer(m_rimDatas[i].renderers[j], m_rimDatas[i].material);
                }
            }
        }

        private void UpdateActive()
        {
            m_rimSettings.SetActive(m_rimDatas.Count > 0);
        }
    }
}