using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(MosaicRenderer), PostProcessEvent.AfterStack, "Custom/Mosaic")]
    public sealed class Mosaic : PostProcessEffectSettings
    {
        [Range(0f, 1f), Tooltip("Pixel size")]
        public FloatParameter pixelSize = new FloatParameter { value = 0f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && pixelSize.value != 0;
        }
    }

    internal sealed class MosaicRenderer : PostProcessEffectRenderer<Mosaic>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Mosaic"));

            sheet.properties.SetFloat("_PixelSize", settings.pixelSize);
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
