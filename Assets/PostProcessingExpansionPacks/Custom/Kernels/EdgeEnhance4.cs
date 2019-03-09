using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(EdgeEnhance4Renderer), PostProcessEvent.AfterStack, "Custom/Kernels/EdgeEnhance4")]
    public sealed class EdgeEnhance4 : PostProcessEffectSettings
    {
        
    }

    internal sealed class EdgeEnhance4Renderer : PostProcessEffectRenderer<EdgeEnhance4>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/EdgeEnhance4"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
