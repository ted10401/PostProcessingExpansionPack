using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(AccumulationBufferRenderer), PostProcessEvent.AfterStack, "Custom/AccumulationBuffer")]
    public sealed class AccumulationBuffer : PostProcessEffectSettings
    {
        [Range(0f, 1f)]
        public FloatParameter alpha = new FloatParameter { value = 0.1f };
    }

    internal sealed class AccumulationBufferRenderer : PostProcessEffectRenderer<AccumulationBuffer>
    {
        private Shader m_shader;
        private RenderTexture m_accumulationBuffer;

        public override void Init()
        {
            m_shader = Shader.Find("Hidden/Custom/AccumulationBuffer");
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(m_shader);
            sheet.properties.SetFloat("_Alpha", settings.alpha);

            if(m_accumulationBuffer == null || m_accumulationBuffer.width != context.width || m_accumulationBuffer.height != context.height)
            {
                if(m_accumulationBuffer != null)
                {
                    m_accumulationBuffer.Release();
                    m_accumulationBuffer = null;
                }

                m_accumulationBuffer = context.GetScreenSpaceTemporaryRT();
            }

            context.command.BlitFullscreenTriangle(context.source, m_accumulationBuffer, sheet, 0);
            context.command.Blit(m_accumulationBuffer, context.destination);
        }

        public override void Release()
        {
            if(m_accumulationBuffer != null)
            {
                m_accumulationBuffer.Release();
                m_accumulationBuffer = null;
            }
        }
    }
}
