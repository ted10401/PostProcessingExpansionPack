using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(DepthBufferRenderer), PostProcessEvent.AfterStack, "Custom/DepthBuffer")]
    public sealed class DepthBuffer : PostProcessEffectSettings
    {
        
    }

    internal sealed class DepthBufferRenderer : PostProcessEffectRenderer<DepthBuffer>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/DepthBuffer"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
