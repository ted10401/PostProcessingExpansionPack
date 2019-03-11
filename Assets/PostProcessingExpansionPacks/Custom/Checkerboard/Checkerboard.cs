using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(CheckerboardRenderer), PostProcessEvent.AfterStack, "Custom/Checkerboard")]
    public sealed class Checkerboard : PostProcessEffectSettings
    {
        public ColorParameter color = new ColorParameter { value = Color.white };
        [Range(0f, 1f)]
        public FloatParameter size = new FloatParameter { value = 1f };
    }

    internal sealed class CheckerboardRenderer : PostProcessEffectRenderer<Checkerboard>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Checkerboard"));

            sheet.properties.SetColor("_Color", settings.color);
            sheet.properties.SetFloat("_Size", settings.size);
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
