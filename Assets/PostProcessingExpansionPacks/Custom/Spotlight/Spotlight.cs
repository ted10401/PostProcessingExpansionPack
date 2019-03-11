using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(SpotlightRenderer), PostProcessEvent.AfterStack, "Custom/Spotlight")]
    public sealed class Spotlight : PostProcessEffectSettings
    {
        [Range(-0.5f, 0.5f)]
        public FloatParameter centerX = new FloatParameter { value = 0f };

        [Range(-0.5f, 0.5f)]
        public FloatParameter centerY = new FloatParameter { value = 0f };

        [Range(0.01f, 0.5f)]
        public FloatParameter radius = new FloatParameter { value = 0.1f };

        [Range(1f, 20f)]
        public FloatParameter sharpness = new FloatParameter { value = 1 };
    }

    internal sealed class SpotlightRenderer : PostProcessEffectRenderer<Spotlight>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Spotlight"));

            sheet.properties.SetFloat("_CenterX", settings.centerX);
            sheet.properties.SetFloat("_CenterY", settings.centerY);
            sheet.properties.SetFloat("_Radius", settings.radius);
            sheet.properties.SetFloat("_Sharpness", settings.sharpness);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
