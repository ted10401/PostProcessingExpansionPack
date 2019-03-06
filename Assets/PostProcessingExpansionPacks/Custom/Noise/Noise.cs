using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(NoiseRenderer), PostProcessEvent.AfterStack, "Custom/Noise")]
    public sealed class Noise : PostProcessEffectSettings
    {
        [Tooltip("Noise texture")]
        public TextureParameter noiseTexture = new TextureParameter { };

        [Tooltip("Noise speed x")]
        public FloatParameter noiseSpeedX = new FloatParameter { value = 0f };

        [Tooltip("Noise speed y")]
        public FloatParameter noiseSpeedY = new FloatParameter { value = 0f };

        [Range(0f, 1f), Tooltip("Blend value")]
        public FloatParameter blend = new FloatParameter { value = 0f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && noiseTexture.value != null;
        }
    }

    internal sealed class NoiseRenderer : PostProcessEffectRenderer<Noise>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Noise"));

            sheet.properties.SetTexture("_NoiseTex", settings.noiseTexture);
            sheet.properties.SetFloat("_NoiseSpeedX", settings.noiseSpeedX);
            sheet.properties.SetFloat("_NoiseSpeedY", settings.noiseSpeedY);
            sheet.properties.SetFloat("_Blend", settings.blend);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
