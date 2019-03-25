using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(EdgeEnhance1Renderer), PostProcessEvent.AfterStack, "Custom/Kernels/EdgeEnhance1")]
    public sealed class EdgeEnhance1 : PostProcessEffectSettings
    {
        
    }

    internal sealed class EdgeEnhance1Renderer : PostProcessEffectRenderer<EdgeEnhance1>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/EdgeEnhance1"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
