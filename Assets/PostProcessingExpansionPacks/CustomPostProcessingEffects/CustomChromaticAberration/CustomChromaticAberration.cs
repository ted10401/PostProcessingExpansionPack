using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(CustomChromaticAberrationRenderer), PostProcessEvent.AfterStack, "Custom/CustomChromaticAberration")]
    public sealed class CustomChromaticAberration : PostProcessEffectSettings
    {
        [Range(-0.5f, 0.5f)]
        public FloatParameter redX = new FloatParameter { value = 0f };

        [Range(-0.5f, 0.5f)]
        public FloatParameter redY = new FloatParameter { value = 0f };

        [Range(-0.5f, 0.5f)]
        public FloatParameter greenX = new FloatParameter { value = 0f };

        [Range(-0.5f, 0.5f)]
        public FloatParameter greenY = new FloatParameter { value = 0f };

        [Range(-0.5f, 0.5f)]
        public FloatParameter blueX = new FloatParameter { value = 0f };

        [Range(-0.5f, 0.5f)]
        public FloatParameter blueY = new FloatParameter { value = 0f };
    }

    internal sealed class CustomChromaticAberrationRenderer : PostProcessEffectRenderer<CustomChromaticAberration>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/CustomChromaticAberration"));

            sheet.properties.SetFloat("_RedX", settings.redX);
            sheet.properties.SetFloat("_RedY", settings.redY);
            sheet.properties.SetFloat("_GreenX", settings.greenX);
            sheet.properties.SetFloat("_GreenY", settings.greenY);
            sheet.properties.SetFloat("_BlueX", settings.blueX);
            sheet.properties.SetFloat("_BlueY", settings.blueY);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
