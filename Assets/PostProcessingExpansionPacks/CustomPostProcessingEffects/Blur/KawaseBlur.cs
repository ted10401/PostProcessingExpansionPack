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
        private int m_downsample;

        public override void Init()
        {
            m_downsample = Shader.PropertyToID("_Downsample");
        }

        public override void Render(PostProcessRenderContext context)
        {
            context.command.BeginSample("KawaseBlur");

            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Blurs/KawaseBlur"));

            context.GetScreenSpaceTemporaryRT(context.command, m_downsample, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, context.width, context.height);

            var last = context.source;

            context.command.Blit(last, m_downsample);
            last = m_downsample;
            context.command.BlitFullscreenTriangle(last, m_downsample, sheet, 0);
            last = m_downsample;
            context.command.BlitFullscreenTriangle(last, m_downsample, sheet, 1);
            last = m_downsample;
            context.command.BlitFullscreenTriangle(last, m_downsample, sheet, 2);
            last = m_downsample;
            context.command.BlitFullscreenTriangle(last, m_downsample, sheet, 2);
            last = m_downsample;
            context.command.BlitFullscreenTriangle(last, context.destination, sheet, 3);

            context.command.ReleaseTemporaryRT(m_downsample);

            context.command.EndSample("KawaseBlur");
        }
    }
}
