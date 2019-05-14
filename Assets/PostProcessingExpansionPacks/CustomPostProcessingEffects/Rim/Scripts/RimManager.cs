using TEDCore;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class RimManager : Singleton<RimManager>
{
    public readonly int RIM_COLOR_ID = Shader.PropertyToID("_RimColor");
    public readonly int RIM_POWER_ID = Shader.PropertyToID("_RimPower");
    public readonly int RIM_INTENSITY_ID = Shader.PropertyToID("_RimIntensity");
    private readonly Shader PREPASS_SHADER = Shader.Find("Rim/Prepass");

    private List<RimData> m_rimDatas = new List<RimData>();

    public Shader GetPrepassShader()
    {
        return PREPASS_SHADER;
    }

    public void Register(GameObject parent, Color color, float power, float intensity)
    {
        Register(new RimData(parent, color, power, intensity));
    }

    public void Register(RimData rimData)
    {
        if (rimData.renderers == null || rimData.renderers.Length == 0)
        {
            return;
        }

        if (m_rimDatas.Contains(rimData))
        {
            return;
        }

        m_rimDatas.Add(rimData);
    }

    public void Unregister(GameObject parent)
    {
        for(int i = 0, count = m_rimDatas.Count; i < count; i++)
        {
            if(m_rimDatas[i].parent == parent)
            {
                m_rimDatas.RemoveAt(i);
                break;
            }
        }
    }

    public void Unregister(RimData rimData)
    {
        m_rimDatas.Remove(rimData);
    }

    public void ExecuteCommandBuffer(CommandBuffer commandBuffer)
    {
        for (int i = 0, count = m_rimDatas.Count; i < count; i++)
        {
            for (int j = 0; j < m_rimDatas[i].renderers.Length; j++)
            {
                commandBuffer.DrawRenderer(m_rimDatas[i].renderers[j], m_rimDatas[i].prepassMaterial);
            }
        }
    }
}
