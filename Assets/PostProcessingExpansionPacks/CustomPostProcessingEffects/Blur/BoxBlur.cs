using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(BoxBlurRenderer), PostProcessEvent.AfterStack, "Custom/Blurs/BoxBlur")]
    public sealed class BoxBlur : PostProcessEffectSettings
    {
        
    }

    internal sealed class BoxBlurRenderer : PostProcessEffectRenderer<BoxBlur>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Blurs/BoxBlur"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
