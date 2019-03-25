using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(HSBCRenderer), PostProcessEvent.AfterStack, "Custom/HSBC")]
    public sealed class HSBC : PostProcessEffectSettings
    {
        [Range(0f, 1f), Tooltip("Hue effect intensity.")]
        public FloatParameter hue = new FloatParameter { value = 0f };

        [Range(0f, 1f), Tooltip("Saturation effect intensity.")]
        public FloatParameter saturation = new FloatParameter { value = 0.5f };

        [Range(0f, 1f), Tooltip("Brightness effect intensity.")]
        public FloatParameter brightness = new FloatParameter { value = 0.5f };

        [Range(0f, 1f), Tooltip("Contrast effect intensity.")]
        public FloatParameter contrast = new FloatParameter { value = 0.5f };
    }

    internal sealed class HSBCRenderer : PostProcessEffectRenderer<HSBC>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/HSBC"));

            sheet.properties.SetFloat("_Hue", settings.hue);
            sheet.properties.SetFloat("_Saturation", settings.saturation);
            sheet.properties.SetFloat("_Brightness", settings.brightness);
            sheet.properties.SetFloat("_Contrast", settings.contrast);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
