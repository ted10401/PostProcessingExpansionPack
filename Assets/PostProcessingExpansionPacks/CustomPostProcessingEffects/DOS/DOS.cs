using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(DOSRenderer), PostProcessEvent.AfterStack, "Custom/DOS")]
    public sealed class DOS : PostProcessEffectSettings
    {
        [Range(0.001f, 0.1f)]
        public FloatParameter pixelSize = new FloatParameter { value = 0.001f };
    }

    internal sealed class DOSRenderer : PostProcessEffectRenderer<DOS>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/DOS"));

            sheet.properties.SetFloat("_PixelSize", settings.pixelSize);
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
