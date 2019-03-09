using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(GradientSobel3x3Renderer), PostProcessEvent.AfterStack, "Custom/Kernels/GradientSobel3x3")]
    public sealed class GradientSobel3x3 : PostProcessEffectSettings
    {
        
    }

    internal sealed class GradientSobel3x3Renderer : PostProcessEffectRenderer<GradientSobel3x3>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/GradientSobel3x3"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
