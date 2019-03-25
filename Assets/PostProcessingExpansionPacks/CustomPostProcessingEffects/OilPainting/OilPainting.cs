using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(OilPaintingRenderer), PostProcessEvent.AfterStack, "Custom/OilPainting")]
    public sealed class OilPainting : PostProcessEffectSettings
    {
        public IntParameter radius = new IntParameter { value = 0 };
    }

    internal sealed class OilPaintingRenderer : PostProcessEffectRenderer<OilPainting>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/OilPainting"));

            sheet.properties.SetInt("_Radius", settings.radius);
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
