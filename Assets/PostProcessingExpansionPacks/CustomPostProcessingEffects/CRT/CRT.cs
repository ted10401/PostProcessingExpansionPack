using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(CRTRenderer), PostProcessEvent.AfterStack, "Custom/CRT")]
    public sealed class CRT : PostProcessEffectSettings
    {
        [Range(0f, 1f)]
        public FloatParameter noiseX = new FloatParameter { value = 0f };

        public Vector2Parameter offset = new Vector2Parameter { value = Vector2.zero };

        [Range(0f, 1f)]
        public FloatParameter rgbNoise = new FloatParameter { value = 0f };

        [Range(0f, 10f)]
        public FloatParameter sinNoiseWidth = new FloatParameter { value = 0f };

        [Range(0f, 1f)]
        public FloatParameter sinNoiseScale = new FloatParameter { value = 0f };
        public FloatParameter sinNoiseOffset = new FloatParameter { value = 0f };

        [Range(0f, 2f)]
        public FloatParameter scanLineTail = new FloatParameter { value = 1.5f };

        [Range(-10f, 10f)]
        public FloatParameter scanLineSpeed = new FloatParameter { value = 10f };
    }

    internal sealed class CRTRenderer : PostProcessEffectRenderer<CRT>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/CRT"));

            sheet.properties.SetFloat("_NoiseX", settings.noiseX);
            sheet.properties.SetFloat("_RGBNoise", settings.rgbNoise);
            sheet.properties.SetFloat("_SinNoiseScale", settings.sinNoiseScale);
            sheet.properties.SetFloat("_SinNoiseWidth", settings.sinNoiseWidth);
            sheet.properties.SetFloat("_SinNoiseOffset", settings.sinNoiseOffset);
            sheet.properties.SetFloat("_ScanLineSpeed", settings.scanLineSpeed);
            sheet.properties.SetFloat("_ScanLineTail", settings.scanLineTail);
            sheet.properties.SetVector("_Offset", settings.offset);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
