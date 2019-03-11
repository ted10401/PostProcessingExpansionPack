using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(SepiaEffectRenderer), PostProcessEvent.AfterStack, "Custom/SepiaEffect")]
    public sealed class SepiaEffect : PostProcessEffectSettings
    {
        public FloatParameter sepiaIntensity = new FloatParameter { value = 1f };
    }

    internal sealed class SepiaEffectRenderer : PostProcessEffectRenderer<SepiaEffect>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/SepiaEffect"));

            sheet.properties.SetFloat("_SepiaIntensity", settings.sepiaIntensity);
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
