using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(GradientRoberts2x2Renderer), PostProcessEvent.AfterStack, "Custom/Kernels/GradientRoberts2x2")]
    public sealed class GradientRoberts2x2 : PostProcessEffectSettings
    {
        
    }

    internal sealed class GradientRoberts2x2Renderer : PostProcessEffectRenderer<GradientRoberts2x2>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/GradientRoberts2x2"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
