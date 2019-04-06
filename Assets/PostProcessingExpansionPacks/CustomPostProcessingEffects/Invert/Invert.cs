using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(InvertRenderer), PostProcessEvent.AfterStack, "Custom/Invert")]
    public sealed class Invert : PostProcessEffectSettings
    {
        
    }

    internal sealed class InvertRenderer : PostProcessEffectRenderer<Invert>
    {
        private Shader m_shader;
        
        public override void Init()
        {
            m_shader = Shader.Find("Hidden/Custom/Invert");
        }
        
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(m_shader);
            
            
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
