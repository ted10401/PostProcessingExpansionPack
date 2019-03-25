using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(KawaseBlurRenderer), PostProcessEvent.AfterStack, "Custom/Blurs/KawaseBlur")]
    public sealed class KawaseBlur : PostProcessEffectSettings
    {

    }

    internal sealed class KawaseBlurRenderer : PostProcessEffectRenderer<KawaseBlur>
    {
        private int m_downsample1;
        private int m_downsample2;

        public override void Init()
        {
            m_downsample1 = Shader.PropertyToID("_Downsample1");
            m_downsample2 = Shader.PropertyToID("_Downsample2");
        }

        public override void Render(PostProcessRenderContext context)
        {
            context.command.BeginSample("KawaseBlur");

            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Blurs/KawaseBlur"));

            context.GetScreenSpaceTemporaryRT(context.command, m_downsample1);
            context.GetScreenSpaceTemporaryRT(context.command, m_downsample2);

            context.command.BlitFullscreenTriangle(context.source, m_downsample1, sheet, 0);
            context.command.BlitFullscreenTriangle(m_downsample1, m_downsample2, sheet, 1);
            context.command.BlitFullscreenTriangle(m_downsample2, m_downsample1, sheet, 2);
            context.command.BlitFullscreenTriangle(m_downsample1, m_downsample2, sheet, 2);
            context.command.BlitFullscreenTriangle(m_downsample2, context.destination, sheet, 3);

            context.command.ReleaseTemporaryRT(m_downsample1);
            context.command.ReleaseTemporaryRT(m_downsample2);

            context.command.EndSample("KawaseBlur");
        }
    }
}
