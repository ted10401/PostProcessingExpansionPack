using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(GaussianBlurRenderer), PostProcessEvent.AfterStack, "Custom/Blurs/GaussianBlur")]
    public sealed class GaussianBlur : PostProcessEffectSettings
    {
        
    }

    internal sealed class GaussianBlurRenderer : PostProcessEffectRenderer<GaussianBlur>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Blurs/GaussianBlur"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
