using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(PencilRenderer), PostProcessEvent.AfterStack, "Custom/Pencil")]
    public sealed class Pencil : PostProcessEffectSettings
    {
        
    }

    internal sealed class PencilRenderer : PostProcessEffectRenderer<Pencil>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Pencil"));
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
