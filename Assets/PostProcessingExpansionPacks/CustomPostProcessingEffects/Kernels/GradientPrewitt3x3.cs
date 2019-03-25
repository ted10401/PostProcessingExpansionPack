using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(GradientPrewitt3x3Renderer), PostProcessEvent.AfterStack, "Custom/Kernels/GradientPrewitt3x3")]
    public sealed class GradientPrewitt3x3 : PostProcessEffectSettings
    {
        
    }

    internal sealed class GradientPrewitt3x3Renderer : PostProcessEffectRenderer<GradientPrewitt3x3>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/GradientPrewitt3x3"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
