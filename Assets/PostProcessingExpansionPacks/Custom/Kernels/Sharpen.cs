using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(SharpenRenderer), PostProcessEvent.AfterStack, "Custom/Kernels/Sharpen")]
    public sealed class Sharpen : PostProcessEffectSettings
    {
        
    }

    internal sealed class SharpenRenderer : PostProcessEffectRenderer<Sharpen>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Sharpen"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
