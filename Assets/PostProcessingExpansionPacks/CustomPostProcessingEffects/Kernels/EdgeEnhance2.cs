using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(EdgeEnhance2Renderer), PostProcessEvent.AfterStack, "Custom/Kernels/EdgeEnhance2")]
    public sealed class EdgeEnhance2 : PostProcessEffectSettings
    {
        
    }

    internal sealed class EdgeEnhance2Renderer : PostProcessEffectRenderer<EdgeEnhance2>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/EdgeEnhance2"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
