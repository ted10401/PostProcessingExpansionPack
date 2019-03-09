using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(EdgeEnhance3Renderer), PostProcessEvent.AfterStack, "Custom/Kernels/EdgeEnhance3")]
    public sealed class EdgeEnhance3 : PostProcessEffectSettings
    {
        
    }

    internal sealed class EdgeEnhance3Renderer : PostProcessEffectRenderer<EdgeEnhance3>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/EdgeEnhance3"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
