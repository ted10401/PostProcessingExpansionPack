using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(SepiaRenderer), PostProcessEvent.AfterStack, "Custom/Sepia")]
    public sealed class Sepia : PostProcessEffectSettings
    {
        public FloatParameter sepiaIntensity = new FloatParameter { value = 1f };
    }

    internal sealed class SepiaRenderer : PostProcessEffectRenderer<Sepia>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Sepia"));

            sheet.properties.SetFloat("_SepiaIntensity", settings.sepiaIntensity);
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
