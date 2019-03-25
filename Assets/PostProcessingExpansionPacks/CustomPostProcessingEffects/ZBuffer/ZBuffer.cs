using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(ZBufferRenderer), PostProcessEvent.AfterStack, "Custom/ZBuffer")]
    public sealed class ZBuffer : PostProcessEffectSettings
    {
        
    }

    internal sealed class ZBufferRenderer : PostProcessEffectRenderer<ZBuffer>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/ZBuffer"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
