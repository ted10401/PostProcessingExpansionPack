using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(Emboss3x3Renderer), PostProcessEvent.AfterStack, "Custom/Kernels/Emboss3x3")]
    public sealed class Emboss3x3 : PostProcessEffectSettings
    {
        
    }

    internal sealed class Emboss3x3Renderer : PostProcessEffectRenderer<Emboss3x3>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Emboss3x3"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
