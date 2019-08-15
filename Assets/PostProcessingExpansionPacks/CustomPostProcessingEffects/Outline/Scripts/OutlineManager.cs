using TEDCore;
using System.Collections.Generic;

namespace UnityEngine.Rendering.PostProcessing
{
    public class OutlineManager : Singleton<OutlineManager>
    {
        public readonly int COLOR_ID = Shader.PropertyToID("_Color");
        private readonly Dictionary<OutlinePrepassType, Shader> m_prepassShaders = new Dictionary<OutlinePrepassType, Shader>
        {
            { OutlinePrepassType.SolidColor, Shader.Find("Outline/Prepass/SolidColor") },
            { OutlinePrepassType.SolidColorDepth, Shader.Find("Outline/Prepass/SolidColorDepth") },
            { OutlinePrepassType.SolidColorDepthInvert, Shader.Find("Outline/Prepass/SolidColorDepthInvert") },
            { OutlinePrepassType.Alpha, Shader.Find("Outline/Prepass/Alpha") },
            { OutlinePrepassType.AlphaDepth, Shader.Find("Outline/Prepass/AlphaDepth") },
            { OutlinePrepassType.AlphaDepthInvert, Shader.Find("Outline/Prepass/AlphaDepthInvert") },
        };

        private List<OutlineData> m_outlineDatas = new List<OutlineData>();
        private PostProcessingSettingsHandler<Outline> m_outlineSettings = new PostProcessingSettingsHandler<Outline>();

        public Shader GetPrepassShader(OutlinePrepassType outlinePrepassType)
        {
            return m_prepassShaders[outlinePrepassType];
        }

        public void Register(GameObject parent, Color color, OutlinePrepassType outlinePrepassType)
        {
            Register(new OutlineData(parent, color, outlinePrepassType));
        }

        public void Register(OutlineData outlineData)
        {
            if (outlineData.renderers == null || outlineData.renderers.Length == 0)
            {
                return;
            }

            for (int i = 0, count = m_outlineDatas.Count; i < count; i++)
            {
                if (m_outlineDatas[i].parent == outlineData.parent)
                {
                    m_outlineDatas[i] = outlineData;
                    break;
                }
            }
            
            m_outlineDatas.Add(outlineData);
            UpdateActive();
        }

        public void Unregister(OutlineData outlineData)
        {
            Unregister(outlineData.parent);
        }

        public void Unregister(GameObject parent)
        {
            for (int i = 0, count = m_outlineDatas.Count; i < count; i++)
            {
                if (m_outlineDatas[i].parent == parent)
                {
                    m_outlineDatas.RemoveAt(i);
                    UpdateActive();
                    break;
                }
            }
        }

        public void Clear()
        {
            while(m_outlineDatas.Count > 0)
            {
                m_outlineDatas.RemoveAt(0);
            }

            UpdateActive();
        }

        public void ExecuteCommandBuffer(CommandBuffer commandBuffer)
        {
            for (int i = 0, count = m_outlineDatas.Count; i < count; i++)
            {
                for (int j = 0; j < m_outlineDatas[i].renderers.Length; j++)
                {
                    commandBuffer.DrawRenderer(m_outlineDatas[i].renderers[j], m_outlineDatas[i].prepassMaterial);
                }
            }
        }

        private void UpdateActive()
        {
            m_outlineSettings.SetActive(m_outlineDatas.Count > 0);
        }
    }
}