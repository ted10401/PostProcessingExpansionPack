using TEDCore;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class DistortionManager : Singleton<DistortionManager>
{
    private List<DistortionData> m_distortionDatas = new List<DistortionData>();

    public void Register(GameObject parent)
    {
        Register(new DistortionData(parent));
    }

    public void Register(DistortionData distortionData)
    {
        if (distortionData.renderer == null)
        {
            return;
        }

        if (m_distortionDatas.Contains(distortionData))
        {
            return;
        }

        m_distortionDatas.Add(distortionData);
    }

    public void Unregister(GameObject parent)
    {
        for(int i = 0, count = m_distortionDatas.Count; i < count; i++)
        {
            if(m_distortionDatas[i].parent == parent)
            {
                m_distortionDatas.RemoveAt(i);
                break;
            }
        }
    }

    public void Unregister(DistortionData distortionData)
    {
        m_distortionDatas.Remove(distortionData);
    }

    public void ExecuteCommandBuffer(CommandBuffer commandBuffer)
    {
        for (int i = 0, count = m_distortionDatas.Count; i < count; i++)
        {
            commandBuffer.DrawRenderer(m_distortionDatas[i].renderer, m_distortionDatas[i].material);
        }
    }
}
